namespace Zutatensuppe.DiabloInterface.Core.Extensions
{
    using System;

    public static class EnumFlagExtension
    {
        public static T SetFlag<T>(this Enum value, T flag)
        {
            var baseEnumType = value.GetType();
            if (!IsFlagEnum(baseEnumType)) throw new ArgumentException("Enum must be marked with the Flag attribute.", nameof(value));
            if (!IsFlagEnum(typeof(T))) throw new ArgumentException("Flag must be marked with the Flag attribute.", nameof(flag));
            if (!(value is T)) throw new ArgumentException("Mismatched enum types.");

            var underlyingType = Enum.GetUnderlyingType(baseEnumType);
            dynamic collection = Convert.ChangeType(value, underlyingType);
            dynamic collectionAppend = Convert.ChangeType(flag, underlyingType);

            collection |= collectionAppend;
            return (T)Enum.ToObject(baseEnumType, collection);
        }

        public static T ClearFlag<T>(this Enum value, T flag)
        {
            var baseEnumType = value.GetType();
            if (!IsFlagEnum(baseEnumType)) throw new ArgumentException("Enum must be marked with the Flag attribute.", nameof(value));
            if (!IsFlagEnum(typeof(T))) throw new ArgumentException("Flag must be marked with the Flag attribute.", nameof(flag));
            if (!(value is T)) throw new ArgumentException("Mismatched enum types.");

            var underlyingType = Enum.GetUnderlyingType(baseEnumType);
            dynamic collection = Convert.ChangeType(value, underlyingType);
            dynamic collectionClear = Convert.ChangeType(flag, underlyingType);

            collection &= ~collectionClear;
            return (T)Enum.ToObject(baseEnumType, collection);
        }

        static bool IsFlagEnum(Type type)
        {
            return type.GetCustomAttributes(typeof(FlagsAttribute), true).Length > 0;
        }
    }
}
