namespace Zutatensuppe.D2Reader
{
    using System;
    using System.Collections.Generic;

    public interface IGameMemoryTableFactory
    {
        GameMemoryTable CreateForVersion(string gameVersion, Dictionary<Models.D2Module, IntPtr> moduleBaseAddresses);
    }

    public class GameVersionUnsupportedException : Exception
    {
        public String GameVersion;
        public GameVersionUnsupportedException(String gameVersion) :
            base(string.Format("Failed to create memory table for game version {0}", gameVersion))
        {
            GameVersion = gameVersion;
        }
    }

    public class GameMemoryTableFactory : IGameMemoryTableFactory
    {
        public GameMemoryTable CreateForVersion(string gameVersion, Dictionary<Models.D2Module, IntPtr> moduleBaseAddresses)
        {
            var memoryTable = new GameMemoryTable
            {
                // Offsets are the same for all versions so far.
                Offset = { Quests = new[] { 0x264, 0x450, 0x20, 0x00 } }
            };

            // Refer to the wiki at https://github.com/Zutatensuppe/DiabloInterface/wiki/Finding-memory-table-addresses
            // for information about how to find addresses for a different version of the game.
            switch (gameVersion)
            {
                case Models.GameVersion.V113C:
                    int baseAddress = 0x400000;
                    try
                    {
                        int d2CommonAddress = moduleBaseAddresses[Models.D2Module.D2Common].ToInt32() - baseAddress; // D2Common.dll
                        int d2LaunchAddress = moduleBaseAddresses[Models.D2Module.D2Launch].ToInt32() - baseAddress; // D2Launch.dll
                        int d2LangAddress = moduleBaseAddresses[Models.D2Module.D2Lang].ToInt32(); // D2Lang.dll
                        int d2NetAddress = moduleBaseAddresses[Models.D2Module.D2Net].ToInt32() - baseAddress; // D2Net.dll
                        int d2GameAddress = moduleBaseAddresses[Models.D2Module.D2Game].ToInt32() - baseAddress; // D2Game.dll
                        int d2ClientAddress = moduleBaseAddresses[Models.D2Module.D2Client].ToInt32() - baseAddress; // D2Client.dll
                        memoryTable.Address.GlobalData = new IntPtr(d2CommonAddress + 0x00099E1C);

                        memoryTable.Address.World = new IntPtr(d2GameAddress + 0x111C24);
                        memoryTable.Address.GameId = new IntPtr(d2NetAddress + 0xB428);
                        memoryTable.Address.LowQualityItems = new IntPtr(d2CommonAddress + 0x9FD98);
                        memoryTable.Address.ItemDescriptions = new IntPtr(d2CommonAddress + 0x9FB94);
                        memoryTable.Address.MagicModifierTable = new IntPtr(d2CommonAddress + 0x9FBB8);
                        memoryTable.Address.RareModifierTable = new IntPtr(d2CommonAddress + 0x9FBDC);

                        memoryTable.Address.PlayerUnit = new IntPtr(d2ClientAddress + 0x0010A60C);
                        memoryTable.Address.Area = new IntPtr(d2ClientAddress + 0x0011C310);

                        memoryTable.Address.StringIndexerTable = new IntPtr(-baseAddress + d2LangAddress + 0x10A64);
                        memoryTable.Address.StringAddressTable = new IntPtr(-baseAddress + d2LangAddress + 0x10a68);
                        memoryTable.Address.PatchStringIndexerTable = new IntPtr(-baseAddress + d2LangAddress + 0x10A6C);
                        memoryTable.Address.PatchStringAddressTable = new IntPtr(-baseAddress + d2LangAddress + 0x10A80);
                        memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(-baseAddress + d2LangAddress + 0x10A84);
                        memoryTable.Address.ExpansionStringAddressTable = new IntPtr(-baseAddress + d2LangAddress + 0x10A70);
                    } catch(KeyNotFoundException ex)
                    {
                        // TODO: throw ProcessMemoryReadException instead...
                        throw new GameVersionUnsupportedException(gameVersion);
                    }


                    break;
                case Models.GameVersion.V113D:
                    throw new GameVersionUnsupportedException(gameVersion);
                case Models.GameVersion.V114B:
                    memoryTable.Address.GlobalData = new IntPtr(0x00340D78);

                    memoryTable.Address.World = new IntPtr(0x0047BD78);
                    memoryTable.Address.GameId = new IntPtr(0x0047AD4C);
                    memoryTable.Address.LowQualityItems = new IntPtr(0x564C98);
                    memoryTable.Address.ItemDescriptions = new IntPtr(0x564A98);
                    memoryTable.Address.MagicModifierTable = new IntPtr(0x564ABC);
                    memoryTable.Address.RareModifierTable = new IntPtr(0x564AE0);

                    memoryTable.Address.PlayerUnit = new IntPtr(0x0039DEFC);
                    memoryTable.Address.Area = new IntPtr(0x0039B1C8);
                    memoryTable.Address.StringIndexerTable = new IntPtr(0x47AAF4);
                    memoryTable.Address.StringAddressTable = new IntPtr(0x47AAF8);
                    memoryTable.Address.PatchStringIndexerTable = new IntPtr(0x47AB10);
                    memoryTable.Address.PatchStringAddressTable = new IntPtr(0x47AAFC);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x47AB14);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x47AB00);

