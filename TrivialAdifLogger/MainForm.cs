using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdifLog
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region state
        private LogBook logBook = new LogBook();
        private object digirite;
        private Type digiriteType;
        private HamlibThreadWrapper rig;
        private DigiRiteCallbacks callbacks;
        private bool prevPollCompleted;

        private string fileSavePath;
        private bool isDirty = false;
        private bool isReading = false;
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            labelFreq.Text = "";
            logBook.entryAddedDel += OnEntryAdded;

            var rigType = Properties.Settings.Default.HamlibRigType;
            if (rigType > 0)
            {
                if (initHamLib(rigType,
                    Properties.Settings.Default.RigCommPort,
                    Properties.Settings.Default.RigBaudRate))
                    initDigiRite();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (null != rig)
                rig.Dispose();
            rig = null;
            if (null != digirite)
            try
            {   // it might already be closed...
                digiriteType.InvokeMember("CloseWindow", System.Reflection.BindingFlags.InvokeMethod, null, digirite, new object[0]);
            }
            catch (System.Exception)
            { }
            Properties.Settings.Default.Save();
        }

        string BackupFileName
        {
            get
            {   // add -bck to last saved file name
                string toSaveFile = fileSavePath;
                int dotIdx = toSaveFile.LastIndexOf('.');
                if (dotIdx >= 0)
                    toSaveFile = toSaveFile.Substring(0, dotIdx) + "-bck" + toSaveFile.Substring(dotIdx);
                else
                    toSaveFile += "-bck";
                return toSaveFile;
            }
        }

        private void OnEntryAdded(LogBook.LogEntry le)
        {
            dataGridView1.Rows.Add(new GridRowItem(le).getRowBinding(dataGridView1.ColumnCount));
            dataGridView1.FirstDisplayedScrollingRowIndex =logBook.Count-1;
            isDirty = true;
            if (!isReading && !String.IsNullOrEmpty(fileSavePath))
                using (var writer = new System.IO.StreamWriter(BackupFileName, true))
                {   // append QSO to -bck file between File/Save operations
                    Adif.saveQso(writer, le);
                }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        { Close();  }
        
        private bool initHamLib(int modelNumber, string comPort, uint baud)
        {
            if (null != rig)
            {
                rig.Dispose();
                rig = null;
                timer1.Enabled = false;
            }
            rig = new HamlibThreadWrapper(modelNumber);
            if (!rig.open(comPort, baud))
            {
                rig.Dispose();
                rig = null;
                MessageBox.Show("Rig open failed", "Trivial ADIF Logger");
            }
            else
            {
                timer1.Enabled = true;
                if (null != callbacks)
                    callbacks.rig = rig;
                return true;
            }
            return false;
        }
        
        private void initDigiRite()
        {
            if (null != digirite)
            {   // check if our digirite object is still alive
                try
                {
                    digiriteType.InvokeMember("GetCurrentMode", System.Reflection.BindingFlags.InvokeMethod, null, digirite, null);
                }
                catch (System.Exception)
                { digirite = null; } /*  if cannot invoke "GetCurrentMode" then it must be gone.*/
            }
            if (null == digirite)
            {   // goop to get DigiRite started and plumbed to us
                digiriteType = Type.GetTypeFromProgID("DigiRite.Ft8Auto");
                digirite = Activator.CreateInstance(digiriteType);
                digiriteType.InvokeMember("SetLoggerAssemblyName", System.Reflection.BindingFlags.InvokeMethod,
                    null, digirite, new object[] { "DigiRiteComLogger" });
                if (null == callbacks)
                    callbacks = new DigiRiteCallbacks(this, logBook, rig);
                digiriteType.InvokeMember("SetWlEntry", System.Reflection.BindingFlags.InvokeMethod, null, digirite,
                    new object[] { callbacks, 1 });
                digiriteType.InvokeMember("Show", System.Reflection.BindingFlags.InvokeMethod, null, digirite, new object[0]);
            }
        }

        private void rigSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new PortSelect();
            if ((f.ModelNumber = Properties.Settings.Default.HamlibRigType) > 0)
            {
                f.ComPort = Properties.Settings.Default.RigCommPort;
                f.Baud = Properties.Settings.Default.RigBaudRate;
            }
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                if (initHamLib(f.ModelNumber, f.ComPort, f.Baud))
                {
                    Properties.Settings.Default.HamlibRigType = f.ModelNumber;
                    Properties.Settings.Default.RigBaudRate = f.Baud;
                    Properties.Settings.Default.RigCommPort = f.ComPort;
                    initDigiRite();
                }
            }
        }


        private void onPollComplete(double rxKhz, double txKhz, HamLibClr.Mode_t mode, bool split)
        {   // from hamlib thread
            if (!IsDisposed)
                BeginInvoke(new Action(() =>
                {
                    string m = "Phone";
                    switch (mode)
                    {
                        case HamLibClr.Mode_t.MODE_CW:
                            m = "CW";
                            break;
                        case HamLibClr.Mode_t.MODE_DIG:
                            m = "DIG";
                            break;
                    }
                    labelFreq.Text = String.Format("{0:0.00} KHz {1}", rxKhz, m);
                    prevPollCompleted = true;
                }));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (null != rig && prevPollCompleted)
                rig.poll(new HamlibThreadWrapper.Pollcallback(onPollComplete));
            else
                prevPollCompleted = true; // turn timer back on once per tick
        }


        private string FileSavePath
        {
            get {return fileSavePath;  }
            set
            {
                fileSavePath = value;
                this.Text = "Trivial ADIF Logger - " + fileSavePath;
            }
        }

        private void fileSave()
        {
            var adif = new Adif(FileSavePath, logBook);
            if (adif.fileSave())
            {
                if (System.IO.File.Exists(BackupFileName))
                    System.IO.File.Delete(BackupFileName);
                isDirty = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(FileSavePath))
                saveAsToolStripMenuItem_Click(sender, e);
            else
                fileSave();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "adif files (*.adif)|*.adif|adif files (*.adi)|*.adi";
            sfd.FilterIndex = 2;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileSavePath = sfd.FileName;
                fileSave();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isDirty || MessageBox.Show("Overwrite existing log?") == DialogResult.OK)
            {
                Clear();
                importToolStripMenuItem_Click(sender, e);
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "adif files (*.adif)|*.adif|adif files (*.adi)|*.adi";
            ofd.FilterIndex = 2;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileSavePath = ofd.FileName;
                var adif = new Adif(FileSavePath, logBook);
                isReading = true;
                adif.fileOpen();
                if (System.IO.File.Exists(BackupFileName))
                {
                    if (MessageBox.Show("Unsaved backup file found. Restore those QSOs now?", "Trivial Adif Logger", 
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        adif = new Adif(BackupFileName, logBook);
                        adif.fileOpen();
                    }
                }
                isReading = false;
                isDirty = false;
            }
        }

        private void Clear()
        {
            logBook.Reset();
            dataGridView1.Rows.Clear();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isDirty || MessageBox.Show("Remove all QSOs from the log?") == DialogResult.OK)
                Clear();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && isDirty)
            {
                if (MessageBox.Show("Do you really want to exit without saving the log?", "Trivial ADIF Logger", MessageBoxButtons.YesNo) !=
                    DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void getStartedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Use the File Menu 'Setup Rig' to choose your rig type and COM port and, if needed,"+
                " the baud rate of your rig. Successfully configuring a rig brings up DigiRite.\r\n\r\n" +
                "File/Open to resume an old log, if needed. File/Save to save it to ADIF. Use the ADIF tool of your choice to edit the log.");
        }
    }
}
