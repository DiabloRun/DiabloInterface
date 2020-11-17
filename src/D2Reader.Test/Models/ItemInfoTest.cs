using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Struct.Item;

namespace Zutatensuppe.DiabloInterface.D2Reader.Test.Models
{
    [TestClass]
    public class ItemInfoTest
    {
        [TestMethod]
        public void TestAreEqual()
        {
            Assert.AreEqual(true, ItemInfo.AreEqual(null, null));

            var itemA = new ItemInfo
            {
                Class = 1,
                ItemName = "somename",
                ItemBaseName = "somebasename",
                QualityColor = "somecolor",
                Location = new ItemLocation {
                    BodyLocation = BodyLocation.Gloves
                },
                Properties = new List<string> { "hello", "world" },
            };

            Assert.AreEqual(false, ItemInfo.AreEqual(itemA, null));

            Assert.AreEqual(false, ItemInfo.AreEqual(null, itemA));

            var itemB = new ItemInfo
            {
                Class = 1,
                ItemName = "somename",
                ItemBaseName = "somebasename",
                QualityColor = "somecolor",
                Location = new ItemLocation {
                    BodyLocation = BodyLocation.Gloves
                },
                Properties = new List<string> { "hello", "world" },
            };

            Assert.AreEqual(true, ItemInfo.AreEqual(itemA, itemB));

            itemB.Class = 2;
            Assert.AreEqual(false, ItemInfo.AreEqual(itemA, itemB));
            itemB.Class = 1;
            Assert.AreEqual(true, ItemInfo.AreEqual(itemA, itemB));

            itemB.Properties = new List<string> { "world", "hello" };
            Assert.AreEqual(false, ItemInfo.AreEqual(itemA, itemB));
            itemB.Properties = new List<string> { "hello", "world" };
            Assert.AreEqual(true, ItemInfo.AreEqual(itemA, itemB));
        }
    }
}
