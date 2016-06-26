using DiabloInterface.D2;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DiabloInterface.Gui.Controls
{
    public partial class AutoSplitSettingsRow : UserControl
    {
        private static string LABEL_CHAR_LEVEL = "Level";
        private static string LABEL_AREA = "Area";
        private static string LABEL_ITEM = "Item";
        private static string LABEL_QUEST = "Quest";
        private static string LABEL_SPECIAL = "Special";

        private static string LABEL_NORMAL = "Normal";
        private static string LABEL_NIGHTMARE = "Nightmare";
        private static string LABEL_HELL = "Hell";

        private static string LABEL_HORADRIC_CUBE = "Horadric Cube";
        private static string LABEL_HORADRIC_SHAFT = "Horadric Shaft";
        private static string LABEL_HORADRIC_AMULET = "Horadric Amulet";
        private static string LABEL_KHALIMS_EYE = "Khalim's Eye";
        private static string LABEL_KHALIMS_HEART = "Khalim's Heart";
        private static string LABEL_KHALIMS_BRAIN = "Khalim's Brain";

        private static string LABEL_GAME_START = "Game Start";
        private static string LABEL_CLEAR_100_PERCENT = "Clear 100%";
        private static string LABEL_CLEAR_100_PERCENT_ALL = "Clear 100% of all difficulties";

        private class Item
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

        public event Action<AutoSplitSettingsRow> OnDelete;
        public bool IsDirty { get; private set; }

        public AutoSplit AutoSplit { get; private set; }

        public AutoSplitSettingsRow(AutoSplit autosplit)
        {
            InitializeComponent();

            AutoSplit = autosplit;

            txtName.Text = autosplit.Name;

            cmbType.Items.Add(new Item(LABEL_CHAR_LEVEL, (int)AutoSplit.SplitType.CharLevel));
            cmbType.Items.Add(new Item(LABEL_AREA, (int)AutoSplit.SplitType.Area));
            cmbType.Items.Add(new Item(LABEL_ITEM, (int)AutoSplit.SplitType.Item));
            cmbType.Items.Add(new Item(LABEL_QUEST, (int)AutoSplit.SplitType.Quest));
            cmbType.Items.Add(new Item(LABEL_SPECIAL, (int)AutoSplit.SplitType.Special));
            cmbType.SelectedIndex = (int)autosplit.Type;

            FillComboBoxes();

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
        }
        
        private void FillComboBoxes()
        {
            cmbDifficulty.Items.Clear();
            cmbDifficulty.Items.Add(new Item(LABEL_NORMAL, 0));
            cmbDifficulty.Items.Add(new Item(LABEL_NIGHTMARE, 1));
            cmbDifficulty.Items.Add(new Item(LABEL_HELL, 2));
            cmbDifficulty.SelectedIndex = AutoSplit.Difficulty;
            cmbDifficulty.Show();
            if ( AutoSplit.IsDifficultyIgnored() )
            {
                cmbDifficulty.Hide();
            } else
            {
                cmbDifficulty.SelectedIndex = AutoSplit.Difficulty;
            }

            cmbValue.Items.Clear();
            switch (AutoSplit.Type)
            {
                case AutoSplit.SplitType.CharLevel:
                    for (int i = 1; i < 100; i++)
                    {
                        cmbValue.Items.Add(new Item("" + i, i));
                    }
                    break;
                case AutoSplit.SplitType.Area:
                    foreach (D2Level lvl in D2Level.getAll())
                    {
                        if (lvl.id > 0) cmbValue.Items.Add(new Item(lvl.name, lvl.id));
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
                    foreach ( KeyValuePair<D2QuestHelper.Quest, D2QuestHelper.D2Quest> item in D2QuestHelper.Quests )
                    {
                        cmbValue.Items.Add(new Item((item.Value.BossQuest ? "" : ("Act " + item.Value.Act + " - ")) + item.Value.CommonName, (int)item.Key));
                    }
                    break;
                case AutoSplit.SplitType.Special:
                    cmbValue.Items.Add(new Item(LABEL_GAME_START, (int)AutoSplit.Special.GameStart));
                    cmbValue.Items.Add(new Item(LABEL_CLEAR_100_PERCENT, (int)AutoSplit.Special.Clear100Percent));
                    cmbValue.Items.Add(new Item(LABEL_CLEAR_100_PERCENT_ALL, (int)AutoSplit.Special.Clear100PercentAllDifficulties));
                    break;
            }
        }

        public void MarkClean()
        {
            IsDirty = false;
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Item selectedItem = (Item)comboBox.SelectedItem;
            AutoSplit.Type = (AutoSplit.SplitType)selectedItem.Value;
            FillComboBoxes();

            IsDirty = true;
        }

        private void cmbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            AutoSplit.Name = txtName.Text;

            IsDirty = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (OnDelete != null)
            {
                OnDelete(this);
            }
        }
    }
}
