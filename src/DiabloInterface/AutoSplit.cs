using System.Drawing;
using System.Windows.Forms;

namespace DiabloInterface
{
    public class AutoSplit
    {
        enum ItemId
        {
            GIBDINN = 87,
            WIRTS_LEG = 88,
            HORADRIC_MALUS = 89,
            HELLFORGE_HAMMER = 90,
            HORADRIC_CUBE = 549,
            HORADRIC_SHAFT = 92,
            HORADRIC_STAFF = 91,
            HORADRIC_AMULET = 521,
            KHALIM_EYE = 553,
            KHALIM_HEART = 554,
            KHALIM_BRAIN = 555,
            KHALIM_FLAIL = 173,
            KHALIM_WILL = 174,
            INIFUSS_SCROLL = 524,
            POTION_OF_LIFE = 545,
            JADE_FIGURINE = 546,
            GOLDEN_BIRD = 547,
            LAM_ESEN_TOME = 548,
            MEPHISTOS_SOULSTONE = 551,
            BOOK_OF_SKILL = 552,
        }

        public enum Special
        {
            GAMESTART = 1,
        }

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

        public const int TYPE_NONE = -1;
        public const int TYPE_CHAR_LEVEL = 0;
        public const int TYPE_AREA = 1;
        public const int TYPE_ITEM = 2;
        public const int TYPE_QUEST = 3;
        public const int TYPE_SPECIAL = 4;

        private string _name = "";
        private int _type = TYPE_NONE;
        private int _value = -1;

        public int type
        {
            get { return this._type; }
        }
        public int value
        {
            get { return _value; }
        }
        public string name
        {
            get { return _name; }
        }

        private Control control;
        private bool _reached = false;
        private bool _deleted = false;

        public bool reached
        {
            get { return this._reached; }
            set
            {
                this._reached = value;
                updateControl();
            }
        }
        public bool deleted
        {
            get { return this._deleted; }
            set
            {
                this._deleted = value;
                txtName.Enabled = false;
                cmbType.Enabled = false;
                cmbValueCharLevel.Enabled = false;
                cmbValueArea.Enabled = false;
                cmbValueItem.Enabled = false;
                cmbValueQuest.Enabled = false;
            }
        }


        public void updateControl()
        {
            if (this.control == null)
            {
                return;
            }

            this.control.Text = this.name;
                
            if (this._reached)
            {
                this.control.ForeColor = Color.Lime;
            }
            else
            {
                this.control.ForeColor = Color.Red;
            }

            if (this._deleted)
            {
                this.control.Parent.Controls.Remove(this.control);
                this.control = null;
            }
        }
        public void bindControl(Control control)
        {
            this.control = control;
            updateControl();
        }

        #region setting controls
        private TextBox txtName;
        private ComboBox cmbType;
        private ComboBox cmbValueCharLevel;
        private ComboBox cmbValueArea;
        private ComboBox cmbValueItem;
        private ComboBox cmbValueQuest;
        private ComboBox cmbValueSpecial;
        #endregion

        public AutoSplit()
        {

        }
        public AutoSplit(string name, int type, int value)
        {
            this._name = name;
            this._type = type;
            this._value = value;
        }

