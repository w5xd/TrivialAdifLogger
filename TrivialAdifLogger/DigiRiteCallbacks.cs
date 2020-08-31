using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace AdifLog
{
    // This is the class that DigiRite calls out-of-process
    [ComVisible(true)]
    public class DigiRiteCallbacks 
        : StandardOleMarshalObject // put this object on STA thread so we don't have to sync with the UI thread
    {
        public DigiRiteCallbacks(MainForm mf, LogBook lb, HamlibThreadWrapper r)
        {
            mainForm = mf;
            logBook = lb;
            rig = r;
        }
        private MainForm mainForm;

        public HamlibThreadWrapper rig { private get; set; } = null;
        private LogBook logBook {  get;  set; } = null;

        [ComVisible(true)]
        public string CallUsed { get; set; } = "";

        [ComVisible(true)]
        public string GridUsed { private get; set; } = "";

        [ComVisible(true)]
        public void CheckDupeAndMult(string call, string digitalMode, string m, out bool dupe, out short mult, int i3, int n3)
        {
            logBook.CheckDupe(call, rig.getFrequency(), out dupe);
            mult = 0;
        }

        [ComVisible(true)]
        public void ForceRigToUsb()
        {
            if (null != rig)
                rig.setMode(HamLibClr.Mode_t.MODE_USB);
        }

        [ComVisible(true)]
        public short GetCurrentBand()
        {
            if (null == rig)
                return 0;
            return (short)LogBook.bandIdx(rig.getFrequency());       
        }

        [ComVisible(true)]
        public void GetRigFrequency(out double rxKHz, out double txKHz, out bool split)
        {
            rxKHz = 0;
            txKHz = 0;
            split = false;
            HamLibClr.Mode_t m = HamLibClr.Mode_t.MODE_CW;
            if (null != rig)
               rig.getFrequencyAndMode(ref rxKHz, ref txKHz, ref m, ref split);
        }

        [ComVisible(true)]
        public uint GetSendSerialNumber(string call)
        {
            return (uint)(logBook.Count + 1);
        }

        [ComVisible(true)]
        public string GridSquareSendingOverride()
        {            return "";        } // let DigiRite handle grid square setting

        [ComVisible(true)]
        public void LogFieldDayQso(string category, string section)
        {
            var dict = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(category))
                dict["CLASS"] = category;
            if (!String.IsNullOrEmpty(section))
                dict["ARRL_SECT"] = section;
            logBook.AddToLog(items, dict, rig.getFrequency());
            items = null;
        }

        [ComVisible(true)]
        public void LogGridSquareQso(string sentRst, string receivedGrid, string receivedDbReport)
        {
            var dict = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(sentRst))
                dict["RST_SENT"] = sentRst;
            if (!String.IsNullOrEmpty(receivedGrid))
                dict["GRIDSQUARE"] = receivedGrid;
            if (!String.IsNullOrEmpty(receivedDbReport))
                dict["RST_RCVD"] = receivedDbReport;
            logBook.AddToLog(items, dict, rig.getFrequency());
            items = null;
        }

        [ComVisible(true)]
        public void LogRoundUpQso(string sentRst, string receivedRst, string stateOrSerial)
        {
            var dict = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(sentRst))
                dict["RST_SENT"] = sentRst;
            int v;
            if (Int32.TryParse(stateOrSerial, out v))
                dict["SRX"] = stateOrSerial;
            else if (!String.IsNullOrEmpty(stateOrSerial))
                dict["STATE"] = stateOrSerial;
            if (!String.IsNullOrEmpty(receivedRst))
                dict["RST_RCVD"] = receivedRst;
            logBook.AddToLog(items, dict, rig.getFrequency());
            items = null;
        }

        [ComVisible(true)]
        public void SetCurrentCallAndGridAndSerial(string call, string grid, uint serialNumber)
        {   // show this call/ grid/ serial as QSO in progress
        }

        [ComVisible(true)]
        public void SetPtt(bool ptt)
        {
            if (null != rig)
                rig.PTT = ptt;
        }     

        ItemsToLog items;

        [ComVisible(true)]
        public void SetQsoItemsToLog(string call, uint SentSerialNumber, string AdifDate, string AdifTime, string sentGrid, string digitalMode)
        {
            items = new ItemsToLog();
            items.mycall = CallUsed;
            items.hiscall = call; items.sentSerialNumber = SentSerialNumber; items.adifDate = AdifDate;
            items.adifTime = AdifTime;
            items.sentGrid = String.IsNullOrEmpty(sentGrid) ? GridUsed : sentGrid;
            items.digitalMode = digitalMode;
        }

        [ComVisible(true)]
        public void SetRigFrequency(double rxKHz, double txKHz, bool split)
        {
            if (null != rig)
            {
                if (split)
                    rig.setSplit(rxKHz, txKHz);
                else
                    rig.setTransceive(rxKHz);
             }
        }

        [ComVisible(true)]
        public void SetTransmitFocus()
        {   // nothing to do here.
        }
    }
}