                    break;
                case Models.GameVersion.V114C:
                    memoryTable.Address.GlobalData = new IntPtr(0x33FD78);

                    memoryTable.Address.World = new IntPtr(0x0047ACC0);
                    memoryTable.Address.GameId = new IntPtr(0x00479C94);
                    memoryTable.Address.LowQualityItems = new IntPtr(0x563BE0);
                    memoryTable.Address.ItemDescriptions = new IntPtr(0x5639E0);
                    memoryTable.Address.MagicModifierTable = new IntPtr(0x563A04);
                    memoryTable.Address.RareModifierTable = new IntPtr(0x563A28);

                    memoryTable.Address.PlayerUnit = new IntPtr(0x0039CEFC);
                    memoryTable.Address.Area = new IntPtr(0x0039A1C8);
                    memoryTable.Address.StringIndexerTable = new IntPtr(0x479A3C);
                    memoryTable.Address.StringAddressTable = new IntPtr(0x479A40);
                    memoryTable.Address.PatchStringIndexerTable = new IntPtr(0x479A58);
                    memoryTable.Address.PatchStringAddressTable = new IntPtr(0x479A44);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x479A5C);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x479A48);

                    break;
                case Models.GameVersion.V114D:
                    memoryTable.Address.GlobalData = new IntPtr(0x00344304);

                    memoryTable.Address.World = new IntPtr(0x00483D38);
                    memoryTable.Address.GameId = new IntPtr(0x00482D0C);
                    memoryTable.Address.LowQualityItems = new IntPtr(0x56CC58);
                    memoryTable.Address.ItemDescriptions = new IntPtr(0x56CA58);
                    memoryTable.Address.MagicModifierTable = new IntPtr(0x56CA7C);
                    memoryTable.Address.RareModifierTable = new IntPtr(0x56CAA0);

                    memoryTable.Address.PlayerUnit = new IntPtr(0x003A5E74);
                    memoryTable.Address.Area = new IntPtr(0x003A3140);
                    memoryTable.Address.StringIndexerTable = new IntPtr(0x4829B4);
                    memoryTable.Address.StringAddressTable = new IntPtr(0x4829B8);
                    memoryTable.Address.PatchStringIndexerTable = new IntPtr(0x4829D0);
                    memoryTable.Address.PatchStringAddressTable = new IntPtr(0x4829BC);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x4829D4);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x4829C0);

                    break;

                default:
                    throw new GameVersionUnsupportedException(gameVersion);
            }

            return memoryTable;
        }
    }
}
