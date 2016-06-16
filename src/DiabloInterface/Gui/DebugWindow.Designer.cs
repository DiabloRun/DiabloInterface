namespace DiabloInterface.Gui
{
    partial class DebugWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugWindow));
            this.autosplitPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabBoxItems = new System.Windows.Forms.TabControl();
            this.tabPageHead = new System.Windows.Forms.TabPage();
            this.tabPageAmulet = new System.Windows.Forms.TabPage();
            this.tabPageWeaponLeft = new System.Windows.Forms.TabPage();
            this.tabPageWeaponRight = new System.Windows.Forms.TabPage();
            this.tabPageBody = new System.Windows.Forms.TabPage();
            this.tabPageHand = new System.Windows.Forms.TabPage();
            this.tabPageFeet = new System.Windows.Forms.TabPage();
            this.tabPageRingLeft = new System.Windows.Forms.TabPage();
            this.tabPageRingRight = new System.Windows.Forms.TabPage();
            this.tabPageBelt = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabBoxItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // autosplitPanel
            // 
            this.autosplitPanel.AutoScroll = true;
            this.autosplitPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autosplitPanel.Location = new System.Drawing.Point(3, 16);
            this.autosplitPanel.Name = "autosplitPanel";
            this.autosplitPanel.Size = new System.Drawing.Size(192, 236);
            this.autosplitPanel.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Location = new System.Drawing.Point(15, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(644, 555);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Quest-Bits";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(638, 536);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(630, 510);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Normal";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(630, 510);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Nightmare";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(630, 510);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Hell";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.autosplitPanel);
            this.groupBox2.Location = new System.Drawing.Point(665, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(198, 255);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Splits";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tabBoxItems);
            this.groupBox3.Location = new System.Drawing.Point(668, 263);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(192, 296);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Last Item";
            // 
            // tabBoxItems
            // 
            this.tabBoxItems.Controls.Add(this.tabPageHead);
            this.tabBoxItems.Controls.Add(this.tabPageAmulet);
            this.tabBoxItems.Controls.Add(this.tabPageWeaponLeft);
            this.tabBoxItems.Controls.Add(this.tabPageWeaponRight);
            this.tabBoxItems.Controls.Add(this.tabPageBody);
            this.tabBoxItems.Controls.Add(this.tabPageHand);
            this.tabBoxItems.Controls.Add(this.tabPageFeet);
            this.tabBoxItems.Controls.Add(this.tabPageRingLeft);
            this.tabBoxItems.Controls.Add(this.tabPageRingRight);
            this.tabBoxItems.Controls.Add(this.tabPageBelt);
            this.tabBoxItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabBoxItems.Location = new System.Drawing.Point(3, 16);
            this.tabBoxItems.Multiline = true;
            this.tabBoxItems.Name = "tabBoxItems";
            this.tabBoxItems.SelectedIndex = 0;
            this.tabBoxItems.Size = new System.Drawing.Size(186, 277);
            this.tabBoxItems.TabIndex = 0;
            // 
            // tabPageHead
            // 
            this.tabPageHead.Location = new System.Drawing.Point(4, 76);
            this.tabPageHead.Name = "tabPageHead";
            this.tabPageHead.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHead.Size = new System.Drawing.Size(178, 197);
            this.tabPageHead.TabIndex = 0;
            this.tabPageHead.Text = "Head";
            this.tabPageHead.UseVisualStyleBackColor = true;
            // 
            // tabPageAmulet
            // 
            this.tabPageAmulet.Location = new System.Drawing.Point(4, 76);
            this.tabPageAmulet.Name = "tabPageAmulet";
            this.tabPageAmulet.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAmulet.Size = new System.Drawing.Size(178, 197);
            this.tabPageAmulet.TabIndex = 1;
            this.tabPageAmulet.Text = "Amulet";
            // 
            // tabPageWeaponLeft
            // 
            this.tabPageWeaponLeft.Location = new System.Drawing.Point(4, 76);
            this.tabPageWeaponLeft.Name = "tabPageWeaponLeft";
            this.tabPageWeaponLeft.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWeaponLeft.Size = new System.Drawing.Size(178, 197);
            this.tabPageWeaponLeft.TabIndex = 2;
            this.tabPageWeaponLeft.Text = "Weapon-Left";
            this.tabPageWeaponLeft.UseVisualStyleBackColor = true;
            // 
            // tabPageWeaponRight
            // 
            this.tabPageWeaponRight.Location = new System.Drawing.Point(4, 76);
            this.tabPageWeaponRight.Name = "tabPageWeaponRight";
            this.tabPageWeaponRight.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWeaponRight.Size = new System.Drawing.Size(178, 197);
            this.tabPageWeaponRight.TabIndex = 3;
            this.tabPageWeaponRight.Text = "Weapon-Right";
            this.tabPageWeaponRight.UseVisualStyleBackColor = true;
            // 
            // tabPageBody
            // 
            this.tabPageBody.Location = new System.Drawing.Point(4, 76);
            this.tabPageBody.Name = "tabPageBody";
            this.tabPageBody.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBody.Size = new System.Drawing.Size(178, 197);
            this.tabPageBody.TabIndex = 4;
            this.tabPageBody.Text = "Body";
            this.tabPageBody.UseVisualStyleBackColor = true;
            // 
            // tabPageHand
            // 
            this.tabPageHand.Location = new System.Drawing.Point(4, 76);
            this.tabPageHand.Name = "tabPageHand";
            this.tabPageHand.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHand.Size = new System.Drawing.Size(178, 197);
            this.tabPageHand.TabIndex = 5;
            this.tabPageHand.Text = "Hand";
            this.tabPageHand.UseVisualStyleBackColor = true;
            // 
            // tabPageFeet
            // 
            this.tabPageFeet.Location = new System.Drawing.Point(4, 76);
            this.tabPageFeet.Name = "tabPageFeet";
            this.tabPageFeet.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFeet.Size = new System.Drawing.Size(178, 197);
            this.tabPageFeet.TabIndex = 6;
            this.tabPageFeet.Text = "Feet";
            this.tabPageFeet.UseVisualStyleBackColor = true;
            // 
            // tabPageRingLeft
            // 
            this.tabPageRingLeft.Location = new System.Drawing.Point(4, 76);
            this.tabPageRingLeft.Name = "tabPageRingLeft";
            this.tabPageRingLeft.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRingLeft.Size = new System.Drawing.Size(178, 197);
            this.tabPageRingLeft.TabIndex = 7;
            this.tabPageRingLeft.Text = "Ring-Left";
            this.tabPageRingLeft.UseVisualStyleBackColor = true;
            // 
            // tabPageRingRight
            // 
            this.tabPageRingRight.Location = new System.Drawing.Point(4, 76);
            this.tabPageRingRight.Name = "tabPageRingRight";
            this.tabPageRingRight.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRingRight.Size = new System.Drawing.Size(178, 197);
            this.tabPageRingRight.TabIndex = 8;
            this.tabPageRingRight.Text = "Ring-Right";
            this.tabPageRingRight.UseVisualStyleBackColor = true;
            // 
            // tabPageBelt
            // 
            this.tabPageBelt.Location = new System.Drawing.Point(4, 76);
            this.tabPageBelt.Name = "tabPageBelt";
            this.tabPageBelt.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBelt.Size = new System.Drawing.Size(178, 197);
            this.tabPageBelt.TabIndex = 9;
            this.tabPageBelt.Text = "Belt";
            this.tabPageBelt.UseVisualStyleBackColor = true;
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 571);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DebugWindow";
            this.Text = "Debug";
            this.Load += new System.EventHandler(this.DebugWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabBoxItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel autosplitPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabControl tabBoxItems;
        private System.Windows.Forms.TabPage tabPageHead;
        private System.Windows.Forms.TabPage tabPageAmulet;
        private System.Windows.Forms.TabPage tabPageWeaponLeft;
        private System.Windows.Forms.TabPage tabPageWeaponRight;
        private System.Windows.Forms.TabPage tabPageBody;
        private System.Windows.Forms.TabPage tabPageHand;
        private System.Windows.Forms.TabPage tabPageFeet;
        private System.Windows.Forms.TabPage tabPageRingLeft;
        private System.Windows.Forms.TabPage tabPageRingRight;
        private System.Windows.Forms.TabPage tabPageBelt;
    }
}