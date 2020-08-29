namespace AdifLog
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fIleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rigSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelFreq = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CALL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rxKHz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HisGrid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MyRST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HisRST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Section = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.getStartedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fIleToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(854, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fIleToolStripMenuItem
            // 
            this.fIleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rigSetupToolStripMenuItem,
            this.openToolStripMenuItem,
            this.importToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.clearToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fIleToolStripMenuItem.Name = "fIleToolStripMenuItem";
            this.fIleToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fIleToolStripMenuItem.Text = "File";
            // 
            // rigSetupToolStripMenuItem
            // 
            this.rigSetupToolStripMenuItem.Name = "rigSetupToolStripMenuItem";
            this.rigSetupToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.rigSetupToolStripMenuItem.Text = "Rig Setup...";
            this.rigSetupToolStripMenuItem.Click += new System.EventHandler(this.rigSetupToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.importToolStripMenuItem.Text = "Import and Append...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getStartedToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // labelFreq
            // 
            this.labelFreq.AutoSize = true;
            this.labelFreq.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelFreq.Location = new System.Drawing.Point(0, 340);
            this.labelFreq.Name = "labelFreq";
            this.labelFreq.Size = new System.Drawing.Size(35, 13);
            this.labelFreq.TabIndex = 1;
            this.labelFreq.Text = "label1";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.CALL,
            this.rxKHz,
            this.HisGrid,
            this.MyRST,
            this.HisRST,
            this.State,
            this.Section});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(854, 316);
            this.dataGridView1.TabIndex = 2;
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // CALL
            // 
            this.CALL.HeaderText = "CALL";
            this.CALL.Name = "CALL";
            this.CALL.ReadOnly = true;
            // 
            // rxKHz
            // 
            this.rxKHz.HeaderText = "KHz";
            this.rxKHz.Name = "rxKHz";
            this.rxKHz.ReadOnly = true;
            // 
            // HisGrid
            // 
            this.HisGrid.HeaderText = "GRID";
            this.HisGrid.Name = "HisGrid";
            this.HisGrid.ReadOnly = true;
            // 
            // MyRST
            // 
            this.MyRST.HeaderText = "RST";
            this.MyRST.Name = "MyRST";
            this.MyRST.ReadOnly = true;
            this.MyRST.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // HisRST
            // 
            this.HisRST.HeaderText = "SNT";
            this.HisRST.Name = "HisRST";
            this.HisRST.ReadOnly = true;
            this.HisRST.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // State
            // 
            this.State.HeaderText = "State/Prov";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            // 
            // Section
            // 
            this.Section.HeaderText = "Section";
            this.Section.Name = "Section";
            this.Section.ReadOnly = true;
            // 
            // getStartedToolStripMenuItem
            // 
            this.getStartedToolStripMenuItem.Name = "getStartedToolStripMenuItem";
            this.getStartedToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.getStartedToolStripMenuItem.Text = "Get started...";
            this.getStartedToolStripMenuItem.Click += new System.EventHandler(this.getStartedToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 353);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.labelFreq);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Simple Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fIleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rigSetupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label labelFreq;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn CALL;
        private System.Windows.Forms.DataGridViewTextBoxColumn rxKHz;
        private System.Windows.Forms.DataGridViewTextBoxColumn HisGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn MyRST;
        private System.Windows.Forms.DataGridViewTextBoxColumn HisRST;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn Section;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getStartedToolStripMenuItem;
    }
}

