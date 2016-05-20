using System;
using System.Windows.Forms;

namespace DiabloInterface
{
    public partial class AutoSplitSettingsRow : UserControl
    {

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

        AutoSplit autosplit;
        public AutoSplitSettingsRow( AutoSplit autosplit )
        {
            InitializeComponent();
            this.autosplit = autosplit;

            txtName.Text = autosplit.name;

            cmbType.Items.Add(new Item("Char Level", AutoSplit.TYPE_CHAR_LEVEL));
            cmbType.Items.Add(new Item("Area", AutoSplit.TYPE_AREA));
            cmbType.Items.Add(new Item("Item", AutoSplit.TYPE_ITEM));
            cmbType.Items.Add(new Item("Quest", AutoSplit.TYPE_QUEST));
            cmbType.Items.Add(new Item("Special", AutoSplit.TYPE_SPECIAL));
            cmbType.SelectedIndex = autosplit.type;

            cmbDifficulty.Items.Add(new Item("Normal", 0));
            cmbDifficulty.Items.Add(new Item("Nightmare", 1));
            cmbDifficulty.Items.Add(new Item("Hell", 2));
            cmbDifficulty.SelectedIndex = autosplit.difficulty;

            fillValueComboBox();

            var i = 0;
            foreach (Item item in cmbValue.Items)
            {
                if (item.Value == autosplit.value)
                {
                    cmbValue.SelectedIndex = i;
                    break;
                }
                i++;
            }
        }
        private void fillValueComboBox ()
        {


            cmbValue.Items.Clear();
            switch (autosplit.type)
            {
                case AutoSplit.TYPE_CHAR_LEVEL:
                    for (int i = 1; i < 100; i++)
                    {
                        cmbValue.Items.Add(new Item("" + i, i));
                    }
                    break;
                case AutoSplit.TYPE_AREA:

                    foreach (D2Level lvl in D2Level.getAll())
                    {
                        if (lvl.id > 0)
                        {
                            cmbValue.Items.Add(new Item(lvl.name, lvl.id));
                        }
                    }
                    break;
                case AutoSplit.TYPE_ITEM:
                    cmbValue.Items.Add(new Item("Horadric Cube", (int)D2Data.ItemId.HORADRIC_CUBE));
                    cmbValue.Items.Add(new Item("Horadric Shaft", (int)D2Data.ItemId.HORADRIC_SHAFT));
                    cmbValue.Items.Add(new Item("Horadric Amulet", (int)D2Data.ItemId.HORADRIC_AMULET));
                    cmbValue.Items.Add(new Item("Khalim's Eye", (int)D2Data.ItemId.KHALIM_EYE));
                    cmbValue.Items.Add(new Item("Khalim's Heart", (int)D2Data.ItemId.KHALIM_HEART));
                    cmbValue.Items.Add(new Item("Khalim's Brain", (int)D2Data.ItemId.KHALIM_BRAIN));
                    break;
                case AutoSplit.TYPE_QUEST:
                    cmbValue.Items.Add(new Item("Den of Evil", (int)D2Data.Quest.A1Q1));
                    cmbValue.Items.Add(new Item("Andariel", (int)D2Data.Quest.A1Q6));
                    cmbValue.Items.Add(new Item("Duriel", (int)D2Data.Quest.A2Q6));
                    cmbValue.Items.Add(new Item("Mephisto", (int)D2Data.Quest.A3Q6));
                    cmbValue.Items.Add(new Item("Diablo", (int)D2Data.Quest.A4Q2));
                    cmbValue.Items.Add(new Item("Ancients", (int)D2Data.Quest.A5Q5));
                    cmbValue.Items.Add(new Item("Baal", (int)D2Data.Quest.A5Q6));
                    break;
                case AutoSplit.TYPE_SPECIAL:
                    cmbValue.Items.Add(new Item("Game Start", (int)AutoSplit.Special.GAMESTART));
                    break;
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Item selectedItem = (Item)comboBox.SelectedItem;
            autosplit.type = (short)selectedItem.Value;
            fillValueComboBox();
        }

        private void cmbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item selectedValueItem = (Item)cmbValue.SelectedItem;
            if (selectedValueItem != null)
            {
                autosplit.value = (short)selectedValueItem.Value;
                if (autosplit.name == "")
                {
                    txtName.Text = selectedValueItem.Name;
                }
            }
            else
            {
                autosplit.value = -1;
            }
        }

        private void cmbDifficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item selectedValueItem = (Item)cmbDifficulty.SelectedItem;
            if (selectedValueItem != null)
            {
                autosplit.difficulty = (short)selectedValueItem.Value;
            }
            else
            {
                autosplit.difficulty = -1;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            autosplit.name = txtName.Text;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            autosplit.deleted = true;
            foreach (Control c in this.Controls )
            {
                c.Enabled = false;
            }
        }
    }
}
