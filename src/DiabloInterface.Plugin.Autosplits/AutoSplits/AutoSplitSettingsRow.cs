namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits.AutoSplits
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Plugin.Autosplits.Data;

    public class AutoSplitSettingsRow : UserControl
    {
        static string LABEL_CHAR_LEVEL = "Level";
        static string LABEL_AREA = "Area";
        static string LABEL_ITEM = "Item";
        static string LABEL_QUEST = "Quest";
        static string LABEL_SPECIAL = "Special";
        static string LABEL_GEMS = "Gems";

        static string LABEL_NORMAL = "Normal";
        static string LABEL_NIGHTMARE = "Nightmare";
        static string LABEL_HELL = "Hell";

        static string LABEL_HORADRIC_CUBE = "Horadric Cube";
        static string LABEL_HORADRIC_SHAFT = "Horadric Shaft";
        static string LABEL_HORADRIC_AMULET = "Horadric Amulet";
        static string LABEL_KHALIMS_EYE = "Khalim's Eye";
        static string LABEL_KHALIMS_HEART = "Khalim's Heart";
        static string LABEL_KHALIMS_BRAIN = "Khalim's Brain";

        static string LABEL_GAME_START = "Game Start";
        static string LABEL_CLEAR_100_PERCENT = "Clear 100%";
        static string LABEL_CLEAR_100_PERCENT_ALL = "Clear 100% of all difficulties";

        class Item
        {
            public string Name;
            public int Value;

            // Generates the text shown in the combo box
            public override string ToString() => Name;
        }

        private bool updating;
        public event Action<AutoSplitSettingsRow> OnDelete;
        public bool IsDirty { get; private set; }

        public AutoSplit AutoSplit { get; private set; }

        private TextBox txtName;
        private ComboBox cmbType;
        private ComboBox cmbValue;
        private ComboBox cmbDifficulty;
        private Button btnDelete;
        private TableLayoutPanel tableLayoutPanel1;
        private void InitializeComponent()
        {
            txtName = new TextBox();
            cmbType = new ComboBox();
            cmbValue = new ComboBox();
            cmbDifficulty = new ComboBox();
            btnDelete = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();

            txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtName.Location = new System.Drawing.Point(3, 3);
            txtName.Size = new System.Drawing.Size(169, 20);
            txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);

            cmbType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.FormattingEnabled = true;
            cmbType.Location = new System.Drawing.Point(178, 3);
            cmbType.Size = new System.Drawing.Size(111, 21);
            cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);

            cmbValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbValue.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbValue.DropDownWidth = 300;
            cmbValue.FormattingEnabled = true;
            cmbValue.Location = new System.Drawing.Point(295, 3);
            cmbValue.Size = new System.Drawing.Size(169, 21);
            cmbValue.SelectedIndexChanged += new System.EventHandler(this.cmbValue_SelectedIndexChanged);

            cmbDifficulty.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbDifficulty.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDifficulty.FormattingEnabled = true;
            cmbDifficulty.Location = new System.Drawing.Point(470, 3);
            cmbDifficulty.Size = new System.Drawing.Size(111, 21);
            cmbDifficulty.SelectedIndexChanged += new System.EventHandler(this.cmbDifficulty_SelectedIndexChanged);

            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnDelete.Location = new System.Drawing.Point(587, 3);
            btnDelete.Size = new System.Drawing.Size(39, 21);
            btnDelete.Text = "X";
            btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 43F));
            tableLayoutPanel1.Controls.Add(btnDelete, 4, 0);
            tableLayoutPanel1.Controls.Add(txtName, 0, 0);
            tableLayoutPanel1.Controls.Add(cmbValue, 2, 0);
            tableLayoutPanel1.Controls.Add(cmbDifficulty, 3, 0);
            tableLayoutPanel1.Controls.Add(cmbType, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(629, 27);

            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(0);
            Size = new System.Drawing.Size(629, 27);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        public AutoSplitSettingsRow(AutoSplit autosplit)
        {
            updating = false;
            InitializeComponent();

            cmbType.Items.Add(new Item { Name = LABEL_CHAR_LEVEL, Value = (int)AutoSplit.SplitType.CharLevel });
            cmbType.Items.Add(new Item { Name = LABEL_AREA, Value = (int)AutoSplit.SplitType.Area });
            cmbType.Items.Add(new Item { Name = LABEL_ITEM, Value = (int)AutoSplit.SplitType.Item });
            cmbType.Items.Add(new Item { Name = LABEL_QUEST, Value = (int)AutoSplit.SplitType.Quest });
            cmbType.Items.Add(new Item { Name = LABEL_SPECIAL, Value = (int)AutoSplit.SplitType.Special });
            cmbType.Items.Add(new Item { Name = LABEL_GEMS, Value = (int)AutoSplit.SplitType.Gems });

            cmbDifficulty.Items.Clear();
            cmbDifficulty.Items.Add(new Item { Name = LABEL_NORMAL, Value = 0 });
            cmbDifficulty.Items.Add(new Item { Name = LABEL_NIGHTMARE, Value = 1 });
            cmbDifficulty.Items.Add(new Item { Name = LABEL_HELL, Value = 2 });

            SetAutosplit(autosplit);
        }

        public void SetAutosplit(AutoSplit autosplit)
        {
            updating = true;
            txtName.Text = autosplit.Name;
            cmbType.SelectedIndex = (int)autosplit.Type;

            if (AutoSplit == null || autosplit.Type != AutoSplit.Type)
            {
                FillComboBoxes(autosplit);
            }

            cmbDifficulty.SelectedIndex = autosplit.Difficulty;

            var i = 0;
            foreach (Item item in cmbValue.Items)
            {
                if (item.Value == autosplit.Value)
                {
                    cmbValue.SelectedIndex = i;
                    break;
                }
                i++;
            }
            AutoSplit = autosplit;
            updating = false;
        }

        private void FillComboBoxes(AutoSplit autosplit)
        {
            if (autosplit.IsDifficultyIgnored())
            {
                cmbDifficulty.Hide();
            }
            else
            {
                cmbDifficulty.Show();
                cmbDifficulty.SelectedIndex = autosplit.Difficulty;
            }

            cmbValue.Items.Clear();
            switch (autosplit.Type)
            {
                case AutoSplit.SplitType.CharLevel:
                    for (int i = 1; i < 100; i++)
                    {
                        cmbValue.Items.Add(new Item { Name = $"Lvl {i}", Value = i });
                    }
                    break;
                case AutoSplit.SplitType.Area:
                    foreach (Area area in Area.getAreaList())
                    {
                        cmbValue.Items.Add(new Item { Name = area.ToString(), Value = area.Id });
                    }
                    break;
                case AutoSplit.SplitType.Item:
                    cmbValue.Items.Add(new Item { Name = LABEL_HORADRIC_CUBE, Value = (int)D2Data.ItemId.HORADRIC_CUBE });
                    cmbValue.Items.Add(new Item { Name = LABEL_HORADRIC_SHAFT, Value = (int)D2Data.ItemId.HORADRIC_SHAFT });
                    cmbValue.Items.Add(new Item { Name = LABEL_HORADRIC_AMULET, Value = (int)D2Data.ItemId.HORADRIC_AMULET });
                    cmbValue.Items.Add(new Item { Name = LABEL_KHALIMS_EYE, Value = (int)D2Data.ItemId.KHALIM_EYE });
                    cmbValue.Items.Add(new Item { Name = LABEL_KHALIMS_HEART, Value = (int)D2Data.ItemId.KHALIM_HEART });
                    cmbValue.Items.Add(new Item { Name = LABEL_KHALIMS_BRAIN, Value = (int)D2Data.ItemId.KHALIM_BRAIN });
                    break;
                case AutoSplit.SplitType.Quest:
                    foreach (QuestId questId in Enum.GetValues(typeof(QuestId)))
                    {
                        cmbValue.Items.Add(new Item { Name = QuestName(questId), Value = (int)questId });
                    }
                    break;
                case AutoSplit.SplitType.Special:
                    cmbValue.Items.Add(new Item { Name = LABEL_GAME_START, Value = (int)AutoSplit.Special.GameStart });
                    cmbValue.Items.Add(new Item { Name = LABEL_CLEAR_100_PERCENT, Value = (int)AutoSplit.Special.Clear100Percent });
                    cmbValue.Items.Add(new Item { Name = LABEL_CLEAR_100_PERCENT_ALL, Value = (int)AutoSplit.Special.Clear100PercentAllDifficulties });
                    break;
                case AutoSplit.SplitType.Gems:
                    foreach (Gem gem in Enum.GetValues(typeof(Gem)))
                    {
                        cmbValue.Items.Add(new Item { Name = GemName(gem), Value = (int)gem });
                    }

                    break;
            }
        }

        private string QuestName(QuestId questId)
        {
            var quest = QuestFactory.Create(questId, 0);
            return (quest.IsBossQuest ? "" : $"Act {quest.Act} - ") + quest.CommonName;
        }

        private string GemName(Gem gem)
        {
            return Regex.Replace(gem.ToString(), @"(\B[A-Z])", " $1");
        }

        public void MarkClean()
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => MarkClean()));
                return;
            }
            IsDirty = false;
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updating)
                return;

            var comboBox = (ComboBox)sender;
            var selectedItem = (Item)comboBox.SelectedItem;
            var type = (AutoSplit.SplitType)selectedItem.Value;
            if (type != AutoSplit.Type)
            {
                AutoSplit.Type = type;
                FillComboBoxes(AutoSplit);
                IsDirty = true;
            }
        }

        private void cmbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updating)
                return;

            var selectedValueItem = (Item)cmbValue.SelectedItem;
            if (selectedValueItem != null)
            {
                AutoSplit.Value = (short)selectedValueItem.Value;
                if (AutoSplit.Name == "" || AutoSplit.Name == AutoSplit.DEFAULT_NAME)
                {
                    txtName.Text = selectedValueItem.Name;
                }
                if (AutoSplit.Type == AutoSplit.SplitType.Special)
                {
                    cmbDifficulty.Visible = AutoSplit.Value == (int)AutoSplit.Special.Clear100Percent;
                }
            }
            else
            {
                AutoSplit.Value = -1;
            }

            IsDirty = true;
        }

        private void cmbDifficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updating)
                return;

            var item = (Item)cmbDifficulty.SelectedItem;
            AutoSplit.Difficulty = (short)(item == null ? -1 : item.Value);
            IsDirty = true;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (updating)
                return;

            AutoSplit.Name = txtName.Text;

            IsDirty = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OnDelete?.Invoke(this);
        }
    }
}
