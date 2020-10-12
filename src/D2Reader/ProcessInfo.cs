using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zutatensuppe.D2Reader
{
    public class ProcessInfo
    {
        public string ProcessName { get; internal set; }
        public string ModuleName { get; internal set; }
        public IntPtr BaseAddress { get; internal set; }
        public Dictionary<string, IntPtr> ModuleBaseAddresses { get; internal set; }
        public string FileVersion { get; internal set; }
        public string[] CommandLineArgs { get; internal set; }
    }
}
