using System.Drawing;
using System.Windows.Forms;

namespace DiabloInterface
{
    public class AutoSplit
    {
        public enum Type
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

        private string _name = "";
        private Type _type = Type.None;
        private short _value = -1;
        private short _difficulty = 0;

        public short difficulty
        {
            get { return this._difficulty; }
            set { _difficulty = value; }
        }
        public Type type
        {
            get { return this._type; }
            set { _type = value; }
        }
        public short value
        {
            get { return _value; }
            set { _value = value; }
        }
        public string name
        {
            get { return _name; }
            set { _name = value; }
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
                this.control.ForeColor = Color.Green;
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

        public AutoSplit()
        {

        }
        public AutoSplit(string name, Type type, short value, short difficulty)
        {
            this._name = name;
            this._type = type;
            this._value = value;
            this._difficulty = difficulty;
        }
    }
}
