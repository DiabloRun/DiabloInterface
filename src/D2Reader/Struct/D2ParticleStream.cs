using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zutatensuppe.D2Reader.Struct
{
    // struct is probably longer
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x08)]
    public class D2ParticleStream
    {
        [ExpectOffset(0x00)] public int __unknown00; // 
        [ExpectOffset(0x04)] public int __unknown04; //
    }
}
