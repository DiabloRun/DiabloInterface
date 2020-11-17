using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.D2Reader.Struct;

namespace Zutatensuppe.D2Reader.Readers
{
    public interface IInventoryReader
    {
        IEnumerable<Item> Filter(IEnumerable<Item> enumerable, Func<Item, bool> filter);
        IEnumerable<Item> EnumerateInventoryBackward(D2Unit unit);
        IEnumerable<Item> EnumerateInventoryForward(D2Unit unit);

        List<Item> GetAllItems(D2Unit owner);
    }
}
