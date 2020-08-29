namespace AdifLog
{
    partial class PortSelect
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
            this.comboBoxPorts = new System.Windows.Forms.ComboBox();
            this.comboBoxBaud = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.comboBoxRigSel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxPorts
            // 
            this.comboBoxPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPorts.FormattingEnabled = true;
            this.comboBoxPorts.Location = new System.Drawing.Point(70, 52);
            this.comboBoxPorts.Name = "comboBoxPorts";
            this.comboBoxPorts.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPorts.Sorted = true;
            this.comboBoxPorts.TabIndex = 3;
            this.comboBoxPorts.SelectedIndexChanged += new System.EventHandler(this.check_SelectedIndexChanged);
            // 
            // comboBoxBaud
            // 
            this.comboBoxBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaud.FormattingEnabled = true;
            this.comboBoxBaud.Location = new System.Drawing.Point(197, 52);
            this.comboBoxBaud.Name = "comboBoxBaud";
            this.comboBoxBaud.Size = new System.Drawing.Size(106, 21);
            this.comboBoxBaud.TabIndex = 4;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new System.Drawing.Point(228, 88);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // comboBoxRigSel
            // 
            this.comboBoxRigSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRigSel.FormattingEnabled = true;
            this.comboBoxRigSel.Location = new System.Drawing.Point(70, 13);
            this.comboBoxRigSel.Name = "comboBoxRigSel";
            this.comboBoxRigSel.Size = new System.Drawing.Size(233, 21);
            this.comboBoxRigSel.Sorted = true;
            this.comboBoxRigSel.TabIndex = 1;
            this.comboBoxRigSel.SelectedIndexChanged += new System.EventHandler(this.check_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rig type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port:";
            // 
            // PortSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 123);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxRigSel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxBaud);
            this.Controls.Add(this.comboBoxPorts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PortSelect";
            this.Text = "Rig Setup";
            this.Load += new System.EventHandler(this.PortSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxPorts;
        private System.Windows.Forms.ComboBox comboBoxBaud;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ComboBox comboBoxRigSel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}