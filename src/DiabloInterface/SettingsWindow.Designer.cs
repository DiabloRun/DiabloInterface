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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtAutoSplitHotkey = new System.Windows.Forms.TextBox();
            this.lblAutoSplitHotkey = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.btnFont.Location = new System.Drawing.Point(189, 19);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(76, 23);
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
            this.txtTitleFontSize.Size = new System.Drawing.Size(75, 20);
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
            this.groupBox1.Location = new System.Drawing.Point(387, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 122);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Font";
            // 
            // txtFontSize
            // 
            this.txtFontSize.Location = new System.Drawing.Point(91, 52);
            this.txtFontSize.Name = "txtFontSize";
            this.txtFontSize.Size = new System.Drawing.Size(75, 20);
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
            this.groupBox2.Location = new System.Drawing.Point(387, 140);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(271, 45);
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
            this.groupBox3.Size = new System.Drawing.Size(369, 275);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Auto-Split";
            // 
            // chkAutosplit
            // 
            this.chkAutosplit.AutoSize = true;
            this.chkAutosplit.Location = new System.Drawing.Point(6, 252);
            this.chkAutosplit.Name = "chkAutosplit";
            this.chkAutosplit.Size = new System.Drawing.Size(68, 17);
            this.chkAutosplit.TabIndex = 1;
            this.chkAutosplit.Text = "AutoSplit";
            this.chkAutosplit.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(357, 227);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(170, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Value";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 0);
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
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 328);
            this.Controls.Add(this.lblAutoSplitHotkey);
            this.Controls.Add(this.txtAutoSplitHotkey);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Name = "SettingsWindow";
            this.Text = "SettingsWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindow_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
    }
}