        public void setSettingControls(
            TextBox txtName,
            ComboBox cmbType,
            ComboBox cmbValueCharLevel,
            ComboBox cmbValueArea,
            ComboBox cmbValueItem,
            ComboBox cmbValueQuest,
            ComboBox cmbValueSpecial)
        {
            this.txtName = txtName;
            this.cmbType = cmbType;
            this.cmbValueCharLevel = cmbValueCharLevel;
            this.cmbValueArea = cmbValueArea;
            this.cmbValueItem = cmbValueItem;
            this.cmbValueQuest = cmbValueQuest;
            this.cmbValueSpecial = cmbValueSpecial;

            txtName.TextChanged += TxtName_TextChanged;

            cmbType.Items.Add(new Item("Char Level", AutoSplit.TYPE_CHAR_LEVEL));
            cmbType.Items.Add(new Item("Area", AutoSplit.TYPE_AREA));
            cmbType.Items.Add(new Item("Item", AutoSplit.TYPE_ITEM));
            cmbType.Items.Add(new Item("Quest", AutoSplit.TYPE_QUEST));
            cmbType.Items.Add(new Item("Special", AutoSplit.TYPE_SPECIAL));

            int i = 0;

            for (i = 1; i < 100; i++)
            {
                cmbValueCharLevel.Items.Add(new Item("" + i, i));
            }

            cmbValueItem.Items.Add(new Item("Horadric Cube", (int)ItemId.HORADRIC_CUBE));
            cmbValueItem.Items.Add(new Item("Horadric Shaft", (int)ItemId.HORADRIC_SHAFT));
            cmbValueItem.Items.Add(new Item("Horadric Amulet", (int)ItemId.HORADRIC_AMULET));
            cmbValueItem.Items.Add(new Item("Khalim's Eye", (int)ItemId.KHALIM_EYE));
            cmbValueItem.Items.Add(new Item("Khalim's Heart", (int)ItemId.KHALIM_HEART));
            cmbValueItem.Items.Add(new Item("Khalim's Brain", (int)ItemId.KHALIM_BRAIN));


            cmbValueSpecial.Items.Add(new Item("Game Start", (int)Special.GAMESTART));

            foreach (Level lvl in Level.getAll() )
            {
                if (lvl.id > 0)
                {
                    cmbValueArea.Items.Add(new Item(lvl.name, lvl.id));
                }
            }

            cmbValueQuest.Items.Add(new Item("Den of Evil", (int)D2Data.QUEST_A1Q1));
            cmbValueQuest.Items.Add(new Item("Andariel", (int)D2Data.QUEST_A1Q6));
            cmbValueQuest.Items.Add(new Item("Duriel", (int)D2Data.QUEST_A2Q6));
            cmbValueQuest.Items.Add(new Item("Mephisto", (int)D2Data.QUEST_A3Q6));
            cmbValueQuest.Items.Add(new Item("Diablo", (int)D2Data.QUEST_A4Q2));
            cmbValueQuest.Items.Add(new Item("Ancients", (int)D2Data.QUEST_A5Q5));
            cmbValueQuest.Items.Add(new Item("Baal", (int)D2Data.QUEST_A5Q6));

            i = 0;
            foreach (Item item in cmbType.Items)
            {
                if (item.Value == _type)
                {
                    cmbType.SelectedIndex = i;
                    break;
                }
                i++;
            }

            switch (_type)
            {
                case AutoSplit.TYPE_CHAR_LEVEL:
                    i = 0;
                    foreach (Item item in cmbValueCharLevel.Items)
                    {
                        if (item.Value == _value)
                        {
                            cmbValueCharLevel.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    cmbValueCharLevel.Show();
                    break;
                case AutoSplit.TYPE_AREA:
                    i = 0;
                    foreach (Item item in cmbValueArea.Items)
                    {
                        if (item.Value == _value)
                        {
                            cmbValueArea.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    cmbValueArea.Show();
                    break;
                case AutoSplit.TYPE_ITEM:
                    i = 0;
                    foreach (Item item in cmbValueItem.Items)
                    {
                        if (item.Value == _value)
                        {
                            cmbValueItem.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    cmbValueItem.Show();
                    break;
                case AutoSplit.TYPE_QUEST:
                    i = 0;
                    foreach (Item item in cmbValueQuest.Items)
                    {
                        if (item.Value == _value)
                        {
                            cmbValueQuest.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    cmbValueQuest.Show();
                    break;
                case AutoSplit.TYPE_SPECIAL:
                    i = 0;
                    foreach (Item item in cmbValueSpecial.Items)
                    {
                        if (item.Value == _value)
                        {
                            cmbValueSpecial.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    cmbValueSpecial.Show();
                    break;
            }

            txtName.Text = _name;

            cmbType.SelectedIndexChanged += new System.EventHandler(cmbType_SelectedIndexChanged);
            cmbValueCharLevel.SelectedIndexChanged += new System.EventHandler(cmbValue_SelectedIndexChanged);
            cmbValueArea.SelectedIndexChanged += new System.EventHandler(cmbValue_SelectedIndexChanged);
            cmbValueItem.SelectedIndexChanged += new System.EventHandler(cmbValue_SelectedIndexChanged);
            cmbValueQuest.SelectedIndexChanged += new System.EventHandler(cmbValue_SelectedIndexChanged);
            cmbValueSpecial.SelectedIndexChanged += new System.EventHandler(cmbValue_SelectedIndexChanged);

        }

        private void TxtName_TextChanged(object sender, System.EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            _name = txtBox.Text;
        }

        private void cmbValue_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Item selectedItem = (Item)comboBox.SelectedItem;
            _value = selectedItem.Value;
            if (_name == "")
            {
                txtName.Text = selectedItem.Name;
            }
        }
        private void cmbType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Item selectedItem = (Item)comboBox.SelectedItem;
            Item selectedValueItem = null;
            _type = selectedItem.Value;

            this.cmbValueCharLevel.Hide();
            this.cmbValueArea.Hide();
            this.cmbValueItem.Hide();
            this.cmbValueQuest.Hide();

            switch (_type)
            {
                case AutoSplit.TYPE_CHAR_LEVEL:
                    this.cmbValueCharLevel.Show();
                    selectedValueItem = (Item)cmbValueCharLevel.SelectedItem;
                    break;
                case AutoSplit.TYPE_AREA:
                    this.cmbValueArea.Show();
                    selectedValueItem = (Item)cmbValueArea.SelectedItem;
                    break;
                case AutoSplit.TYPE_ITEM:
                    this.cmbValueItem.Show();
                    selectedValueItem = (Item)cmbValueItem.SelectedItem;
                    break;
                case AutoSplit.TYPE_QUEST:
                    this.cmbValueQuest.Show();
                    selectedValueItem = (Item)cmbValueQuest.SelectedItem;
                    break;
            }
            if (selectedValueItem != null)
            {
                _value = selectedValueItem.Value;
            }
            else
            {
                _value = -1;
            }

        }
    }
}
