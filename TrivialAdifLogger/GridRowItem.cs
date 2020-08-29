using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdifLog
{
    // helper class for displaying a QSO in a .NET DataGridView control
    class GridRowItem
    {     
        public GridRowItem(LogBook.LogEntry le)
        {  entry = le; }

        public object[] getRowBinding(int numColumns)
        {
            var ret = new object[numColumns];
            int i = 0;
            ret[i++] = entry.items.adifTime.Substring(0, 4);
            ret[i++] = entry.items.hiscall;
            ret[i++] = String.Format("{0:0.00}", entry.rxKhz);
            string v;
            entry.fields.TryGetValue("GRIDSQUARE", out v);
            ret[i++] = v;
            entry.fields.TryGetValue("RST_RCVD", out v);
            ret[i++] = v;
            entry.fields.TryGetValue("RST_SENT", out v);
            ret[i++] = v;
            entry.fields.TryGetValue("STATE", out v);
            ret[i++] = v;
            entry.fields.TryGetValue("ARRL_SECT", out v);
            ret[i++] = v;
            return ret;
        }

        private LogBook.LogEntry entry;
    }
}
