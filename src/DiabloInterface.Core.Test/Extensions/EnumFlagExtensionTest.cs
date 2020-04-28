namespace DiabloInterface.Core.Extensions.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Zutatensuppe.DiabloInterface.Core.Extensions;

    [TestClass]
    public class EnumFlagExtensionTest
    {
        [Flags]
        enum DefaultFlagEnum
        {
            Flag1 = 1,
            Flag2 = 2,
            Flag3 = 4,
        }

        [Flags]
        enum ByteFlagEnum : byte
        {
            Flag1 = 1,
            Flag2 = 2,
            Flag3 = 4,
        }

        [Flags]
        enum LongFlagEnum : long
        {
            Flag1 = 1,
            Flag2 = 2,
            Flag3 = (1 << 40),
        }

        [TestMethod]
        public void CanSetEnumFlags()
        {
            var flags = DefaultFlagEnum.Flag1 | DefaultFlagEnum.Flag2;

            flags = flags.SetFlag(DefaultFlagEnum.Flag3);

            Assert.IsTrue(flags.HasFlag(DefaultFlagEnum.Flag1));
            Assert.IsTrue(flags.HasFlag(DefaultFlagEnum.Flag2));
            Assert.IsTrue(flags.HasFlag(DefaultFlagEnum.Flag3));
        }

        [TestMethod]
        public void CanClearEnumFlags()
        {
            var flags = DefaultFlagEnum.Flag1 | DefaultFlagEnum.Flag2;

            flags = flags.ClearFlag(DefaultFlagEnum.Flag1);

            Assert.IsFalse(flags.HasFlag(DefaultFlagEnum.Flag1));
            Assert.IsTrue(flags.HasFlag(DefaultFlagEnum.Flag2));
            Assert.IsFalse(flags.HasFlag(DefaultFlagEnum.Flag3));
        }

        [TestMethod]
        public void CanSetAndClearByteFlags()
        {
            var flags = ByteFlagEnum.Flag1 | ByteFlagEnum.Flag2;

            flags = flags.ClearFlag(ByteFlagEnum.Flag1);
            flags = flags.SetFlag(ByteFlagEnum.Flag3);

            Assert.IsFalse(flags.HasFlag(ByteFlagEnum.Flag1));
            Assert.IsTrue(flags.HasFlag(ByteFlagEnum.Flag2));
            Assert.IsTrue(flags.HasFlag(ByteFlagEnum.Flag3));
        }

        [TestMethod]
        public void CanSetAndClearLongFlags()
        {
            var flags = LongFlagEnum.Flag1 | LongFlagEnum.Flag2;

            flags = flags.ClearFlag(LongFlagEnum.Flag1);
            flags = flags.SetFlag(LongFlagEnum.Flag3);

            Assert.IsFalse(flags.HasFlag(LongFlagEnum.Flag1));
            Assert.IsTrue(flags.HasFlag(LongFlagEnum.Flag2));
            Assert.IsTrue(flags.HasFlag(LongFlagEnum.Flag3));
        }
    }
}
