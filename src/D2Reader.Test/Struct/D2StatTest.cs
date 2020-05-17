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

            stat.LoStatID = 140;
            Assert.AreEqual(false, stat.HasValidLoStatIdentifier());

            stat.LoStatID = (ushort)StatIdentifier.PoisonDivisor;
            Assert.AreEqual(true, stat.HasValidLoStatIdentifier());

            stat.LoStatID = (ushort)StatIdentifier.CannotBeFrozen;
            Assert.AreEqual(true, stat.HasValidLoStatIdentifier());

            stat.LoStatID = 300;
            Assert.AreEqual(false, stat.HasValidLoStatIdentifier());
        }
    }
}
