namespace Zutatensuppe.DiabloInterface.Gui.Controls
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    using Zutatensuppe.D2Reader;
    using Zutatensuppe.D2Reader.Models;
    using Zutatensuppe.DiabloInterface.Business.AutoSplits;

    public partial class AutoSplitSettingsRow : UserControl
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
            public Item(string name, int value)
            {
                Name = name; Value = value;
            }
            public override string ToString()
            {
                // Generates the text shown in the combo box
                return Name;
            }
        }

        private bool updating;
        public event Action<AutoSplitSettingsRow> OnDelete;
        public bool IsDirty { get; private set; }

        public AutoSplit AutoSplit { get; private set; }

        public AutoSplitSettingsRow(AutoSplit autosplit)
        {
            updating = false;
            InitializeComponent();

            cmbType.Items.Add(new Item(LABEL_CHAR_LEVEL, (int)AutoSplit.SplitType.CharLevel));
            cmbType.Items.Add(new Item(LABEL_AREA, (int)AutoSplit.SplitType.Area));
            cmbType.Items.Add(new Item(LABEL_ITEM, (int)AutoSplit.SplitType.Item));
            cmbType.Items.Add(new Item(LABEL_QUEST, (int)AutoSplit.SplitType.Quest));
            cmbType.Items.Add(new Item(LABEL_SPECIAL, (int)AutoSplit.SplitType.Special));
            cmbType.Items.Add(new Item(LABEL_GEMS, (int)AutoSplit.SplitType.Gems));

            cmbDifficulty.Items.Clear();
            cmbDifficulty.Items.Add(new Item(LABEL_NORMAL, 0));
            cmbDifficulty.Items.Add(new Item(LABEL_NIGHTMARE, 1));
            cmbDifficulty.Items.Add(new Item(LABEL_HELL, 2));

            SetAutosplit(autosplit);
        }

        public void SetAutosplit(AutoSplit autosplit)
        {
            updating = true;
            txtName.Text = autosplit.Name;
            cmbType.SelectedIndex = (int)autosplit.Type;

            if (this.AutoSplit == null || autosplit.Type != this.AutoSplit.Type)
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
            this.AutoSplit = autosplit;
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
                        cmbValue.Items.Add(new Item("" + i, i));
                    }
                    break;
                case AutoSplit.SplitType.Area:
                    foreach (Area area in Area.getAreaList())
                    {
                        if (area.Id > 0) cmbValue.Items.Add(new Item(area.ToString(), area.Id));
                    }
                    break;
                case AutoSplit.SplitType.Item:
                    cmbValue.Items.Add(new Item(LABEL_HORADRIC_CUBE, (int)D2Data.ItemId.HORADRIC_CUBE));
                    cmbValue.Items.Add(new Item(LABEL_HORADRIC_SHAFT, (int)D2Data.ItemId.HORADRIC_SHAFT));
                    cmbValue.Items.Add(new Item(LABEL_HORADRIC_AMULET, (int)D2Data.ItemId.HORADRIC_AMULET));
                    cmbValue.Items.Add(new Item(LABEL_KHALIMS_EYE, (int)D2Data.ItemId.KHALIM_EYE));
                    cmbValue.Items.Add(new Item(LABEL_KHALIMS_HEART, (int)D2Data.ItemId.KHALIM_HEART));
                    cmbValue.Items.Add(new Item(LABEL_KHALIMS_BRAIN, (int)D2Data.ItemId.KHALIM_BRAIN));
                    break;
                case AutoSplit.SplitType.Quest:
                    foreach (QuestId questId in Enum.GetValues(typeof(QuestId)))
                    {
                        var quest = QuestFactory.Create(questId, 0);
                        var name = (quest.IsBossQuest ? "" : $"Act {quest.Act} - ") + quest.CommonName;

                        cmbValue.Items.Add(new Item(name, (int)questId));
                    }
                    break;
                case AutoSplit.SplitType.Special:
                    cmbValue.Items.Add(new Item(LABEL_GAME_START, (int)AutoSplit.Special.GameStart));
                    cmbValue.Items.Add(new Item(LABEL_CLEAR_100_PERCENT, (int)AutoSplit.Special.Clear100Percent));
                    cmbValue.Items.Add(new Item(LABEL_CLEAR_100_PERCENT_ALL, (int)AutoSplit.Special.Clear100PercentAllDifficulties));
                    break;
                case AutoSplit.SplitType.Gems:
                    foreach (Gem gem in Enum.GetValues(typeof(Gem)))
                    {
                        var name = Regex.Replace(gem.ToString(), @"(\B[A-Z])", " $1");
                        cmbValue.Items.Add(new Item(name, (int)gem));
                    }

                    break;
            }
        }

        public void MarkClean()
        {
            if (InvokeRequired)
            {
                // Delegate call to UI thread.
                Invoke((Action)(() => MarkClean()));
                return;
            }
            IsDirty = false;
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updating)
                return;

            ComboBox comboBox = (ComboBox)sender;
            Item selectedItem = (Item)comboBox.SelectedItem;
            AutoSplit.SplitType type = (AutoSplit.SplitType)selectedItem.Value;
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

            Item selectedValueItem = (Item)cmbValue.SelectedItem;
            if (selectedValueItem != null)
            {
                AutoSplit.Value = (short)selectedValueItem.Value;
                if (AutoSplit.Name == "" || AutoSplit.Name == "Unnamed")
                {
                    txtName.Text = selectedValueItem.Name;
                }
                if (AutoSplit.Type == AutoSplit.SplitType.Special)
                {
                    if (AutoSplit.Value == (int)AutoSplit.Special.Clear100Percent)
                    {
                        cmbDifficulty.Show();
                    }
                    else
                    {
                        cmbDifficulty.Hide();
                    }
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
            Item selectedValueItem = (Item)cmbDifficulty.SelectedItem;
            if (selectedValueItem != null)
            {
                AutoSplit.Difficulty = (short)selectedValueItem.Value;
            }
            else
            {
                AutoSplit.Difficulty = -1;
            }

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
