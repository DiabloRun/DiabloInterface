using System;

namespace Zutatensuppe.DiabloInterface.Core
{
    public class Utility
    {
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return min;

            if (value.CompareTo(max) > 0)
                return max;

            return value;
        }
    }
}
