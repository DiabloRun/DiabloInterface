using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Zutatensuppe.DiabloInterface.Plugin.Updater;

namespace DiabloInterface.Plugin.Updater.Test
{
    [TestClass]
    public class VersionCheckerTest
    {
        [TestMethod]
        public void TestTryParseVersionString()
        {
            string versionString;

            Version expected;
            Version actual;

            versionString = null;
            expected = null;
            actual = VersionChecker.TryParseVersionString(versionString);
            Assert.AreEqual(expected, actual);

            versionString = "p123";
            expected = null;
            actual = VersionChecker.TryParseVersionString(versionString);
            Assert.AreEqual(expected, actual);

            versionString = "123";
            expected = null;
            actual = VersionChecker.TryParseVersionString(versionString);
            Assert.AreEqual(expected, actual);

            versionString = "123.3";
            expected = new Version(123, 3);
            actual = VersionChecker.TryParseVersionString(versionString);
            Assert.AreEqual(expected, actual);

            versionString = "p123.3";
            expected = null;
            actual = VersionChecker.TryParseVersionString(versionString);
            Assert.AreEqual(expected, actual);

            versionString = "21.3.13";
            expected = new Version(21, 3, 13);
            actual = VersionChecker.TryParseVersionString(versionString);
            Assert.AreEqual(expected, actual);

            versionString = "21.3.13.1123";
            expected = new Version(21, 3, 13, 1123);
            actual = VersionChecker.TryParseVersionString(versionString);
            Assert.AreEqual(expected, actual);

            string versionUrl;
            string regexPrefix = ".*/releases/tag/v";

            versionUrl = null;
            expected = null;
            actual = VersionChecker.TryParseVersionString(versionUrl, regexPrefix);
            Assert.AreEqual(expected, actual);

            versionUrl = "123";
            expected = null;
            actual = VersionChecker.TryParseVersionString(versionUrl, regexPrefix);
            Assert.AreEqual(expected, actual);

            versionUrl = "v123";
            expected = null;
            actual = VersionChecker.TryParseVersionString(versionUrl, regexPrefix);
            Assert.AreEqual(expected, actual);

            versionUrl = "releases/tag/v123";
            expected = null;
            actual = VersionChecker.TryParseVersionString(versionUrl, regexPrefix);
            Assert.AreEqual(expected, actual);

            versionUrl = "/releases/tag/v123";
            expected = null;
            actual = VersionChecker.TryParseVersionString(versionUrl, regexPrefix);
            Assert.AreEqual(expected, actual);

            versionUrl = "helloworld/releases/tag/v123.2";
            expected = new Version(123, 2);
            actual = VersionChecker.TryParseVersionString(versionUrl, regexPrefix);
            Assert.AreEqual(expected, actual);

            versionUrl = "/releases/tag/v123.3";
            expected = new Version(123, 3);
            actual = VersionChecker.TryParseVersionString(versionUrl, regexPrefix);
            Assert.AreEqual(expected, actual);

            versionUrl = "/releases/tag/v21.3.13";
            expected = new Version(21, 3, 13);
            actual = VersionChecker.TryParseVersionString(versionUrl, regexPrefix);
            Assert.AreEqual(expected, actual);

            versionUrl = "/releases/tag/v21.3.13.1123";
            expected = new Version(21, 3, 13, 1123);
            actual = VersionChecker.TryParseVersionString(versionUrl, regexPrefix);
            Assert.AreEqual(expected, actual);
        }
    }
}
