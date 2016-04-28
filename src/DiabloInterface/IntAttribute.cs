namespace DiabloInterface
{
    public class IntAttribute
    {
        int _pointer;
        bool _pointerRelative;
        int[] _offsets;
        int _bytes;
        int _value;
        public IntAttribute(int pointer, int[] offsets, int bytes, bool pointerRelative)
        {
            this._pointer = pointer;
            this._offsets = offsets;
            this._bytes = bytes;
            this._pointerRelative = pointerRelative;
        }

        public int pointer
        {
            get { return _pointer; }
        }
        public int value
        {
            get { return _value; }
            set { _value = value; }
        }
        public bool pointerRelative
        {
            get { return _pointerRelative; }
        }
        public int[] offsets
        {
            get { return _offsets; }
        }
        public int bytes
        {
            get { return _bytes; }
        }
    }
}
