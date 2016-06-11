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
            this.btnTestHotkey = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblAutoSplitHotkey = new System.Windows.Forms.Label();
            this.txtAutoSplitHotkey = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkShowDebug = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbVersion = new System.Windows.Forms.ComboBox();
            this.grpRunes = new System.Windows.Forms.GroupBox();
            this.runeDisplayPanel = new System.Windows.Forms.Panel();
            this.btnAddRune = new System.Windows.Forms.Button();
            this.comboBoxRunes = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.grpRunes.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(597, 293);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(61, 23);
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
            this.btnApply.Location = new System.Drawing.Point(525, 293);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(58, 23);
            this.btnApply.TabIndex = 6;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(444, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
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
            this.groupBox3.Controls.Add(this.btnTestHotkey);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.lblAutoSplitHotkey);
            this.groupBox3.Controls.Add(this.txtAutoSplitHotkey);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(426, 277);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Auto-Split";
            // 
            // chkAutosplit
            // 
            this.chkAutosplit.AutoSize = true;
            this.chkAutosplit.Location = new System.Drawing.Point(296, 252);
            this.chkAutosplit.Name = "chkAutosplit";
            this.chkAutosplit.Size = new System.Drawing.Size(59, 17);
            this.chkAutosplit.TabIndex = 1;
            this.chkAutosplit.Text = "Enable";
            this.chkAutosplit.UseVisualStyleBackColor = true;
            this.chkAutosplit.CheckedChanged += new System.EventHandler(this.chkAutosplit_CheckedChanged);
            // 
            // btnTestHotkey
            // 
            this.btnTestHotkey.Location = new System.Drawing.Point(167, 248);
            this.btnTestHotkey.Name = "btnTestHotkey";
            this.btnTestHotkey.Size = new System.Drawing.Size(75, 23);
            this.btnTestHotkey.TabIndex = 18;
            this.btnTestHotkey.Text = "Test Hotkey";
            this.btnTestHotkey.UseVisualStyleBackColor = true;
            this.btnTestHotkey.Click += new System.EventHandler(this.btnTestHotkey_Click);
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
            this.panel1.Size = new System.Drawing.Size(414, 225);
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
            this.button1.Location = new System.Drawing.Point(361, 248);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Add Split";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblAutoSplitHotkey
            // 
            this.lblAutoSplitHotkey.AutoSize = true;
            this.lblAutoSplitHotkey.Location = new System.Drawing.Point(6, 253);
            this.lblAutoSplitHotkey.Name = "lblAutoSplitHotkey";
            this.lblAutoSplitHotkey.Size = new System.Drawing.Size(67, 13);
            this.lblAutoSplitHotkey.TabIndex = 13;
            this.lblAutoSplitHotkey.Text = "Split-Hotkey:";
            // 
            // txtAutoSplitHotkey
            // 
            this.txtAutoSplitHotkey.Location = new System.Drawing.Point(79, 250);
            this.txtAutoSplitHotkey.Name = "txtAutoSplitHotkey";
            this.txtAutoSplitHotkey.Size = new System.Drawing.Size(82, 20);
            this.txtAutoSplitHotkey.TabIndex = 12;
            this.txtAutoSplitHotkey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAutoSplitHotkey_KeyDown);
            this.txtAutoSplitHotkey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAutoSplitHotkey_KeyPress);
            this.txtAutoSplitHotkey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtAutoSplitHotkey_KeyUp);
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
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Version:";
            // 
            // cmbVersion
            // 
            this.cmbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVersion.FormattingEnabled = true;
            this.cmbVersion.Items.AddRange(new object[] {
            "1.14d",
            "1.14c"});
            this.cmbVersion.Location = new System.Drawing.Point(90, 17);
            this.cmbVersion.Name = "cmbVersion";
            this.cmbVersion.Size = new System.Drawing.Size(53, 21);
            this.cmbVersion.TabIndex = 0;
            // 
            // grpRunes
            // 
            this.grpRunes.Controls.Add(this.runeDisplayPanel);
            this.grpRunes.Controls.Add(this.btnAddRune);
            this.grpRunes.Controls.Add(this.comboBoxRunes);
            this.grpRunes.Location = new System.Drawing.Point(12, 295);
            this.grpRunes.Name = "grpRunes";
            this.grpRunes.Size = new System.Drawing.Size(426, 111);
            this.grpRunes.TabIndex = 19;
            this.grpRunes.TabStop = false;
            this.grpRunes.Text = "Rune Display";
            // 
            // runeDisplayPanel
            // 
            this.runeDisplayPanel.AutoScroll = true;
            this.runeDisplayPanel.Location = new System.Drawing.Point(6, 19);
            this.runeDisplayPanel.Name = "runeDisplayPanel";
            this.runeDisplayPanel.Size = new System.Drawing.Size(414, 57);
            this.runeDisplayPanel.TabIndex = 2;
            // 
            // btnAddRune
            // 
            this.btnAddRune.Location = new System.Drawing.Point(345, 82);
            this.btnAddRune.Name = "btnAddRune";
            this.btnAddRune.Size = new System.Drawing.Size(75, 23);
            this.btnAddRune.TabIndex = 1;
            this.btnAddRune.Text = "Add Rune";
            this.btnAddRune.UseVisualStyleBackColor = true;
            this.btnAddRune.Click += new System.EventHandler(this.btnAddRune_Click);
            // 
            // comboBoxRunes
            // 
            this.comboBoxRunes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRunes.FormattingEnabled = true;
            this.comboBoxRunes.Items.AddRange(new object[] {
            "El",
            "Eld",
            "Tir",
            "Nef",
            "Eth",
            "Ith",
            "Tal",
            "Ral",
            "Ort",
            "Thul",
            "Amn",
            "Sol",
            "Shael",
            "Dol",
            "Hel",
            "Io",
            "Lum",
            "Ko",
            "Fal",
            "Lem",
            "Pul",
            "Um",
            "Mal",
            "Ist",
            "Gul",
            "Vex",
            "Ohm",
            "Lo",
            "Sur",
            "Ber",
            "Jah",
            "Cham",
            "Zod"});
            this.comboBoxRunes.Location = new System.Drawing.Point(218, 83);
            this.comboBoxRunes.Name = "comboBoxRunes";
            this.comboBoxRunes.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRunes.TabIndex = 0;
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 416);
            this.Controls.Add(this.grpRunes);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
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
            this.grpRunes.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkShowDebug;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbVersion;
        private System.Windows.Forms.Button btnTestHotkey;
        private System.Windows.Forms.GroupBox grpRunes;
        private System.Windows.Forms.Button btnAddRune;
        private System.Windows.Forms.ComboBox comboBoxRunes;
        private System.Windows.Forms.Panel runeDisplayPanel;
    }
}