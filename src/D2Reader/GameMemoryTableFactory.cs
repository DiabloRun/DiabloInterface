using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Zutatensuppe.DiabloInterface.Core.Logging;

namespace Zutatensuppe.D2Reader
{
    public interface IGameMemoryTableFactory
    {
        GameMemoryTable CreateForVersion(string versionString, IntPtr BaseAddress, Dictionary<string, IntPtr> moduleBaseAddresses);
        GameMemoryTable CreateForReader(IProcessMemoryReader reader);
    }

    public class GameVersionUnsupportedException : Exception
    {
        public string GameVersion;
        public GameVersionUnsupportedException(string gameVersion) :
            base(string.Format("Failed to create memory table for game version {0}", gameVersion))
        {
            GameVersion = gameVersion;
        }
    }

    public class ModuleNotLoadedException : Exception
    {
        public string ModuleName;
        public ModuleNotLoadedException(string moduleName) :
            base(string.Format("Failed to create memory table, module not loaded {0}", moduleName))
        {
            ModuleName = moduleName;
        }
    }

    public class GameMemoryTableFactory : IGameMemoryTableFactory
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public GameMemoryTable CreateForVersion(string gameVersion, IntPtr BaseAddress, Dictionary<string, IntPtr> moduleBaseAddresses)
        {
            // Refer to the wiki at https://github.com/Zutatensuppe/DiabloInterface/wiki/Finding-memory-table-addresses
            // for information about how to find addresses for a different version of the game.
            switch (gameVersion)
            {
                case "1.14d":
                case "1.14.3.71":
                    return CreateMemoryTableForVersion114D(BaseAddress);
                case "1.14c":
                case "1.14.2.70":
                    return CreateMemoryTableForVersion114C(BaseAddress);
                case "1.14b":
                case "1.14.1.68":
                    return CreateMemoryTableForVersion114B(BaseAddress);
                case "1.13d":
                case "1, 0, 13, 64":
                    return CreateMemoryTableForVersion113D(moduleBaseAddresses);
                case "1.13c":
                case "v 1.13c":
                case "1, 0, 13, 60":
                case "1, 0, 0, 0": // D2Loader-high.exe
                    return CreateMemoryTableForVersion113C(moduleBaseAddresses);
                default:
                    throw new GameVersionUnsupportedException(gameVersion);
            }
        }

