using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Item;

namespace Zutatensuppe.D2Reader.Readers
{
    public interface IInventoryReader
    {
        ItemReader ItemReader { get; }

        IEnumerable<D2Unit> EnumerateInventoryBackward(D2Unit unit, Func<D2ItemData, bool> filter);

        IEnumerable<D2Unit> EnumerateInventoryBackward(D2Unit unit);

        IEnumerable<D2Unit> EnumerateInventoryForward(D2Unit unit);
    }
}
