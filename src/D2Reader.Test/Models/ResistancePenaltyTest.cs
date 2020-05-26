using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Struct;

namespace Zutatensuppe.DiabloInterface.D2Reader.Test.Models
{
    [TestClass]
    public class ResistancePenaltyTest
    {
        [TestMethod]
        public void TestGetPenaltyByGame()
        {
            D2Game g = new D2Game();

            // expansion
            g.LODFlag = 1;

            // normal
            g.Difficulty = 0;
            Assert.AreEqual(0, ResistancePenalty.GetPenaltyByGame(g));
            // nm
            g.Difficulty = 1;
            Assert.AreEqual(-40, ResistancePenalty.GetPenaltyByGame(g));
            // hell
            g.Difficulty = 2;
            Assert.AreEqual(-100, ResistancePenalty.GetPenaltyByGame(g));

            // classic 
            g.LODFlag = 0;

            // normal
            g.Difficulty = 0;
            Assert.AreEqual(0, ResistancePenalty.GetPenaltyByGame(g));
            // nm
            g.Difficulty = 1;
            Assert.AreEqual(-20, ResistancePenalty.GetPenaltyByGame(g));
            // hell
            g.Difficulty = 2;
            Assert.AreEqual(-50, ResistancePenalty.GetPenaltyByGame(g));
        }
    }
}
