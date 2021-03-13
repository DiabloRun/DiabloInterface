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
            // equal (null)
            List<int> listA = null;
            List<int> listB = null;
            Assert.AreEqual(true, DiffUtil.ListsEqual(listA, listB));

            // equal (values)
            listA = new List<int> { 1, 2, 3 };
            listB = new List<int> { 1, 2, 3 };
            Assert.AreEqual(true, DiffUtil.ListsEqual(listA, listB));

            // not equal (null)
            listA = new List<int> { 1, 2, 3 };
            listB = null;
            Assert.AreEqual(false, DiffUtil.ListsEqual(listA, listB));

            // not equal (values)
            listA = new List<int> { 1, 2, 3 };
            listB = new List<int> { 1, 2, 4 };
            Assert.AreEqual(false, DiffUtil.ListsEqual(listA, listB));

            // not equal (order)
            listA = new List<int> { 1, 2, 3 };
            listB = new List<int> { 1, 3, 2 };
            Assert.AreEqual(false, DiffUtil.ListsEqual(listA, listB));
        }

        [TestMethod]
        public void TestListsEqualObj()
        {
            var x = new MockType{ Some = 108, Thing = "Hello_X" };
            var y = new MockType{ Some = 108, Thing = "Hello_Y" };
            var z = new MockType{ Some = 108, Thing = "Hello_Z" };
            var z2 = new MockType{ Some = 108, Thing = "Hello_Z" };
            var a = new MockType{ Some = 108, Thing = "Hello_A" };

            // equal (null)
            List<object> listA = null;
            List<object> listB = null;
            Assert.AreEqual(true, DiffUtil.ListsEqual(listA, listB));

            // equal (values)
            listA = new List<object> { x, y, z };
            listB = new List<object> { x, y, z };
            Assert.AreEqual(true, DiffUtil.ListsEqual(listA, listB));

            // not equal (null)
            listA = new List<object> { x, y, z };
            listB = null;
            Assert.AreEqual(false, DiffUtil.ListsEqual(listA, listB));

            // not equal (values)
            listA = new List<object> { x, y, z };
            listB = new List<object> { x, y, a };
            Assert.AreEqual(false, DiffUtil.ListsEqual(listA, listB));

            // not equal (order)
            listA = new List<object> { x, y, z };
            listB = new List<object> { x, z, y };
            Assert.AreEqual(false, DiffUtil.ListsEqual(listA, listB));

            // not equal (same value, but not same value reference)
            listA = new List<object> { x, y, z };
            listB = new List<object> { x, y, z2 };
            Assert.AreEqual(false, DiffUtil.ListsEqual(listA, listB));
        }

        [TestMethod]
        public void TestObjectsEqual()
        {
            // TODO: implement
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
