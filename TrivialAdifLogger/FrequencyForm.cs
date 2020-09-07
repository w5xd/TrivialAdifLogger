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
    public partial class FrequencyForm : Form
    {
        public FrequencyForm()
        {
            InitializeComponent();
        }

        public double Khz { get; set; } = 0;
        public HamLibClr.Mode_t mode { get; set; } = HamLibClr.Mode_t.MODE_DIG;
        private void FrequencyForm_Load(object sender, EventArgs e)
        {
            numericUpDownKhz.Value = (decimal)Khz;
            comboBoxMode.SelectedIndex = (int)mode - 1;
            numericUpDownKhz.Select(0, numericUpDownKhz.ToString().Length);
            numericUpDownKhz.Focus();
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            Khz = (double)numericUpDownKhz.Value;
            mode = (HamLibClr.Mode_t)(1 + comboBoxMode.SelectedIndex);
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
