using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader.Models;

namespace DiabloInterface.Plugin.HttpClient.Test
{
    [TestClass]
    public class DiffUtilTest
    {
        [TestMethod]
        public void TestListsEqualSimple()
        {
            Assert.IsTrue(DiffUtil.ListsEqual(
                (List<int>)null,
                null
            ), "equal (both null)");

            Assert.IsTrue(DiffUtil.ListsEqual(
                new List<int> { 1, 2, 3 },
                new List<int> { 1, 2, 3 }
            ), "equal (values)");

            Assert.IsFalse(DiffUtil.ListsEqual(
                new List<int> { 1, 2, 3 },
                null
            ), "not equal (second null)");

            Assert.IsFalse(DiffUtil.ListsEqual(
                null,
                new List<int> { 1, 2, 3 }
            ), "not equal (first null)");

            Assert.IsFalse(DiffUtil.ListsEqual(
                new List<int> { 1, 2, 3 },
                new List<int> { 1, 2, 4 }
            ), "not equal (values)");

            Assert.IsFalse(DiffUtil.ListsEqual(
                new List<int> { 1, 2, 3 },
                new List<int> { 1, 3, 2 }
            ), "not equal (order)");
        }

        [TestMethod]
        public void TestListsEqualObj()
        {
            var x = new MockType{ Some = 108, Thing = "Hello_X" };
            var y = new MockType{ Some = 108, Thing = "Hello_Y" };
            var z = new MockType{ Some = 108, Thing = "Hello_Z" };
            var z2 = new MockType{ Some = 108, Thing = "Hello_Z" };
            var a = new MockType{ Some = 108, Thing = "Hello_A" };

            Assert.IsTrue(DiffUtil.ListsEqual(
                (List<object>)null,
                null
            ), "equal (null)");

            Assert.IsTrue(DiffUtil.ListsEqual(
                new List<object> { x, y, z },
                new List<object> { x, y, z }
            ), "equal (values)");

            Assert.IsFalse(DiffUtil.ListsEqual(
                null,
                new List<object> { x, y, z }
            ), "not equal (first null)");

            Assert.IsFalse(DiffUtil.ListsEqual(
                new List<object> { x, y, z },
                null
            ), "not equal (second null)");

            Assert.IsFalse(DiffUtil.ListsEqual(
                new List<object> { x, y, z },
                new List<object> { x, y, a }
            ), "not equal (values)");

            Assert.IsFalse(DiffUtil.ListsEqual(
                new List<object> { x, y, z },
                new List<object> { x, z, y }
            ), "not equal (order)");

            Assert.IsFalse(DiffUtil.ListsEqual(
                new List<object> { x, y, z },
                new List<object> { x, y, z2 }
            ), "not equal (same value, but not same reference)");
        }

        [TestMethod]
        public void TestObjectsEqual()
        {
            Assert.IsFalse(DiffUtil.ObjectsEqual(
                null,
                new MockType { Some = 108, Thing = "Hello_Z" }
            ), "first is null");

            Assert.IsFalse(DiffUtil.ObjectsEqual(
                new MockType { Some = 108, Thing = "Hello_Z" },
                null
            ), "second is null");

            Assert.IsTrue(DiffUtil.ObjectsEqual(
                null,
                null
            ), "both null");

            Assert.IsFalse(DiffUtil.ObjectsEqual(
                new MockType { Some = 108, Thing = "Hello_Z" },
                new MockType { Some = 108, Thing = "Hello_Z" }
            ), "same value, but not same ref");

            var obj = new MockType { Some = 108, Thing = "Hello_Z" };
            Assert.IsTrue(DiffUtil.ObjectsEqual(
                obj,
                obj
            ), "same value and same ref");

            Assert.IsTrue(DiffUtil.ObjectsEqual(
                new { Some = 108, Thing = "Hello_Z" },
                new { Some = 108, Thing = "Hello_Z" }
            ), "same value and (anonymous obj that doesnt use ref)");
        }

