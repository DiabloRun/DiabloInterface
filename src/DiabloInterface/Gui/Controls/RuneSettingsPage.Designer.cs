namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    partial class RuneSettingsPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.classSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.deleteSettingsButton = new System.Windows.Forms.Button();
            this.addSettingsButton = new System.Windows.Forms.Button();
            this.characterListBox = new System.Windows.Forms.ListBox();
            this.runesGroupBox = new System.Windows.Forms.GroupBox();
            this.runeFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.runeListEditBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.difficultyComboBox = new System.Windows.Forms.ComboBox();
            this.characterClassComboBox = new System.Windows.Forms.ComboBox();
            this.classLabel = new System.Windows.Forms.Label();
            this.difficultyLabel = new System.Windows.Forms.Label();
            this.runeComboBox = new System.Windows.Forms.ComboBox();
            this.runeButton = new System.Windows.Forms.Button();
            this.runewordComboBox = new System.Windows.Forms.ComboBox();
            this.runewordButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.classSettingsGroupBox.SuspendLayout();
            this.runesGroupBox.SuspendLayout();
            this.runeListEditBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.classSettingsGroupBox);
            this.splitContainer1.Panel1MinSize = 140;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.runesGroupBox);
            this.splitContainer1.Panel2.Controls.Add(this.runeListEditBox);
            this.splitContainer1.Panel2MinSize = 200;
            this.splitContainer1.Size = new System.Drawing.Size(497, 440);
            this.splitContainer1.SplitterDistance = 150;
            this.splitContainer1.TabIndex = 0;
            // 
            // classSettingsGroupBox
            // 
            this.classSettingsGroupBox.Controls.Add(this.deleteSettingsButton);
            this.classSettingsGroupBox.Controls.Add(this.addSettingsButton);
            this.classSettingsGroupBox.Controls.Add(this.characterListBox);
            this.classSettingsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classSettingsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.classSettingsGroupBox.MinimumSize = new System.Drawing.Size(140, 0);
            this.classSettingsGroupBox.Name = "classSettingsGroupBox";
            this.classSettingsGroupBox.Size = new System.Drawing.Size(150, 440);
            this.classSettingsGroupBox.TabIndex = 0;
            this.classSettingsGroupBox.TabStop = false;
            this.classSettingsGroupBox.Text = "Class Settings";
            // 
            // deleteSettingsButton
            // 
            this.deleteSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteSettingsButton.Location = new System.Drawing.Point(32, 414);
            this.deleteSettingsButton.Name = "deleteSettingsButton";
            this.deleteSettingsButton.Size = new System.Drawing.Size(59, 23);
            this.deleteSettingsButton.TabIndex = 2;
            this.deleteSettingsButton.Text = "Remove";
            this.deleteSettingsButton.UseVisualStyleBackColor = true;
            this.deleteSettingsButton.Click += new System.EventHandler(this.DeleteSettingsButtonOnClick);
            // 
            // addSettingsButton
            // 
            this.addSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addSettingsButton.Location = new System.Drawing.Point(97, 414);
            this.addSettingsButton.Name = "addSettingsButton";
            this.addSettingsButton.Size = new System.Drawing.Size(50, 23);
            this.addSettingsButton.TabIndex = 1;
            this.addSettingsButton.Text = "Add";
            this.addSettingsButton.UseVisualStyleBackColor = true;
            this.addSettingsButton.Click += new System.EventHandler(this.AddSettingsButtonOnClick);
            // 
            // characterListBox
            // 
            this.characterListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.characterListBox.FormattingEnabled = true;
            this.characterListBox.Location = new System.Drawing.Point(3, 16);
            this.characterListBox.Name = "characterListBox";
            this.characterListBox.Size = new System.Drawing.Size(144, 394);
            this.characterListBox.TabIndex = 0;
            this.characterListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CharacterListBoxOnMouseDoubleClick);
            // 
            // runesGroupBox
            // 
            this.runesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runesGroupBox.Controls.Add(this.runeFlowLayout);
            this.runesGroupBox.Location = new System.Drawing.Point(3, 3);
            this.runesGroupBox.Name = "runesGroupBox";
            this.runesGroupBox.Size = new System.Drawing.Size(337, 313);
            this.runesGroupBox.TabIndex = 0;
            this.runesGroupBox.TabStop = false;
            this.runesGroupBox.Text = "Runes";
            // 
            // runeFlowLayout
            // 
            this.runeFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runeFlowLayout.Location = new System.Drawing.Point(3, 16);
            this.runeFlowLayout.Name = "runeFlowLayout";
            this.runeFlowLayout.Size = new System.Drawing.Size(331, 294);
            this.runeFlowLayout.TabIndex = 0;
            // 
            // runeListEditBox
            // 
            this.runeListEditBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runeListEditBox.Controls.Add(this.tableLayoutPanel1);
            this.runeListEditBox.Controls.Add(this.runeComboBox);
            this.runeListEditBox.Controls.Add(this.runeButton);
            this.runeListEditBox.Controls.Add(this.runewordComboBox);
            this.runeListEditBox.Controls.Add(this.runewordButton);
            this.runeListEditBox.Location = new System.Drawing.Point(3, 322);
            this.runeListEditBox.Name = "runeListEditBox";
            this.runeListEditBox.Size = new System.Drawing.Size(337, 115);
            this.runeListEditBox.TabIndex = 0;
            this.runeListEditBox.TabStop = false;
            this.runeListEditBox.Text = "Edit";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.difficultyComboBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.characterClassComboBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.classLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.difficultyLabel, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 14);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(331, 38);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // difficultyComboBox
            // 
            this.difficultyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.difficultyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.difficultyComboBox.FormattingEnabled = true;
            this.difficultyComboBox.Location = new System.Drawing.Point(168, 17);
            this.difficultyComboBox.Name = "difficultyComboBox";
            this.difficultyComboBox.Size = new System.Drawing.Size(160, 21);
            this.difficultyComboBox.TabIndex = 5;
            this.difficultyComboBox.SelectedValueChanged += new System.EventHandler(this.DifficultyComboBoxOnSelectedValueChanged);
            // 
            // characterClassComboBox
            // 
            this.characterClassComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.characterClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.characterClassComboBox.FormattingEnabled = true;
            this.characterClassComboBox.Location = new System.Drawing.Point(3, 17);
            this.characterClassComboBox.Name = "characterClassComboBox";
            this.characterClassComboBox.Size = new System.Drawing.Size(159, 21);
            this.characterClassComboBox.TabIndex = 4;
            this.characterClassComboBox.SelectedValueChanged += new System.EventHandler(this.CharacterClassComboBoxOnSelectedValueChanged);
            // 
            // classLabel
            // 
            this.classLabel.AutoSize = true;
            this.classLabel.Location = new System.Drawing.Point(3, 0);
            this.classLabel.Name = "classLabel";
            this.classLabel.Size = new System.Drawing.Size(36, 13);
            this.classLabel.TabIndex = 6;
            this.classLabel.Text = "Class";
            // 
            // difficultyLabel
            // 
            this.difficultyLabel.AutoSize = true;
            this.difficultyLabel.Location = new System.Drawing.Point(168, 0);
            this.difficultyLabel.Name = "difficultyLabel";
            this.difficultyLabel.Size = new System.Drawing.Size(51, 13);
            this.difficultyLabel.TabIndex = 7;
            this.difficultyLabel.Text = "Difficulty";
            // 
            // runeComboBox
            // 
            this.runeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.runeComboBox.FormattingEnabled = true;
            this.runeComboBox.Location = new System.Drawing.Point(6, 57);
            this.runeComboBox.Name = "runeComboBox";
            this.runeComboBox.Size = new System.Drawing.Size(229, 21);
            this.runeComboBox.TabIndex = 3;
            // 
            // runeButton
            // 
            this.runeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.runeButton.Location = new System.Drawing.Point(241, 55);
            this.runeButton.Name = "runeButton";
            this.runeButton.Size = new System.Drawing.Size(90, 23);
            this.runeButton.TabIndex = 0;
            this.runeButton.Text = "Add";
            this.runeButton.UseVisualStyleBackColor = true;
            this.runeButton.Click += new System.EventHandler(this.RuneButtonOnClick);
            // 
            // runewordComboBox
            // 
            this.runewordComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runewordComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.runewordComboBox.FormattingEnabled = true;
            this.runewordComboBox.Location = new System.Drawing.Point(6, 86);
            this.runewordComboBox.Name = "runewordComboBox";
            this.runewordComboBox.Size = new System.Drawing.Size(229, 21);
            this.runewordComboBox.TabIndex = 10;
            // 
            // runewordButton
            // 
            this.runewordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.runewordButton.Location = new System.Drawing.Point(241, 84);
            this.runewordButton.Name = "runewordButton";
            this.runewordButton.Size = new System.Drawing.Size(90, 23);
            this.runewordButton.TabIndex = 9;
            this.runewordButton.Text = "Add";
            this.runewordButton.UseVisualStyleBackColor = true;
            this.runewordButton.Click += new System.EventHandler(this.RunewordButtonOnClick);
            // 
            // RuneSettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "RuneSettingsPage";
            this.Size = new System.Drawing.Size(497, 440);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.classSettingsGroupBox.ResumeLayout(false);
            this.runesGroupBox.ResumeLayout(false);
            this.runeListEditBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox classSettingsGroupBox;
        private System.Windows.Forms.ListBox characterListBox;
        private System.Windows.Forms.FlowLayoutPanel runeFlowLayout;
        private System.Windows.Forms.ComboBox runeComboBox;
        private System.Windows.Forms.Button runeButton;
        private System.Windows.Forms.Label difficultyLabel;
        private System.Windows.Forms.Label classLabel;
        private System.Windows.Forms.ComboBox difficultyComboBox;
        private System.Windows.Forms.ComboBox characterClassComboBox;
        private System.Windows.Forms.GroupBox runesGroupBox;
        private System.Windows.Forms.GroupBox runeListEditBox;
        private System.Windows.Forms.Button deleteSettingsButton;
        private System.Windows.Forms.Button addSettingsButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox runewordComboBox;
        private System.Windows.Forms.Button runewordButton;
    }
}
