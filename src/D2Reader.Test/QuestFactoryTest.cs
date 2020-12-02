using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zutatensuppe.D2Reader;

namespace Zutatensuppe.DiabloInterface.D2Reader.Test
{
    [TestClass]
    public class QuestFactoryTest
    {
        [TestMethod]
        public void BufferLookup()
        {
            ushort[] questBuffer = new ushort[256];
            var quests = QuestFactory.CreateListFromBuffer(questBuffer);

            Assert.AreEqual(28, quests.Count);

            Assert.AreEqual("Den of Evil", quests[5].Name);
            Assert.AreEqual(false, quests[5].IsBossQuest);

            Assert.AreEqual("Radament's Lair", quests[10].Name);

            Assert.AreEqual("Siege on Harrogath", quests[22].Name);

            Assert.AreEqual("Cow King", quests[27].Name);

            Assert.AreEqual(28, QuestFactory.CreateListFromBuffer(new ushort[1]).Count);
        }
    }
}
