namespace Zutatensuppe.D2Reader
{
    using System;

    public interface IGameMemoryTableFactory
    {
        GameMemoryTable CreateForVersion(string gameVersion);
    }

    public class GameMemoryTableFactory : IGameMemoryTableFactory
    {
        public GameMemoryTable CreateForVersion(string gameVersion)
        {
            var memoryTable = new GameMemoryTable
            {
                // Offsets are the same for all versions so far.
                Offset = { Quests = new[] { 0x264, 0x450, 0x20, 0x00 } }
            };

            switch (gameVersion)
            {
                /*
                // how to find the addresses in cheat engine, for example after new patch without too big changes
                // note: cheat engine wants the addresses with the 400000 offset. 
                // so when copying to CE, add 400000, when copying from CE, substract 400000

                // World
                // ...
                // - launch diablo
                // - scan for 0
                // - start a game 
                // - scan for changed value
                // - relaunch diablo and start a game
                // - scan for unchanged value
                // - leave game
                // - scan for 0
                // - do that until about 200 addresses are left, while in game, some addresses change,
                //   but when clicking outside of game, they are unchanged again. remove those addresses
                // - remove all addresses that are not green
                // - remove all addresses where the values do not look like addresses
                // - there should be about 11 remaining addresses, browse memory of those
                // - the one you are looking for has:
                //   - lots of 0000000 before it
                //   - another address close afterwards
                //   - a counter of some sort a bit after that (or loop variable or something)
                //   - a value field whose value is changing every some seconds
                // - to verify, go to that found address +24 and check if there is a int (= 3) there

                // GameId
                // - launch diablo
                // - scan for int 0
                // - start a game 
                // - scan for changed value
                // -- (repeat until satisfied)
                // - exit game
                // - scan for unchanged value
                // - enter new game
                // - scan for increased value by 1
                // -- (repeat above if neccessary)
                // - there is one green address left, that is the game id

                // PlayerUnit
                // tricky.. might not always work
                // - go to town and stand still
                // - scan for int 5
                // - start running
                // - scan for int 3
                // - start walking
                // - scan for int 6
                // - stand still
                // - scan for int 5
                // - there should be some addresses (eg 05DF8B10 and 05E02710)
                // - (unpause the game if paused)
                // - watch for both addresses + 34 (05DF8B44 and 05E02744). discard the address where the value stays 0 
                // - note that address - 44 (eg 05DF8B00)
                // - make a scan for that address
                // - it is the first green address

                // Area
                // - go to an area (eg. rouge camp)
                // - scan for unknown value
                // - wait a bit
                // -- (repeat from here if needed)
                // - scan for unchanged value
                // - go to another area (eg. cold plains)
                // - scan for changed value
                // - wait a bit 
                // - scan for unchanged value
                // -- (repeat above if needed)
                // - there should be some addresses. browse all
                // - it should be the one address that has lots of 000000 before and lots of FFFFFFF afterwards

                // GlobalData
                // - go ingame
                // - search 0000170300001703 (hex, 8bytes)
                // - that address - 44
                // - make a pointer scan for that address
                // - chose the one address that has 0 offset. it is that address

                // LowQualityItems
                // - search '43 72 75 64 65 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 AE 51 43 72 61 63 6B 65 64 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 BD 06 44 61 6D 61 67 65 64'
                //   (value type: array of byte)
                // - note the address (eg 066FC2F0)
                // - search for the found address (hex, value type: 4 bytes)
                // - note the address, should be only one green (eg 0096CC5C)
                // - that address - 4

                // ItemDescriptions
                // - tricky.. might not work!
                // - search '66 6C 70 68 61 78 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 69 6E 76 68 61 78 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 69 6E 76 68 61 78 75 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 69 6E 76 68 61 78 75'
                // - note the first address (eg 0566D00C)
                // - search for the found address duplicated (hex, value type: 8 bytes, eg 0566D00C0566D00C)
                // - the found address - 4

                // MagicModifierTable
                // - search '6F 66 20 48 65 61 6C 74 68 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2D 07 00 00 03 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 01 00 FF 00 07 00 00 00 01 00 00 00 00 00 00 00 01 05 FF FF 00 00 03 00 25 00 33'
                // - note the address (eg 0599CE8C)
                // - search for the found address duplicated (hex, value type: 8 bytes, eg 0599CE8C0599CE8C)
                // - the found address - 4

                // RareModifierTable
                // - search '00 00 00 00 00 00 00 00 00 00 00 00 15 09 00 00 1E 00 20 00 21 00 22 00 1C 00 43 00 00 00 00 00 00 00 00 00 00 00 62 69 74 65 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 16 09 00 00 1E 00 20 00 21 00 22 00 43 00 00 00 00 00 00 00 00 00 00 00 00 00 73 63 72 61 74 63 68'
                // - note the address (eg 066364BC)
                // - search for the found address duplicated (hex, value type: 8 bytes, eg 066364BC066364BC)
                // - the found address - 4

                // the following depend on the language!! the resulting address should however be the same in the end!

                // StringIndexerTable
                // - see StringAddressTable
                // - that address - 4

                // StringAddressTable
                // - open a game
                // - for german:
                //   - search 'Kampfstab' and go back to the [ms]. use that address
                // - for english:
                //   - search 'Quarterstaff' and use that address
                // - note the address (eg 00C56024)
                // - search for the found address (hex, value type: 4 bytes)
                // - note the non-green address (eg 05A03C24)
                // - search for the found address (hex, value type: 4 bytes)
                // - the found address is the StringAddressTable

                // PatchStringIndexerTable
                // - see PatchStringAddressTable
                // - that address + 14

                // PatchStringAddressTable
                // - open a game
                // - for german:
                //   - search 'Party-Schaden' unicode string
                // - for english:
                //   - search 'Party Damage' unicode string
                // - look before that address, there should be some 20 00 00 00. the first of those is the address. (it should be the found address -14)
                // - note the address (eg 05A14A8C)
                // - search for the found address (hex, value type: 4 bytes)
                // - note the non-green address (eg 059990E4)
                // - search for the found address (hex, value type: 4 bytes)
                // - the found address is the StringAddressTable

                // ExpansionStringIndexerTable
                // - see ExpansionStringAddressTable
                // - that address + 14

                // ExpansionStringAddressTable
                // - open a game
                // - for german or english:
                //   - search 'Trang' unicode string
                // - there are many addresses. the one we are looking for is at the beginning of a block and before it is a block of ????????
                // - note the address (eg 0599CE8C)
                // - search for the found address (hex, value type: 4 bytes)
                // - note the non-green address (eg 05A09074)
                // - search for the found address (hex, value type: 4 bytes)
                // - the found address is the StringAddressTable
                */

                case "1.14b":
                    memoryTable.Address.GlobalData = new IntPtr(0x00340D78);

                    memoryTable.Address.World = new IntPtr(0x0047BD78);
                    memoryTable.Address.GameId = new IntPtr(0x0047AD4C);
                    memoryTable.Address.LowQualityItems = new IntPtr(0x564C98);
                    memoryTable.Address.ItemDescriptions = new IntPtr(0x564A98);
                    memoryTable.Address.MagicModifierTable = new IntPtr(0x564ABC);
                    memoryTable.Address.RareModifierTable = new IntPtr(0x564AE0);

                    memoryTable.Address.PlayerUnit = new IntPtr(0x0039DEFC);//(0x39DAF8);
                    memoryTable.Address.Area = new IntPtr(0x0039B1C8);
                    memoryTable.Address.StringIndexerTable = new IntPtr(0x47AAF4);
                    memoryTable.Address.StringAddressTable = new IntPtr(0x47AAF8);
                    memoryTable.Address.PatchStringIndexerTable = new IntPtr(0x47AB10);
                    memoryTable.Address.PatchStringAddressTable = new IntPtr(0x47AAFC);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x47AB14);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x47AB00);

