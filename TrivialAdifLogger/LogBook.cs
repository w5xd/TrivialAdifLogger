using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdifLog
{
    // trival logbook.
    // Each QSO is a LogEntry.
    public class LogBook : IEnumerable<LogBook.LogEntry>
    {
        static double[] bandEdges = {0,
                1800,
                3500,
                5000,
                7000,
                10100,
                14000,
                18068,
                21000,
                24890,
                28000,
                50000,
                144000,
                219000,
                420000,
                902000,
                1240000,
                2300000,
                3300000,
                5650000,
               10000000,
               24000000,
               47000000,
               76000000
                };

        public LogBook()
        {}

        public void Reset()
        {   // back to zero QSOs in the logbook
            id = 0;
            logById.Clear();
            log.Clear();
            dupes.Clear();
        }

        public class LogEntry
        {
            public LogEntry(ItemsToLog items, Dictionary<string, string> fields, double rxKhz)
            {
                this.items = items;
                this.fields = fields;
                this.rxKhz = rxKhz;
            }
            public ItemsToLog items { get; private set; }
            public Dictionary<string, string> fields { get; private set; }
            public double rxKhz { get; private set; }
        }

        public delegate void EntryAddedDel(LogEntry e);

        public EntryAddedDel entryAddedDel { get; set;} = null;

        public void AddToLog(ItemsToLog myItems, Dictionary<string, string> fields, double rxKhz)
        {
            var n = new LogEntry(myItems, fields, rxKhz);
            logById[id] = n;
            var idx = bandIdx(rxKhz);
            Dictionary<string, List<int>> band;
            if (!dupes.TryGetValue(idx, out band))
            {
                band = new Dictionary<string, List<int>>();
                dupes[idx] = band;
            }
            List<int> onThisBand;
            if (!band.TryGetValue(myItems.hiscall, out onThisBand))
            {
                onThisBand = new List<int>();
                band[myItems.hiscall] = onThisBand;
            }
            onThisBand.Add(id);
            log.Add(id++);
            if (null != entryAddedDel)
                entryAddedDel(n);
        }

        public LogEntry getLogItem(int id)
        {
            LogEntry r;
            logById.TryGetValue(id, out r);
            return r;
        }

        public int getLogItemId(int which)
        {
            if (which < 0 || which >= log.Count)
                return -1;
            return log[which];
        }

        #region state
        // the double indirection is to help in the case of ever implementing removing a QSO.
        // log is a list of id's
        // dupes results in an id.
        // removal of an entry from log does not invalidate the dupes...except that its possible to
        // for a dupe check to result in a deleted QSO.
        private int 
            id = 0;
        private Dictionary<int, LogEntry> 
            logById = new Dictionary<int, LogEntry>();
        private List<int> 
            log = new List<int>();
        // dupes maps a "band" to set of "call"s
        private Dictionary<int, Dictionary<string, List<int>>> 
            dupes = new Dictionary<int, Dictionary<string, List<int>>>();
        #endregion

        public int Count { get { return log.Count; } }
        public void CheckDupe(string call,  double rxKhz, out bool dupe)
        {
            dupe = false;
            var idx = bandIdx(rxKhz);
            Dictionary<string, List<int>> band;
            if (dupes.TryGetValue(idx, out band))
            {   // we have a dupe list for this band
                List<int> ids;
                if (band.TryGetValue(call, out ids))
                    dupe = ids.Any(); // TODO if the id points to removed QSO...but don't remove any yet
            }
        }

        // CheckDupe separates call's onto "bands" per the bandEdges
        public static int bandIdx(double Khz)
        {   // returns the index into bandEdges of the highest entry less than or equal to Khz
            int min = 0;
            int max = bandEdges.Length - 1;
            for (;;)
            {
                if (Khz <= bandEdges[min])
                    return min;
                if (Khz > bandEdges[max])
                    return max;
                // binary search
                int next = (max + min) / 2;
                var test = bandEdges[next];
                if (Khz > test)
                    min = next;
                else if (Khz < test)
                    max = next;
                else
                    return next;
                if (max - min == 1)
                    return min;
            }
        }

        public IEnumerator<LogEntry> GetEnumerator()
        { return new LogbookEnumerator(this);}

        IEnumerator IEnumerable.GetEnumerator()
        {  return new LogbookEnumerator(this);   }

        class LogbookEnumerator : IEnumerator<LogEntry>
        {
            LogBook lb;
            int which = -1;

            public LogbookEnumerator(LogBook lb)
            {   this.lb = lb;    }
            public LogEntry Current
            { get { return lb.getLogItem(lb.getLogItemId(which));} }
            object IEnumerator.Current 
            { get { return lb.getLogItem(lb.getLogItemId(which));} }

            public bool MoveNext()
            {
                var c = lb.Count;
                if (which < c)
                    which += 1;
                return which < c;
            }

            public void Reset()
            {  which = 0; }

            #region IDisposable Support
            public void Dispose()
            { }
            #endregion
        }
    }

    public class ItemsToLog
    {
        public string mycall;
        public string hiscall;
        public uint sentSerialNumber;
        public string adifDate;
        public string adifTime;
        public string sentGrid;
        public string digitalMode;
    }
}
