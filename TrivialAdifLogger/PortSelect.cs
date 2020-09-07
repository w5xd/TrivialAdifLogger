using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdifLog
{
    public partial class PortSelect : Form
    {
        // the hamlib rig list is presented in a combo box with this
        class RigTypeEntry
        {
            public RigTypeEntry(int modelNumber, string mfg, string modelName)
            {
                this.modelNumber = modelNumber;
                this.modelName = modelName;
                this.mfg = mfg;
            }
            public int modelNumber;
            public string mfg;
            public string modelName;
            public override string ToString()
            {
                return mfg + " " + modelName;
            }
        };
        
        public PortSelect()
        {
            InitializeComponent();
        }

        private object NullItem = "Default";

        private void PortSelect_Load(object sender, EventArgs e)
        {
            object sel = null;
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxPorts.Items.Add(s);
                if (ComPort?.ToUpper() == s.ToUpper())
                    sel = s;
            }
            comboBoxPorts.SelectedItem = sel;

            sel = NullItem;
            comboBoxBaud.Items.Add(NullItem);
            for (uint i = 2400; i < 300000; i *= 2)
            {
                object v = i.ToString();
                if (i == Baud)
                    sel = v;
                comboBoxBaud.Items.Add(v);
            }
            comboBoxBaud.SelectedItem = sel;
            sel = null;
            HamlibThreadWrapper.listRigs((int model, string mfg, string modelname) =>
            {
                object v = new RigTypeEntry(model, mfg, modelname);
                if (model == ModelNumber)
                    sel = v;
                else if (null == sel && model == DUMMY_HAMLIB_RIG)
                    sel = v;
                comboBoxRigSel.Items.Add(v);
            });
            comboBoxRigSel.SelectedItem = sel;
        }

        public uint Baud { get; set; } = 0;
        public string ComPort { get; set; } = "";
        public int ModelNumber { get; set; } = -1;

        private void buttonOK_Click(object sender, EventArgs e)
        {
            object baud = comboBoxBaud.SelectedItem;
            if (null != baud && NullItem != baud)
                Baud = UInt32.Parse(baud.ToString());
            ComPort = comboBoxPorts.SelectedItem?.ToString();
            RigTypeEntry rs = comboBoxRigSel.SelectedItem as RigTypeEntry;
            if (null != rs)
                ModelNumber = rs.modelNumber;            
        }

        const int DUMMY_HAMLIB_RIG = 1;

        private void check_SelectedIndexChanged(object sender, EventArgs e)
        {
            RigTypeEntry re = comboBoxRigSel.SelectedItem as RigTypeEntry;
            bool isDummy = re?.modelNumber == DUMMY_HAMLIB_RIG;
            buttonOK.Enabled = (null != re) && (isDummy || (null != comboBoxPorts.SelectedItem));
            if (isDummy)
                comboBoxPorts.SelectedItem = null;
        }
    }
}
