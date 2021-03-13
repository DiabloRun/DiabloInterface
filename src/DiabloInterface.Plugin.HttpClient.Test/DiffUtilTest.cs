using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DiabloInterface.Plugin.HttpClient.Test
{
    [TestClass]
    public class DiffUtilTest
    {
        [TestMethod]
        public void TestListsEqualSimple()
        {
            Assert.AreEqual(true, DiffUtil.ListsEqual(
                (List<int>)null,
                null
            ), "equal (both null)");

            Assert.AreEqual(true, DiffUtil.ListsEqual(
                new List<int> { 1, 2, 3 },
                new List<int> { 1, 2, 3 }
            ), "equal (values)");

            Assert.AreEqual(false, DiffUtil.ListsEqual(
                new List<int> { 1, 2, 3 },
                null
            ), "not equal (second null)");

            Assert.AreEqual(false, DiffUtil.ListsEqual(
                null,
                new List<int> { 1, 2, 3 }
            ), "not equal (first null)");

            Assert.AreEqual(false, DiffUtil.ListsEqual(
                new List<int> { 1, 2, 3 },
                new List<int> { 1, 2, 4 }
            ), "not equal (values)");

            Assert.AreEqual(false, DiffUtil.ListsEqual(
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

            Assert.AreEqual(true, DiffUtil.ListsEqual(
                (List<object>)null,
                null
            ), "equal (null)");

            Assert.AreEqual(true, DiffUtil.ListsEqual(
                new List<object> { x, y, z },
                new List<object> { x, y, z }
            ), "equal (values)");

            Assert.AreEqual(false, DiffUtil.ListsEqual(
                null,
                new List<object> { x, y, z }
            ), "not equal (first null)");

            Assert.AreEqual(false, DiffUtil.ListsEqual(
                new List<object> { x, y, z },
                null
            ), "not equal (second null)");

            Assert.AreEqual(false, DiffUtil.ListsEqual(
                new List<object> { x, y, z },
                new List<object> { x, y, a }
            ), "not equal (values)");

            Assert.AreEqual(false, DiffUtil.ListsEqual(
                new List<object> { x, y, z },
                new List<object> { x, z, y }
            ), "not equal (order)");

            Assert.AreEqual(false, DiffUtil.ListsEqual(
                new List<object> { x, y, z },
                new List<object> { x, y, z2 }
            ), "not equal (same value, but not same reference)");
        }

        [TestMethod]
        public void TestObjectsEqual()
        {
            Assert.AreEqual(false, DiffUtil.ObjectsEqual(
                null,
                new MockType { Some = 108, Thing = "Hello_Z" }
            ), "first is null");

            Assert.AreEqual(false, DiffUtil.ObjectsEqual(
                new MockType { Some = 108, Thing = "Hello_Z" },
                null
            ), "second is null");

            Assert.AreEqual(true, DiffUtil.ObjectsEqual(
                null,
                null
            ), "both null");

            Assert.AreEqual(false, DiffUtil.ObjectsEqual(
                new MockType { Some = 108, Thing = "Hello_Z" },
                new MockType { Some = 108, Thing = "Hello_Z" }
            ), "same value, but not same ref");

            var obj = new MockType { Some = 108, Thing = "Hello_Z" };
            Assert.AreEqual(true, DiffUtil.ObjectsEqual(
                obj,
                obj
            ), "same value and same ref");

            Assert.AreEqual(true, DiffUtil.ObjectsEqual(
                new { Some = 108, Thing = "Hello_Z" },
                new { Some = 108, Thing = "Hello_Z" }
            ), "same value and (anonymous obj that doesnt use ref)");
        }

        [TestMethod]
        public void TestItemsDiff()
        {
            // TODO: implement
        }

        [TestMethod]
        public void TestCompletedQuestsDiff()
        {
            // TODO: implement
        }
    }

    class MockType
    {
        public int Some;
        public string Thing;
    }
}
