using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiabloInterface.D2.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct D2PlayerData
    {
        #region structure (sizeof = 0x10)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string szPlayerName;     // 0x00
        #endregion
    }
}
