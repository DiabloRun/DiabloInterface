using System.Drawing;
using System.Windows.Forms;

namespace DiabloInterface
{
    public class AutoSplit
    {
        public enum SplitType
        {
            None = -1,
            CharLevel = 0,
            Area = 1,
            Item = 2,
            Quest = 3,
            Special = 4,
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

        private Control control;

        public short Difficulty { get; set; }
        public SplitType Type { get; set; }
        public short Value { get; set; }
        public string Name { get; set; }

        private bool _reached = false;
        public bool Reached
        {
            get { return _reached; }
            set { _reached = value; updateControl(); }
        }

        public void updateControl()
        {
            if (control == null)
            {
                return;
            }

            control.Text = Name;

            if (_reached)
            {
                control.ForeColor = Color.Green;
            }
            else
            {
                control.ForeColor = Color.Red;
            }
        }

        public void bindControl(Control control)
        {
            this.control = control;
            updateControl();
        }

        public AutoSplit()
        {
            Name = "Unnamed";
            Type = SplitType.None;
            Value = -1;
            Difficulty = 0;
        }

        public AutoSplit(AutoSplit other)
        {
            Name = other.Name;
            Type = other.Type;
            Value = other.Value;
            Difficulty = other.Difficulty;
        }

        public AutoSplit(string name, SplitType type, short value, short difficulty)
        {
            Name = name;
            Type = type;
            Value = value;
            Difficulty = difficulty;
        }
    }
}