        [TestMethod]
        public void TestItemsDiff()
        {
            List<ItemInfo> prev = null;
            List<ItemInfo> curr = null;
            Tuple<List<ItemInfo>, List<ItemInfo>> res = null;

            // nothing old, nothing new (null version)
            res = DiffUtil.ItemsDiff(curr, prev);
            Assert.IsNull(res.Item1);
            Assert.IsNull(res.Item2);

            // nothing old, nothing new (empty list version)
            prev = new List<ItemInfo>();
            curr = new List<ItemInfo>();
            res = DiffUtil.ItemsDiff(curr, prev);
            Assert.IsNull(res.Item1);
            Assert.IsNull(res.Item2);

            // nothing old, one new
            // -> nothing removed, one added
            curr = new List<ItemInfo>
            {
                new ItemInfo{GUID = 1, Class = 1},
            };
            res = DiffUtil.ItemsDiff(curr, prev);
            Assert.IsNull(res.Item2);
            CollectionAssert.AreEquivalent(curr, res.Item1);

            // one old, one new
            // -> nothing removed, one added
            prev = new List<ItemInfo>
            {
                new ItemInfo{GUID = 1, Class = 1},
            };
            curr = new List<ItemInfo>
            {
                new ItemInfo{GUID = 1, Class = 1},
                new ItemInfo{GUID = 2, Class = 2},
            };
            res = DiffUtil.ItemsDiff(curr, prev);
            Assert.IsNull(res.Item2);
            Assert.AreEqual(1, res.Item1.Count);
            Assert.IsTrue(ItemInfo.AreEqual(curr[1], res.Item1[0]));

            // one old, one new
            // -> one removed, one added
            prev = new List<ItemInfo>
            {
                new ItemInfo{GUID = 1, Class = 1},
            };
            curr = new List<ItemInfo>
            {
                new ItemInfo{GUID = 2, Class = 2},
            };
            res = DiffUtil.ItemsDiff(curr, prev);
            CollectionAssert.AreEquivalent(curr, res.Item1);
            CollectionAssert.AreEquivalent(prev, res.Item2);

            // one old, nothing new
            // -> one removed, nothing added
            prev = new List<ItemInfo>
            {
                new ItemInfo{GUID = 1, Class = 1},
            };
            curr = new List<ItemInfo>
            {
            };
            res = DiffUtil.ItemsDiff(curr, prev);
            Assert.IsNull(res.Item1);
            CollectionAssert.AreEquivalent(prev, res.Item2);
        }

        [TestMethod]
        public void TestCompletedQuestsDiff()
        {
            // Both null
            Assert.IsNull(DiffUtil.CompletedQuestsDiff(null, null));

            // One null (first)
            var list = new Dictionary<GameDifficulty, List<QuestId>>
            {
                { GameDifficulty.Hell, new List<QuestId> { QuestId.Andariel } },
            };
            Assert.AreEqual(list, DiffUtil.CompletedQuestsDiff(list, null));

            // One null (second)
            Assert.IsNull(DiffUtil.CompletedQuestsDiff(null, list));


            // new quests solved for each difficulty
            var prev = new Dictionary<GameDifficulty, List<QuestId>>
            {
                { GameDifficulty.Normal, new List<QuestId> { QuestId.Andariel } },
                { GameDifficulty.Nightmare, new List<QuestId> { QuestId.Andariel } },
                { GameDifficulty.Hell, new List<QuestId> { QuestId.Andariel } },
            };
            var curr = new Dictionary<GameDifficulty, List<QuestId>>
            {
                { GameDifficulty.Normal, new List<QuestId> { QuestId.Andariel, QuestId.Duriel } },
                { GameDifficulty.Nightmare, new List<QuestId> { QuestId.Andariel, QuestId.Mephisto } },
                { GameDifficulty.Hell, new List<QuestId> { QuestId.Andariel, QuestId.Diablo } },
            };
            var expected = new Dictionary<GameDifficulty, List<QuestId>>
            {
                { GameDifficulty.Normal, new List<QuestId> { QuestId.Duriel } },
                { GameDifficulty.Nightmare, new List<QuestId> { QuestId.Mephisto } },
                { GameDifficulty.Hell, new List<QuestId> { QuestId.Diablo } },
            };
            var actual = DiffUtil.CompletedQuestsDiff(curr, prev);
            CollectionAssert.AreEquivalent(
                expected[GameDifficulty.Normal],
                actual[GameDifficulty.Normal]
            );
            CollectionAssert.AreEquivalent(
                expected[GameDifficulty.Nightmare],
                actual[GameDifficulty.Nightmare]
            );
            CollectionAssert.AreEquivalent(
                expected[GameDifficulty.Hell],
                actual[GameDifficulty.Hell]
            );
        }
    }

    class MockType
    {
        public int Some;
        public string Thing;
    }
}
