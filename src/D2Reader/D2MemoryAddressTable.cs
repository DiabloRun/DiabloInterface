using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zutatensuppe.D2Reader
{
    public class D2MemoryAddressTable
    {
        public IntPtr World;
        public IntPtr GameId;
        public IntPtr Area;
        public IntPtr ItemDescriptions;
        public IntPtr MagicModifierTable;
        public IntPtr RareModifierTable;

        // These all probably belong to a global struct the compiler optimized away.
        public IntPtr PlayerUnit;
        public IntPtr GlobalData;
        public IntPtr LowQualityItems;
        public IntPtr StringIndexerTable;
        public IntPtr StringAddressTable;
        public IntPtr PatchStringIndexerTable;
        public IntPtr PatchStringAddressTable;
        public IntPtr ExpansionStringIndexerTable;
        public IntPtr ExpansionStringAddressTable;
    }
}
