using System;

namespace Zutatensuppe.D2Reader.Struct
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
