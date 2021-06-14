using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zutatensuppe.D2Reader.Struct.Stat;

namespace Zutatensuppe.DiabloInterface.D2Reader.Test.Struct
{
    [TestClass]
    public class D2StatTest
    {
        [TestMethod]
        public void TestValidLoStatIdentifier()
        {
            D2Stat stat = new D2Stat();
            stat.HiStatID = 140;
            stat.Value = 4999;

            stat.LoStatID = 0x140; // must be some value not defined in StatIdentifier
            Assert.AreEqual(false, stat.HasValidLoStatIdentifier());

            stat.LoStatID = (ushort)StatIdentifier.PoisonDivisor;
            Assert.AreEqual(true, stat.HasValidLoStatIdentifier());

            stat.LoStatID = (ushort)StatIdentifier.CannotBeFrozen;
            Assert.AreEqual(true, stat.HasValidLoStatIdentifier());

            stat.LoStatID = 0x300; // must be some value not defined in StatIdentifier
            Assert.AreEqual(false, stat.HasValidLoStatIdentifier());
        }
    }
}
