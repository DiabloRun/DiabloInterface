using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zutatensuppe.D2Reader.Struct.Stat
{
    enum StatCalcType
    {
        END_OF_CALC = 0, // ends the calculation ?
        NEXT_VAL = 7, // if this is not the last item in the list, put the next StatCalc Byte on Bottom of stack, then continue with the next after that. 
                      // (a max num of times 40, all bytes.. but maybe the 40 is just a big enough arbitrary number and never reached.. )

    }
}
