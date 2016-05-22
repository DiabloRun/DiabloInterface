using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabloInterface
{
    class ByteReader
    {
        public static int getIntVal(byte[] bytes, int i) {
            return (BitConverter.ToInt32(new[] { bytes[i], bytes[i + 1], bytes[i + 2], bytes[i + 3] }, 0));
        }
        public static int getShortVal(byte[] bytes, int i)
        {
            return (BitConverter.ToInt16(new[] { bytes[i], bytes[i + 1] }, 0));
        }
        public static int getByteVal(byte[] bytes, int i)
        {
            return (int)bytes[i];
        }
    }
}