                    break;
                case "1.14c":
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
                case "1.14d":
                default:
                    /*
                    // some global things:

                    // 0x6C1164: a list of int (addresses), length is probably 22 .. there is a switch case reading from it when casting a spell


                    // possible statlist flags?
                    // they are referenced directly, for example:
                    // => 0x6CE264 => 00 00 00 00
                    // => 0x6CE268 => 01 00 00 00
                    // => 0x6CE26C => 02 00 00 00
                    // => 0x6CE270 => 04 00 00 00
                    // ...

                    // some global stuff found in asm:

                    // 0x745774 => ??? contains Address to something?
                    // 0x7457AC => ??? (some number or const or type id?)

                    // 0x7A0610
                    // 0x7A0650
                    // 0x7A0654
                    // 0x7A2788 => some number?
                    // 0x7A27C0 => some base address to an array
                    // 0x7A2808 => ???
                    // 0x724AC0 => base address for D2CharacterClassSkillIconStruct, length is 7*0x22 (classes: 0-6)
                    // 0x7A6A70 => contains address of player unit

                    // 0x7A6F96 => some array ?
                    // 0x7A740A => end address of that array?

                    // 0x7c07fc => probably some value (0x03e60000 returned when getting some char class related info from skill)
                    // 0x7c8cb0 => some global number read at beginning of "gameLoop/renderUI?" (maybe some kind of game state?

                    // 0x96c8a8 => an address to int array of the length 7? and all 7 values seem to be 0x63
                    // 0x9D4180 => something in "soSomethingWithSkill2" func

                    // somehing to do with the screen resolution???
                    // 0x71146C => ??? some number ... values seen: 0x320 (= 800)
                    // 0x711470 => ??? some number ... values seen: 0x258 (= 600)
                    // 0x7A6AB0 => ??? some number ... values seen: 0x190 (= 400)
                    // 0x7A6AAC => ??? some number ... values seen: 0x12C (= 300)
                    */

                    memoryTable.Address.GlobalData = new IntPtr(0x00344304); // =>  points to 0x96bc30 (0x56bc30)

                    memoryTable.Address.World = new IntPtr(0x00483D38); // points to 0x7A0690 (0x3A0690)
                    memoryTable.Address.GameId = new IntPtr(0x00482D0C);
                    memoryTable.Address.LowQualityItems = new IntPtr(0x56CC58); // count. pointer is that address + 4
                    memoryTable.Address.ItemDescriptions = new IntPtr(0x56CA58);
                    memoryTable.Address.MagicModifierTable = new IntPtr(0x56CA7C);
                    memoryTable.Address.RareModifierTable = new IntPtr(0x56CAA0);

                    memoryTable.Address.PlayerUnit = new IntPtr(0x003A5E74); // points to the player struct address
                    memoryTable.Address.Area = new IntPtr(0x003A3140); // integer that is the current area of the player
                    memoryTable.Address.StringIndexerTable = new IntPtr(0x4829B4); // => points to 0x6684C7C
                    memoryTable.Address.StringAddressTable = new IntPtr(0x4829B8); // => points to 0x5A03c24
                    memoryTable.Address.PatchStringIndexerTable = new IntPtr(0x4829D0);
                    memoryTable.Address.PatchStringAddressTable = new IntPtr(0x4829BC);
                    memoryTable.Address.ExpansionStringIndexerTable = new IntPtr(0x4829D4);
                    memoryTable.Address.ExpansionStringAddressTable = new IntPtr(0x4829C0);

                    break;
            }

            return memoryTable;
        }
    }
}
