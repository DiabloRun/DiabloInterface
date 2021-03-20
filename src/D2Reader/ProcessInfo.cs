using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool Equals(ProcessInfo other)
        {
            return other != null
                && ProcessName == other.ProcessName
                && ModuleName == other.ModuleName
                && BaseAddress == other.BaseAddress
                && FileVersion == other.FileVersion
                && Enumerable.SequenceEqual(CommandLineArgs, other.CommandLineArgs)
                && Enumerable.SequenceEqual(ModuleBaseAddresses, other.ModuleBaseAddresses);
        }

        public string ReadableType()
        {
            if (ModuleBaseAddresses.ContainsKey("projectdiablo.dll"))
            {
               return "PD2";
            }
            return "D2";
        }

        public string ReadableVersion()
        {
            switch (FileVersion)
            {
                case "1.14d": 
                case "1.14.3.71":
                    return "1.14d";

                case "1.14c":
                case "1.14.2.70":
                    return "1.14c";

                case "1.14b":
                case "1.14.1.68":
                    return "1.14b";

                case "1.13d":
                case "1, 0, 13, 64":
                    return "1.13d";

                case "1.13c":
                case "v 1.13c":
                case "1, 0, 13, 60":
                case "1, 0, 0, 0":
                    return "1.13c";

                default:
                    return "unknown";
            }
        }
    }
}
