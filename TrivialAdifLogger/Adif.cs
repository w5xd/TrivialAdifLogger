using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AdifLog
{
    /* read/write logbook from/to ADIF file */
    class Adif
    {
        public Adif(String fp, LogBook lb)
        {
            FilePath = fp;
            logBook = lb;
        }

        string FilePath;
        LogBook logBook;

        static void WriteAdif(System.IO.StreamWriter writer, bool writeIfNull, string tag, string value = null, bool withEOL = false)
        {
            if (String.IsNullOrEmpty(value))
            {
                if (!writeIfNull)
                    return;
                writer.Write("<" + tag + ">");
            }
            else
                writer.Write(String.Format("<{0}:{2}>{1}", tag, value, value.Length));
            if (withEOL)
                writer.WriteLine();
        }

        // one QSO to file
        public static void saveQso(System.IO.StreamWriter writer, LogBook.LogEntry q)
        {
            WriteAdif(writer, false, "FREQ", (q.rxKhz * .001).ToString());
            WriteAdif(writer, false, "STATION_CALLSIGN", q.items.mycall);
            WriteAdif(writer, false, "CALL", q.items.hiscall);
            WriteAdif(writer, false, "QSO_DATE", q.items.adifDate);
            WriteAdif(writer, false, "TIME_ON", q.items.adifTime);
            WriteAdif(writer, false, "MY_GRIDSQUARE", q.items.sentGrid);
            WriteAdif(writer, false, "STX", q.items.sentSerialNumber.ToString());
            if (q.items.digitalMode == "FT8")
                WriteAdif(writer, false, "MODE", q.items.digitalMode);
            else if (q.items.digitalMode == "FT4")
            {
                WriteAdif(writer, false, "MODE", "MFSK");
                WriteAdif(writer, false, "SUBMODE", "FT4");
            }
            foreach (var s in q.fields)
                WriteAdif(writer, false, s.Key, s.Value);
            WriteAdif(writer, true, "EOR", null, true);
        }

        public bool fileSave()
        {
            try
            {
                using (var writer = new System.IO.StreamWriter(FilePath))
                {
                    writer.WriteLine("ADIF file by Trival ADIF logger");
                    WriteAdif(writer, true, "PROGRAMID", "TrivialAdifLogger", true);
                    WriteAdif(writer, true, "EOH", null, true);
                    foreach (var q in logBook)
                        saveQso(writer, q);
                }
                return true;
            }
            catch (System.Exception e)
            {
                MessageBox.Show("File write of \"" + FilePath + "\" failed.\r\n" + e.Message);
            }
            return false;
        }

        private delegate void OnAdifEOR(Dictionary<string, string> values);

        private void OnReadQsoComplete(Dictionary<string, string> values)
        {
            var logBookItem = new ItemsToLog();
            string v;
            string key;
            key = "STATION_CALLSIGN";
            if (values.TryGetValue(key, out v))
            {
                logBookItem.mycall = v;
                values.Remove(key);
            }
            key = "CALL";
            if (values.TryGetValue(key, out v))
            {
                logBookItem.hiscall = v;
                values.Remove(key);
            }
            key = "QSO_DATE";
            if (values.TryGetValue(key, out v))
            {
                logBookItem.adifDate = v;
                values.Remove(key);
            }
            key = "TIME_ON";
            if (values.TryGetValue(key, out v))
            {
                logBookItem.adifTime = v;
                values.Remove(key);
            }
            key = "MY_GRIDSQUARE";
            if (values.TryGetValue(key, out v))
            {
                logBookItem.sentGrid = v;
                values.Remove(key);
            }
            key = "STX";
            if (values.TryGetValue(key, out v))
            {
                logBookItem.sentSerialNumber = UInt32.Parse(v);
                values.Remove(key);
            }

            // The ADIF spec for FT8/FT4 is asymmetrical
            key = "MODE";
            if (values.TryGetValue(key, out v))
            {
                if (v.ToUpper() == "FT8")
                    logBookItem.digitalMode = v;
                else if (v.ToUpper() == "MFSK")
                {
                    string key2 = "SUBMODE";
                    if (values.TryGetValue(key2, out v))
                    {
                        if (v.ToUpper() == "FT4")
                            logBookItem.digitalMode = "FT4";
                        values.Remove(key2);
                    }
                }
                values.Remove(key);
            }

            double rxKhz = 0;
            key = "FREQ";
            if (values.TryGetValue(key, out v))
            {
                rxKhz = Double.Parse(v);
                rxKhz *= 1000;
                values.Remove(key);
            }

            logBook.AddToLog(logBookItem, 
                new Dictionary<string,string>(values), // the logbook needs its own copy of values
                rxKhz);
        }

        // simple ADIF parser based on this delegate signature:
        delegate void AdifElementParser(char c, ref AdifElementParser newDel);
        abstract class AdifParser
        {
            protected abstract void OnValueComplete(ref AdifElementParser newDel);

            public AdifElementParser begin()
            {  return new AdifElementParser(start);  }

            protected void start(char c, ref AdifElementParser newDel)
            {
                if (c == '<')
                {
                    // found beginning of a new tag
                    newDel = new AdifElementParser(parseTag);
                    curTag = "";
                    curValue = "";
                    len = 0;
                }
            }

            void parseTag(char c, ref AdifElementParser newDel)
            {
                if (c == ':')
                {
                    len = 0;
                    newDel = new AdifElementParser(valueLen);
                }
                else if (c != '>')
                    curTag += c;
                else
                    OnTagComplete(ref newDel);
            }

            protected void OnTagComplete(ref AdifElementParser newDel)
            {
                if (len == 0)
                    OnValueComplete(ref newDel);
                else
                    newDel = new AdifElementParser(parseValue);
            }

            void valueLen(char c, ref AdifElementParser newDel)
            {
                if (Char.IsDigit(c))
                {
                    len *= 10;
                    len += (int)(c - '0');
                }
                else if (c == '>')
                    OnTagComplete(ref newDel);
            }

            void parseValue(char c, ref AdifElementParser newDel)
            {
                curValue += c;
                len -= 1;
                if (len == 0)
                     OnValueComplete(ref newDel);
            }

            protected string curTag { get; private set; }
            protected string curValue { get; private set; }
            private int len = 0;
        }

        // once past the ADIF header, this class parses
        class AdifQsoParser : AdifParser
        {
            OnAdifEOR onEor;
            public AdifQsoParser(OnAdifEOR onEor)
            {
                this.onEor = onEor;
            }

            protected override void OnValueComplete(ref AdifElementParser newDel)
            {
                newDel = new AdifElementParser(start);
                if (curTag.ToUpper() == "EOR")
                {   // got a full record. send it to the delegate
                    onEor(values);
                    values.Clear();
                }
                else
                    values[curTag] = curValue;
            }

            Dictionary<string, string> values = new Dictionary<string, string>();
        }

        // Start looking for the header
        class AdifHeaderParser : AdifParser
        {
            OnAdifEOR onEor;
            public AdifHeaderParser(OnAdifEOR onEor)
            {
                this.onEor = onEor;
            }
            protected override void OnValueComplete(ref AdifElementParser newDel)
            {   // we're the Header parser. we finish on EOH
                if (curTag.ToUpper() == "EOH")
                    newDel = new AdifQsoParser(onEor).begin();
                else
                    newDel = new AdifElementParser(start);
            }
        }

        public void fileOpen()
        {
            try
            {
                AdifElementParser p = new AdifHeaderParser(OnReadQsoComplete).begin();
                using (var reader = new System.IO.StreamReader(FilePath))
                {
                    bool first = true;
                    while (reader.Peek() >= 0)
                    {
                        int c = reader.Read();
                        // ADIF spec says if first character is <, then no <EOH>
                        if (first && c == '<')
                            p = new AdifQsoParser(OnReadQsoComplete).begin();
                        first = false;
                        p((char)c, ref p);
                    }
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show("File read of \"" + FilePath + "\" failed.\r\n" + e.Message);
            }
        }
    }
}
