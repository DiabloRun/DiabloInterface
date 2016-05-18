namespace DiabloInterface
{
    partial class SettingsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnFont = new System.Windows.Forms.Button();
            this.lblSelectedFont = new System.Windows.Forms.Label();
            this.lblFontExample = new System.Windows.Forms.Label();
            this.txtTitleFontSize = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtFontSize = new System.Windows.Forms.TextBox();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.lblTitleFontSize = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkCreateFiles = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkAutosplit = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtAutoSplitHotkey = new System.Windows.Forms.TextBox();
            this.lblAutoSplitHotkey = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkShowDebug = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cmbVersion = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(583, 293);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnFont
            // 
            this.btnFont.Location = new System.Drawing.Point(153, 19);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(55, 23);
            this.btnFont.TabIndex = 1;
            this.btnFont.Text = "Change";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click_1);
            // 
            // lblSelectedFont
            // 
            this.lblSelectedFont.AutoSize = true;
            this.lblSelectedFont.Location = new System.Drawing.Point(6, 24);
            this.lblSelectedFont.Name = "lblSelectedFont";
            this.lblSelectedFont.Size = new System.Drawing.Size(76, 13);
            this.lblSelectedFont.TabIndex = 2;
            this.lblSelectedFont.Text = "Selected Font:";
            // 
            // lblFontExample
            // 
            this.lblFontExample.AutoSize = true;
            this.lblFontExample.Location = new System.Drawing.Point(88, 24);
            this.lblFontExample.Name = "lblFontExample";
            this.lblFontExample.Size = new System.Drawing.Size(10, 13);
            this.lblFontExample.TabIndex = 3;
            this.lblFontExample.Text = "-";
            // 
            // txtTitleFontSize
            // 
            this.txtTitleFontSize.Location = new System.Drawing.Point(91, 86);
            this.txtTitleFontSize.Name = "txtTitleFontSize";
            this.txtTitleFontSize.Size = new System.Drawing.Size(49, 20);
            this.txtTitleFontSize.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtFontSize);
            this.groupBox1.Controls.Add(this.lblFontSize);
            this.groupBox1.Controls.Add(this.lblTitleFontSize);
            this.groupBox1.Controls.Add(this.btnFont);
            this.groupBox1.Controls.Add(this.txtTitleFontSize);
            this.groupBox1.Controls.Add(this.lblSelectedFont);
            this.groupBox1.Controls.Add(this.lblFontExample);
            this.groupBox1.Location = new System.Drawing.Point(444, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 122);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Font";
            // 
            // txtFontSize
            // 
            this.txtFontSize.Location = new System.Drawing.Point(91, 52);
            this.txtFontSize.Name = "txtFontSize";
            this.txtFontSize.Size = new System.Drawing.Size(49, 20);
            this.txtFontSize.TabIndex = 8;
            // 
            // lblFontSize
            // 
            this.lblFontSize.AutoSize = true;
            this.lblFontSize.Location = new System.Drawing.Point(6, 55);
            this.lblFontSize.Name = "lblFontSize";
            this.lblFontSize.Size = new System.Drawing.Size(54, 13);
            this.lblFontSize.TabIndex = 7;
            this.lblFontSize.Text = "Font Size:";
            // 
            // lblTitleFontSize
            // 
            this.lblTitleFontSize.AutoSize = true;
            this.lblTitleFontSize.Location = new System.Drawing.Point(6, 89);
            this.lblTitleFontSize.Name = "lblTitleFontSize";
            this.lblTitleFontSize.Size = new System.Drawing.Size(77, 13);
            this.lblTitleFontSize.TabIndex = 6;
            this.lblTitleFontSize.Text = "Title Font Size:";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(502, 293);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 6;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(421, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkCreateFiles
            // 
            this.chkCreateFiles.AutoSize = true;
            this.chkCreateFiles.Location = new System.Drawing.Point(10, 19);
            this.chkCreateFiles.Name = "chkCreateFiles";
            this.chkCreateFiles.Size = new System.Drawing.Size(81, 17);
            this.chkCreateFiles.TabIndex = 8;
            this.chkCreateFiles.Text = "Create Files";
            this.chkCreateFiles.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkCreateFiles);
            this.groupBox2.Location = new System.Drawing.Point(444, 140);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(214, 45);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkAutosplit);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(426, 275);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Auto-Split";
            // 
            // chkAutosplit
            // 
            this.chkAutosplit.AutoSize = true;
            this.chkAutosplit.Location = new System.Drawing.Point(6, 252);
            this.chkAutosplit.Name = "chkAutosplit";
            this.chkAutosplit.Size = new System.Drawing.Size(107, 17);
            this.chkAutosplit.TabIndex = 1;
            this.chkAutosplit.Text = "Enable Auto-Split";
            this.chkAutosplit.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(414, 227);
            this.panel1.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(301, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Difficulty";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(154, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Value";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 293);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Add Auto-Split";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtAutoSplitHotkey
            // 
            this.txtAutoSplitHotkey.Location = new System.Drawing.Point(249, 295);
            this.txtAutoSplitHotkey.Name = "txtAutoSplitHotkey";
            this.txtAutoSplitHotkey.Size = new System.Drawing.Size(100, 20);
            this.txtAutoSplitHotkey.TabIndex = 12;
            this.txtAutoSplitHotkey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAutoSplitHotkey_KeyDown);
            this.txtAutoSplitHotkey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAutoSplitHotkey_KeyPress);
            this.txtAutoSplitHotkey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtAutoSplitHotkey_KeyUp);
            // 
            // lblAutoSplitHotkey
            // 
            this.lblAutoSplitHotkey.AutoSize = true;
            this.lblAutoSplitHotkey.Location = new System.Drawing.Point(154, 298);
            this.lblAutoSplitHotkey.Name = "lblAutoSplitHotkey";
            this.lblAutoSplitHotkey.Size = new System.Drawing.Size(92, 13);
            this.lblAutoSplitHotkey.TabIndex = 13;
            this.lblAutoSplitHotkey.Text = "Auto-Split Hotkey:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.richTextBox1.Location = new System.Drawing.Point(110, 396);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(386, 35);
            this.richTextBox1.TabIndex = 14;
            this.richTextBox1.Text = "These keys are triggered when a split is reached and Auto-Split is enabled. Enter" +
    " the same Hotkey that your split tool (e.g. Livesplit) listens to for Start/Spli" +
    "t.";
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.richTextBox2.Location = new System.Drawing.Point(110, 362);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(414, 28);
            this.richTextBox2.TabIndex = 14;
            this.richTextBox2.Text = "Add a new split that should be triggered. After setting up all your splits, they " +
    "should match the splits in your split tool (e.g. Livesplit).";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(12, 338);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Enable Auto-Split:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.Location = new System.Drawing.Point(12, 362);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Add Auto-Split:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label6.Location = new System.Drawing.Point(12, 396);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Auto-Split Hotkey:";
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.richTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.richTextBox3.Location = new System.Drawing.Point(110, 338);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(414, 18);
            this.richTextBox3.TabIndex = 14;
            this.richTextBox3.Text = "This option enables automatic splitting.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label7.Location = new System.Drawing.Point(12, 437);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Create Files:";
            // 
            // richTextBox4
            // 
            this.richTextBox4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.richTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.richTextBox4.Location = new System.Drawing.Point(110, 437);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.ReadOnly = true;
            this.richTextBox4.Size = new System.Drawing.Size(399, 35);
            this.richTextBox4.TabIndex = 14;
            this.richTextBox4.Text = "Enabling this causes the tool to create txt files in exe-folder/txt/... that can " +
    "be read by your streaming software.";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkShowDebug);
            this.groupBox4.Location = new System.Drawing.Point(444, 191);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(214, 45);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Debug";
            // 
            // chkShowDebug
            // 
            this.chkShowDebug.AutoSize = true;
            this.chkShowDebug.Location = new System.Drawing.Point(10, 20);
            this.chkShowDebug.Name = "chkShowDebug";
            this.chkShowDebug.Size = new System.Drawing.Size(130, 17);
            this.chkShowDebug.TabIndex = 0;
            this.chkShowDebug.Text = "Show Debug Window";
            this.chkShowDebug.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.cmbVersion);
            this.groupBox5.Location = new System.Drawing.Point(445, 243);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(213, 44);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Diablo 2";
            // 
            // cmbVersion
            // 
            this.cmbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVersion.FormattingEnabled = true;
            this.cmbVersion.Items.AddRange(new object[] {
            "1.14c",
            "1.14b"});
            this.cmbVersion.Location = new System.Drawing.Point(90, 17);
            this.cmbVersion.Name = "cmbVersion";
            this.cmbVersion.Size = new System.Drawing.Size(53, 21);
            this.cmbVersion.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Version:";
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 474);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox4);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.lblAutoSplitHotkey);
            this.Controls.Add(this.txtAutoSplitHotkey);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsWindow";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindow_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.Label lblSelectedFont;
        private System.Windows.Forms.Label lblFontExample;
        private System.Windows.Forms.TextBox txtTitleFontSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTitleFontSize;
        private System.Windows.Forms.TextBox txtFontSize;
        private System.Windows.Forms.Label lblFontSize;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkCreateFiles;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkAutosplit;
        private System.Windows.Forms.TextBox txtAutoSplitHotkey;
        private System.Windows.Forms.Label lblAutoSplitHotkey;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkShowDebug;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbVersion;
    }
}