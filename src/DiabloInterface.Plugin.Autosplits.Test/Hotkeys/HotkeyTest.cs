using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zutatensuppe.DiabloInterface.Plugin.Autosplits.Hotkeys.Test
{
    [TestClass]
    public class HotkeyTest
    {
        [TestMethod]
        public void TestKeysToStringConvertion()
        {
            void test(Keys k, string s, string toString = null)
            {
                Assert.AreEqual(k, new Hotkey(s).ToKeys());
                Assert.AreEqual(toString ?? s, new Hotkey(k).ToString());
            }

            test(Keys.Control | Keys.Shift | Keys.Alt | Keys.G, "Ctrl+Shift+Alt+G");
            test(Keys.Control | Keys.Shift | Keys.Alt | Keys.G, "Shift+Alt+G+Ctrl", "Ctrl+Shift+Alt+G");
            test(Keys.Shift | Keys.Alt | Keys.G, "Shift+Alt+G");
            test(Keys.Alt | Keys.G, "Alt+G");
            test(Keys.G, "G");
            test(Keys.F12, "F12");
            test(Keys.NumPad3, "NumPad3");
            test(Keys.None, "");
        }
    }
}
