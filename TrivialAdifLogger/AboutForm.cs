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
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            richTextBox.Text = "Copyright (c) 2020 by WriteLog Contesting Software, LLC. "+
                "Published with source code and under the MIT license. See https://github.com/w5xd/TrivialAdifLogger\r\n\r\n" +
                "Rig control thanks to hamlib, licensed under the LGPL. See https://github.com/Hamlib\r\n" +
                "";
        }

        private void richTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.LinkText))
                System.Diagnostics.Process.Start(e.LinkText);
        }
    }
}
