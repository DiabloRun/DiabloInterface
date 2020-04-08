using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Item;

namespace Zutatensuppe.D2Reader.Readers
{
    public interface IInventoryReader
    {
        IEnumerable<Item> Filter(IEnumerable<Item> enumerable, Func<Item, bool> filter);
        IEnumerable<Item> EnumerateInventoryBackward(D2Unit unit);
        IEnumerable<Item> EnumerateInventoryForward(D2Unit unit);
    }
}
