using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zutatensuppe.D2Reader;
using Zutatensuppe.D2Reader.Models;

namespace Zutatensuppe.DiabloInterface.D2Reader.Test
{
    [TestClass]
    public class QuestFactoryTest
    {
        [TestMethod]
        public void BufferLookup()
        {
            Quest q = QuestFactory.CreateFromBufferIndex(1, 0);
            Assert.AreEqual(q.Name, "Den of Evil");
            Assert.AreEqual(q.IsBossQuest, false);

            q = QuestFactory.CreateFromBufferIndex(9, 0);
            Assert.AreEqual(q.Name, "Radament's Lair");

            q = QuestFactory.CreateFromBufferIndex(29, 0);
            Assert.AreEqual(q, null);

            q = QuestFactory.CreateFromBufferIndex(35, 0);
            Assert.AreEqual(q.Name, "Siege on Harrogath");
        }
    }
}
