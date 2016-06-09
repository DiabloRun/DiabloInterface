using System;

namespace DiabloInterface.D2.Struct
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ExpectOffsetAttribute : Attribute
    {
        public uint Offset { get; private set; }

        public ExpectOffsetAttribute(uint offset)
        {
            Offset = offset;
        }
    }
}
