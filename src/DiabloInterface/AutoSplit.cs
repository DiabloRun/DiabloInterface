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

        private short _difficulty = 0;
        public short Difficulty
        {
            get { return _difficulty; }
            set { _difficulty = value; }
        }

        private SplitType _type = SplitType.None;
        public SplitType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private short _value = -1;
        public short Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private bool _reached = false;
        public bool Reached
        {
            get { return _reached; }
            set { _reached = value; updateControl(); }
        }

        private bool _deleted = false;
        public bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
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

            if (_deleted)
            {
                control.Parent.Controls.Remove(control);
                control = null;
            }
        }
        public void bindControl(Control control)
        {
            this.control = control;
            updateControl();
        }
        
        public AutoSplit()
        {

        }

        public AutoSplit(string name, SplitType type, short value, short difficulty)
        {
            _name = name;
            _type = type;
            _value = value;
            _difficulty = difficulty;
        }
    }
}
