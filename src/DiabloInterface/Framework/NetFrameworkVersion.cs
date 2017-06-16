using System.ComponentModel;

namespace Zutatensuppe.DiabloInterface.Framework
{
    internal enum NetFrameworkVersion
    {
        Unknown,

        [Description("1.1")] Version_1_1,

        [Description("2.0")] Version_2_0,

        [Description("3.0")] Version_3_0,

        [Description("3.5")] Version_3_5,

        [Description("4.5")] Version_4_5,

        [Description("4.5.1")] Version_4_5_1,

        [Description("4.5.2")] Version_4_5_2,

        [Description("4.6")] Version_4_6,

        [Description("4.6.1")] Version_4_6_1,

        [Description("4.6.2")] Version_4_6_2,

        [Description("4.7")] Version_4_7,

        [Description("Unknown Version > 4.7")] Version_Future
    }
}
