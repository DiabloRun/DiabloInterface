using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader.Models;

namespace DiabloInterface.Plugin.HttpClient.Test
{
    [TestClass]
    public class HirelingDiffTest
    {
        [TestMethod]
        public void TestGetDiff()
        {
            HirelingDiff prev;
            HirelingDiff curr;
            HirelingDiff expected;
            HirelingDiff actual;

            // null
            prev = null;
            curr = null;
            Assert.IsNull(HirelingDiff.GetDiff(curr, prev));

            // prev is null
            prev = null;
            curr = new HirelingDiff();
            Assert.AreEqual(curr, HirelingDiff.GetDiff(curr, prev));

            // curr is null
            prev = new HirelingDiff();
            curr = null;
            Assert.AreEqual(curr, HirelingDiff.GetDiff(curr, prev));

            // has some auto property diff
            prev = new HirelingDiff { Level = 3 };
            curr = new HirelingDiff { Level = 10 };
            expected = new HirelingDiff { Level = 10 };
            actual = HirelingDiff.GetDiff(curr, prev);
            Assert.AreEqual(expected.Level, actual.Level);

            // has some item diff
            var item1 = new ItemInfo { GUID = 1, Class = 1 };
            var item2 = new ItemInfo { GUID = 2, Class = 2 };
            prev = new HirelingDiff { Items = new List<ItemInfo> { item1 } };
            curr = new HirelingDiff { Items = new List<ItemInfo> { item2 } };
            expected = new HirelingDiff {
                RemovedItems = new List<ItemInfo> { item1 },
                AddedItems = new List<ItemInfo> { item2 },
            };
            actual = HirelingDiff.GetDiff(curr, prev);
            Assert.AreEqual(expected.Items, actual.Items);
            CollectionAssert.AreEqual(expected.RemovedItems, actual.RemovedItems);
            CollectionAssert.AreEqual(expected.AddedItems, actual.AddedItems);

            // has some new skills
            prev = new HirelingDiff { Skills = new List<SkillInfo> {
                new SkillInfo { Id = 1 },
                new SkillInfo { Id = 2 }
            } };
            curr = new HirelingDiff { Skills = new List<SkillInfo> {
                new SkillInfo { Id = 1 },
                new SkillInfo { Id = 2 },
                new SkillInfo { Id = 3 }
            } };
            expected = new HirelingDiff { Skills = new List<SkillInfo> {
                new SkillInfo { Id = 1 },
                new SkillInfo { Id = 2 },
                new SkillInfo { Id = 3 }
            } };
            actual = HirelingDiff.GetDiff(curr, prev);
            CollectionAssert.AreEqual(expected.Skills, actual.Skills);

            // has lost some skills
            prev = new HirelingDiff { Skills = new List<SkillInfo> {
                new SkillInfo { Id = 1 },
                new SkillInfo { Id = 2 },
                new SkillInfo { Id = 3 }
            } };
            curr = new HirelingDiff { Skills = new List<SkillInfo> {
                new SkillInfo { Id = 1 },
                new SkillInfo { Id = 2 }
            } };
            expected = new HirelingDiff { Skills = new List<SkillInfo> {
                new SkillInfo { Id = 1 },
                new SkillInfo { Id = 2 }
            } };
            actual = HirelingDiff.GetDiff(curr, prev);
            CollectionAssert.AreEqual(expected.Skills, actual.Skills);

            // no change
            prev = new HirelingDiff
            {
                Level = 1,
                Items = new List<ItemInfo> { new ItemInfo { GUID = 1, Class = 1 } },
                Skills = new List<SkillInfo> {
                    new SkillInfo { Id = 1 },
                    new SkillInfo { Id = 2 },
                    new SkillInfo { Id = 3 }
                },
            };
            curr = new HirelingDiff
            {
                Level = 1,
                Items = new List<ItemInfo> { new ItemInfo { GUID = 1, Class = 1 } },
                Skills = new List<SkillInfo> {
                    new SkillInfo { Id = 1 },
                    new SkillInfo { Id = 2 },
                    new SkillInfo { Id = 3 }
                },
            };
            actual = HirelingDiff.GetDiff(curr, prev);
            Assert.IsNull(actual);
        }
    }
}