        private GameMemoryTable CreateMemoryTableForVersion114D(IntPtr BaseAddress)
        {
            return new GameMemoryTable()
            {
                Loading = BaseAddress + 0x30F2C0,
                Saving = BaseAddress + 0x3792F8,
                Saving2 = BaseAddress + 0x3786D0,
                InGame = BaseAddress + 0x30EE8C,
                InMenu = BaseAddress + 0x379970,

                GlobalData = BaseAddress + 0x00344304, // game.744304
                World = BaseAddress + 0x00483D38,
                PlayersX = BaseAddress + 0x483D70,
                GameId = BaseAddress + 0x00482D0C,
                LowQualityItems = BaseAddress + 0x56CC58,
                ItemDescriptions = BaseAddress + 0x56CA58,
                MagicModifierTable = BaseAddress + 0x56CA7C,
                RareModifierTable = BaseAddress + 0x56CAA0,
                Units113 = null,
                Units114 = BaseAddress + 0x003A5E70,
                PlayerUnit = BaseAddress + 0x003A5E74,
                Area = BaseAddress + 0x003A3140,
                Pets = BaseAddress + 0x003BB5BC,
                InventoryTab = BaseAddress + 0x3BCC4C,
                StringIndexerTable = BaseAddress + 0x4829B4,
                StringAddressTable = BaseAddress + 0x4829B8,
                PatchStringIndexerTable = BaseAddress + 0x4829D0,
                PatchStringAddressTable = BaseAddress + 0x4829BC,
                ExpansionStringIndexerTable = BaseAddress + 0x4829D4,
                ExpansionStringAddressTable = BaseAddress + 0x4829C0,
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion114C(IntPtr BaseAddress)
        {
            return new GameMemoryTable()
            {
                Loading = BaseAddress + 0x30DF7C,
                Saving = BaseAddress + 0x36F760,
                Saving2 = BaseAddress + 0x370380,
                InGame = BaseAddress + 0x30DBC4,
                InMenu = BaseAddress + 0x478884,

                GlobalData = BaseAddress + 0x33FD78,
                World = BaseAddress + 0x0047ACC0,
                PlayersX = BaseAddress + 0x47ACF8,
                GameId = BaseAddress + 0x00479C94,
                LowQualityItems = BaseAddress + 0x563BE0,
                ItemDescriptions = BaseAddress + 0x5639E0,
                MagicModifierTable = BaseAddress + 0x563A04,
                RareModifierTable = BaseAddress + 0x563A28,
                Units113 = null,
                Units114 = BaseAddress + 0x0039CEF8,
                PlayerUnit = BaseAddress + 0x0039CEFC,
                Area = BaseAddress + 0x0039A1C8,
                Pets = BaseAddress + 0x003B2644,
                InventoryTab = BaseAddress + 0x003B3CD4,
                StringIndexerTable = BaseAddress + 0x479A3C,
                StringAddressTable = BaseAddress + 0x479A40,
                PatchStringIndexerTable = BaseAddress + 0x479A58,
                PatchStringAddressTable = BaseAddress + 0x479A44,
                ExpansionStringIndexerTable = BaseAddress + 0x479A5C,
                ExpansionStringAddressTable = BaseAddress + 0x479A48,
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion114B(IntPtr BaseAddress)
        {
            return new GameMemoryTable()
            {
                Loading = BaseAddress + 0x30EF7C,
                Saving = BaseAddress + 0x370760,
                Saving2 = BaseAddress + 0x371380,
                InGame = BaseAddress + 0x30EBC4,
                InMenu = BaseAddress + 0x47993C,

                GlobalData = BaseAddress + 0x00340D78,
                World = BaseAddress + 0x0047BD78,
                PlayersX = BaseAddress + 0x47BDB0,
                GameId = BaseAddress + 0x0047AD4C,
                LowQualityItems = BaseAddress + 0x564C98,
                ItemDescriptions = BaseAddress + 0x564A98,
                MagicModifierTable = BaseAddress + 0x564ABC,
                RareModifierTable = BaseAddress + 0x564AE0,
                Units113 = null,
                Units114 = BaseAddress + 0x0039DEF8,
                PlayerUnit = BaseAddress + 0x0039DEFC,
                Area = BaseAddress + 0x0039B1C8,
                Pets = BaseAddress + 0x003B3644,
                InventoryTab = BaseAddress + 0x003B4CD4,
                StringIndexerTable = BaseAddress + 0x47AAF4,
                StringAddressTable = BaseAddress + 0x47AAF8,
                PatchStringIndexerTable = BaseAddress + 0x47AB10,
                PatchStringAddressTable = BaseAddress + 0x47AAFC,
                ExpansionStringIndexerTable = BaseAddress + 0x47AB14,
                ExpansionStringAddressTable = BaseAddress + 0x47AB00,
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion113D(Dictionary<string, IntPtr> moduleBaseAddresses)
        {
            IntPtr d2CommonAddress = GetModuleAddress("D2Common.dll", moduleBaseAddresses);
            IntPtr d2LangAddress = GetModuleAddress("D2Lang.dll", moduleBaseAddresses);
            IntPtr d2NetAddress = GetModuleAddress("D2Net.dll", moduleBaseAddresses);
            IntPtr d2GameAddress = GetModuleAddress("D2Game.dll", moduleBaseAddresses);
            IntPtr d2ClientAddress = GetModuleAddress("D2Client.dll", moduleBaseAddresses);

            return new GameMemoryTable()
            {
                Loading = d2ClientAddress + 0x11D364,
                Saving = null,
                Saving2 = null,
                InGame = null,
                InMenu = null,

                GlobalData = d2CommonAddress + 0x000A33F0,
                World = d2GameAddress + 0x111C10,
                PlayersX = d2GameAddress + 0x111C44,
                GameId = d2NetAddress + 0xB420, //  and the pointer to that address is: new IntPtr(d2NetAddress + 0x70A8);
                LowQualityItems = d2CommonAddress + 0xA4EB0,
                ItemDescriptions = d2CommonAddress + 0xA4CB0,
                MagicModifierTable = d2CommonAddress + 0xA4CD4,
                RareModifierTable = d2CommonAddress + 0xA4CF8,
                Units113 = d2ClientAddress + 0x001049B8,
                Units114 = null,
                PlayerUnit = d2ClientAddress + 0x00101024,
                Area = d2ClientAddress + 0x0008F66C,
                Pets = d2ClientAddress + 0x0011CE30,
                InventoryTab = d2ClientAddress + 0x0011CB84,
                StringIndexerTable = d2LangAddress + 0x10A84,
                StringAddressTable = d2LangAddress + 0x10A88,
                PatchStringIndexerTable = d2LangAddress + 0x10AA0,
                PatchStringAddressTable = d2LangAddress + 0x10A8C,
                ExpansionStringIndexerTable = d2LangAddress + 0x10AA4,
                ExpansionStringAddressTable = d2LangAddress + 0x10A90,
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion113C(Dictionary<string, IntPtr> moduleBaseAddresses)
        {
            IntPtr d2CommonAddress = GetModuleAddress("D2Common.dll", moduleBaseAddresses);
            IntPtr d2LangAddress = GetModuleAddress("D2Lang.dll", moduleBaseAddresses);
            IntPtr d2NetAddress = GetModuleAddress("D2Net.dll", moduleBaseAddresses);
            IntPtr d2GameAddress = GetModuleAddress("D2Game.dll", moduleBaseAddresses);
            IntPtr d2ClientAddress = GetModuleAddress("D2Client.dll", moduleBaseAddresses);

            return new GameMemoryTable()
            {
                Loading = d2ClientAddress + 0xFAE08,
                Saving = null,
                Saving2 = null,
                InGame = null,
                InMenu = null,

                GlobalData = d2CommonAddress + 0x00099E1C,
                World = d2GameAddress + 0x111C24,
                PlayersX = d2GameAddress + 0x111C1C,
                GameId = d2NetAddress + 0xB428,
                LowQualityItems = d2CommonAddress + 0x9FD98,
                ItemDescriptions = d2CommonAddress + 0x9FB94,
                MagicModifierTable = d2CommonAddress + 0x9FBB8,
                RareModifierTable = d2CommonAddress + 0x9FBDC,
                Units113 = d2ClientAddress + 0x0010A808,
                Units114 = null,
                PlayerUnit = d2ClientAddress + 0x0010A60C,
                Area = d2ClientAddress + 0x0011C310,
                Pets = d2ClientAddress + 0x0011C4D4,
                InventoryTab = d2ClientAddress + 0x0011BC94,
                StringIndexerTable = d2LangAddress + 0x10A64,
                StringAddressTable = d2LangAddress + 0x10a68,
                PatchStringIndexerTable = d2LangAddress + 0x10A80,
                PatchStringAddressTable = d2LangAddress + 0x10A6C,
                ExpansionStringIndexerTable = d2LangAddress + 0x10A84,
                ExpansionStringAddressTable = d2LangAddress + 0x10A70,
            };
        }

        private IntPtr GetModuleAddress(string moduleName, Dictionary<string, IntPtr> moduleBaseAddresses)
        {
            if (!moduleBaseAddresses.TryGetValue(moduleName, out IntPtr address))
                throw new ModuleNotLoadedException(moduleName);
            return address;
        }

        public GameMemoryTable CreateForReader(IProcessMemoryReader reader)
        {
            try
            {
                Logger.Info($"Check version: {reader.FileVersion}");
                return CreateForVersion(reader.FileVersion, reader.BaseAddress, reader.ModuleBaseAddresses);
            }
            catch (GameVersionUnsupportedException)
            {
                try
                {
                    return CreateForReaderD2SEFallback1(reader);
                }
                catch (GameVersionUnsupportedException)
                {
                    return CreateForReaderD2SEFallback2(reader);
                }
            }
        }

        private GameMemoryTable CreateForReaderD2SEFallback1(IProcessMemoryReader reader)
        {
            // we try to detect version for D2SE
            IntPtr baseAddress;
            try
            {
                baseAddress = GetModuleAddress("Fog.dll", reader.ModuleBaseAddresses);
            } catch (ModuleNotLoadedException)
            {
                throw new GameVersionUnsupportedException("Unknown");
            }
            var pointer = new Pointer() { Base = baseAddress + 0x0004AFE0, Offsets = new int[] { 0x0, 0xe00 } };
            IntPtr versionAddress = reader.ResolvePointer(pointer);
            string version = reader.ReadNullTerminatedString(versionAddress, 20, Encoding.ASCII);
            Logger.Info($"Check D2SE version: {version}");
            return CreateForVersion(version, reader.BaseAddress, reader.ModuleBaseAddresses);
        }

        private GameMemoryTable CreateForReaderD2SEFallback2(IProcessMemoryReader reader)
        {
            // pre DiabloInterface v0.4.11 check for 1.13c in D2SE
            //
            // the check just reads the string displayed in D2SE,
            // so this is not a safe check, but apparently neither is the one above.
            // Until we can safely determine the D2 version with a single
            // method, we keep the legacy check...
            //
            // this should be 1.13c, but most likely isn't, in many cases
            string version = reader.ReadNullTerminatedString(reader.BaseAddress + 0x1A049, 5, Encoding.ASCII);
            Logger.Info($"Check D2SE version (Fallback check): {version}");
            return CreateForVersion(version, reader.BaseAddress, reader.ModuleBaseAddresses);
        }
    }
}
