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
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.splitContainer1.Size = new System.Drawing.Size(663, 542);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // classSettingsGroupBox
            // 
            this.classSettingsGroupBox.Controls.Add(this.deleteSettingsButton);
            this.classSettingsGroupBox.Controls.Add(this.addSettingsButton);
            this.classSettingsGroupBox.Controls.Add(this.characterListBox);
            this.classSettingsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classSettingsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.classSettingsGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.classSettingsGroupBox.MinimumSize = new System.Drawing.Size(187, 0);
            this.classSettingsGroupBox.Name = "classSettingsGroupBox";
            this.classSettingsGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.classSettingsGroupBox.Size = new System.Drawing.Size(200, 542);
            this.classSettingsGroupBox.TabIndex = 0;
            this.classSettingsGroupBox.TabStop = false;
            this.classSettingsGroupBox.Text = "Class Settings";
            // 
            // deleteSettingsButton
            // 
            this.deleteSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteSettingsButton.Location = new System.Drawing.Point(43, 510);
            this.deleteSettingsButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.deleteSettingsButton.Name = "deleteSettingsButton";
            this.deleteSettingsButton.Size = new System.Drawing.Size(79, 28);
            this.deleteSettingsButton.TabIndex = 2;
            this.deleteSettingsButton.Text = "Remove";
            this.deleteSettingsButton.UseVisualStyleBackColor = true;
            this.deleteSettingsButton.Click += new System.EventHandler(this.DeleteSettingsButtonOnClick);
            // 
            // addSettingsButton
            // 
            this.addSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addSettingsButton.Location = new System.Drawing.Point(129, 510);
            this.addSettingsButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.addSettingsButton.Name = "addSettingsButton";
            this.addSettingsButton.Size = new System.Drawing.Size(67, 28);
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
            this.characterListBox.ItemHeight = 16;
            this.characterListBox.Location = new System.Drawing.Point(4, 20);
            this.characterListBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.characterListBox.Name = "characterListBox";
            this.characterListBox.Size = new System.Drawing.Size(191, 484);
            this.characterListBox.TabIndex = 0;
            this.characterListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CharacterListBoxOnMouseDoubleClick);
            // 
            // runesGroupBox
            // 
            this.runesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runesGroupBox.Controls.Add(this.runeFlowLayout);
            this.runesGroupBox.Location = new System.Drawing.Point(4, 4);
            this.runesGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.runesGroupBox.Name = "runesGroupBox";
            this.runesGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.runesGroupBox.Size = new System.Drawing.Size(450, 385);
            this.runesGroupBox.TabIndex = 0;
            this.runesGroupBox.TabStop = false;
            this.runesGroupBox.Text = "Runes";
            // 
            // runeFlowLayout
            // 
            this.runeFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runeFlowLayout.Location = new System.Drawing.Point(4, 19);
            this.runeFlowLayout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.runeFlowLayout.Name = "runeFlowLayout";
            this.runeFlowLayout.Size = new System.Drawing.Size(442, 362);
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
            this.runeListEditBox.Location = new System.Drawing.Point(4, 396);
            this.runeListEditBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.runeListEditBox.Name = "runeListEditBox";
            this.runeListEditBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.runeListEditBox.Size = new System.Drawing.Size(450, 142);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 17);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(442, 47);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // difficultyComboBox
            // 
            this.difficultyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.difficultyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.difficultyComboBox.FormattingEnabled = true;
            this.difficultyComboBox.Location = new System.Drawing.Point(225, 21);
            this.difficultyComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.difficultyComboBox.Name = "difficultyComboBox";
            this.difficultyComboBox.Size = new System.Drawing.Size(213, 24);
            this.difficultyComboBox.TabIndex = 5;
            this.difficultyComboBox.SelectedValueChanged += new System.EventHandler(this.DifficultyComboBoxOnSelectedValueChanged);
            // 
            // characterClassComboBox
            // 
            this.characterClassComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.characterClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.characterClassComboBox.FormattingEnabled = true;
            this.characterClassComboBox.Location = new System.Drawing.Point(4, 21);
            this.characterClassComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.characterClassComboBox.Name = "characterClassComboBox";
            this.characterClassComboBox.Size = new System.Drawing.Size(213, 24);
            this.characterClassComboBox.TabIndex = 4;
            this.characterClassComboBox.SelectedValueChanged += new System.EventHandler(this.CharacterClassComboBoxOnSelectedValueChanged);
            // 
            // classLabel
            // 
            this.classLabel.AutoSize = true;
            this.classLabel.Location = new System.Drawing.Point(4, 0);
            this.classLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.classLabel.Name = "classLabel";
            this.classLabel.Size = new System.Drawing.Size(42, 17);
            this.classLabel.TabIndex = 6;
            this.classLabel.Text = "Class";
            // 
            // difficultyLabel
            // 
            this.difficultyLabel.AutoSize = true;
            this.difficultyLabel.Location = new System.Drawing.Point(225, 0);
            this.difficultyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.difficultyLabel.Name = "difficultyLabel";
            this.difficultyLabel.Size = new System.Drawing.Size(61, 17);
            this.difficultyLabel.TabIndex = 7;
            this.difficultyLabel.Text = "Difficulty";
            // 
            // runeComboBox
            // 
            this.runeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.runeComboBox.FormattingEnabled = true;
            this.runeComboBox.Location = new System.Drawing.Point(8, 70);
            this.runeComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.runeComboBox.Name = "runeComboBox";
            this.runeComboBox.Size = new System.Drawing.Size(305, 24);
            this.runeComboBox.TabIndex = 3;
            // 
            // runeButton
            // 
            this.runeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.runeButton.Location = new System.Drawing.Point(322, 68);
            this.runeButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.runeButton.Name = "runeButton";
            this.runeButton.Size = new System.Drawing.Size(120, 28);
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
            this.runewordComboBox.Location = new System.Drawing.Point(8, 106);
            this.runewordComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.runewordComboBox.Name = "runewordComboBox";
            this.runewordComboBox.Size = new System.Drawing.Size(305, 24);
            this.runewordComboBox.TabIndex = 10;
            // 
            // runewordButton
            // 
            this.runewordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.runewordButton.Location = new System.Drawing.Point(322, 103);
            this.runewordButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.runewordButton.Name = "runewordButton";
            this.runewordButton.Size = new System.Drawing.Size(120, 28);
            this.runewordButton.TabIndex = 9;
            this.runewordButton.Text = "Add";
            this.runewordButton.UseVisualStyleBackColor = true;
            this.runewordButton.Click += new System.EventHandler(this.RunewordButtonOnClick);
            // 
            // RuneSettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "RuneSettingsPage";
            this.Size = new System.Drawing.Size(663, 542);
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
