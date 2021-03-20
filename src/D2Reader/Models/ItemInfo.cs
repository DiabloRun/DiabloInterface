using System.Collections.Generic;
using System.Linq;
using Zutatensuppe.D2Reader.Readers;
using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Item;

namespace Zutatensuppe.D2Reader.Models
{
    // Class to represent an item for the outside world (like pipe server, http web push, ...)
    // TODO: add some kind of json serializer/renderer to the Item class instead
    //       because all the info SHOULD be available through that class already
    public class ItemInfo
    {
        // array of item base names in english so they can
        // be used like ids in the outside world
        // the index in the array is the item's eClass
        // the names were directly extracted from an english
        // version of the game with patch 1.14d applied
        // (may have to be adjusted if new item classes are added)
        static readonly string[] BaseNames = new string[]
        {
            "Hand Axe", "Axe", "Double Axe", "Military Pick", "War Axe", "Large Axe", "Broad Axe", "Battle Axe", "Great Axe", "Giant Axe", "Wand", "Yew Wand", "Bone Wand", "Grim Wand", "Club", "Scepter", "Grand Scepter", "War Scepter", "Spiked Club", "Mace", "Morning Star", "Flail", "War Hammer", "Maul", "Great Maul", "Short Sword", "Scimitar", "Sabre", "Falchion", "Crystal Sword", "Broad Sword", "Long Sword", "War Sword", "Two-Handed Sword", "Claymore", "Giant Sword", "Bastard Sword", "Flamberge", "Great Sword", "Dagger", "Dirk", "Kris", "Blade", "Throwing Knife", "Throwing Axe", "Balanced Knife", "Balanced Axe", "Javelin", "Pilum", "Short Spear", "Glaive", "Throwing Spear", "Spear", "Trident", "Brandistock", "Spetum", "Pike", "Bardiche", "Voulge", "Scythe", "Poleaxe", "Halberd", "War Scythe", "Short Staff", "Long Staff", "Gnarled Staff", "Battle Staff", "War Staff", "Short Bow", "Hunter's Bow", "Long Bow", "Composite Bow", "Short Battle Bow", "Long Battle Bow", "Short War Bow", "Long War Bow", "Light Crossbow", "Crossbow", "Heavy Crossbow", "Repeating Crossbow", "Rancid Gas Potion", "Oil Potion", "Choking Gas Potion", "Exploding Potion", "Strangling Gas Potion", "Fulminating Potion", "Decoy Gidbinn", "The Gidbinn", "Wirt's Leg", "Horadric Malus", "Hell Forge Hammer", "Horadric Staff", "Shaft of the Horadric Staff", "Hatchet", "Cleaver", "Twin Axe", "Crowbill", "Naga", "Military Axe", "Bearded Axe", "Tabar", "Gothic Axe", "Ancient Axe", "Burnt Wand", "Petrified Wand", "Tomb Wand", "Grave Wand", "Cudgel", "Rune Scepter", "Holy Water Sprinkler", "Divine Scepter", "Barbed Club", "Flanged Mace", "Jagged Star", "Knout", "Battle Hammer", "War Club", "Martel de Fer", "Gladius", "Cutlass", "Shamshir", "Tulwar", "Dimensional Blade", "Battle Sword", "Rune Sword", "Ancient Sword", "Espandon", "Dacian Falx", "Tusk Sword", "Gothic Sword", "Zweihander", "Executioner Sword", "Poignard", "Rondel", "Cinquedeas", "Stiletto", "Battle Dart", "Francisca", "War Dart", "Hurlbat", "War Javelin", "Great Pilum", "Simbilan", "Spiculum", "Harpoon", "War Spear", "Fuscina", "War Fork", "Yari", "Lance", "Lochaber Axe", "Bill", "Battle Scythe", "Partizan", "Bec-de-Corbin", "Grim Scythe", "Jo Staff", "Quarterstaff", "Cedar Staff", "Gothic Staff", "Rune Staff", "Edge Bow", "Razor Bow", "Cedar Bow", "Double Bow", "Short Siege Bow", "Large Siege Bow", "Rune Bow", "Gothic Bow", "Arbalest", "Siege Crossbow", "Ballista", "Chu-Ko-Nu", "Khalim's Flail", "Khalim's Will", "Katar", "Wrist Blade", "Hatchet Hands", "Cestus", "Claws", "Blade Talons", "Scissors Katar", "Quhab", "Wrist Spike", "Fascia", "Hand Scythe", "Greater Claws", "Greater Talons", "Scissors Quhab", "Suwayyah", "Wrist Sword", "War Fist", "Battle Cestus", "Feral Claws", "Runic Talons", "Scissors Suwayyah", "Tomahawk", "Small Crescent", "Ettin Axe", "War Spike", "Berserker Axe", "Feral Axe", "Silver-edged Axe", "Decapitator", "Champion Axe", "Glorious Axe", "Polished Wand", "Ghost Wand", "Lich Wand", "Unearthed Wand", "Truncheon", "Mighty Scepter", "Seraph Rod", "Caduceus", "Tyrant Club", "Reinforced Mace", "Devil Star", "Scourge", "Legendary Mallet", "Ogre Maul", "Thunder Maul", "Falcata", "Ataghan", "Elegant Blade", "Hydra Edge", "Phase Blade", "Conquest Sword", "Cryptic Sword", "Mythical Sword", "Legend Sword", "Highland Blade", "Balrog Blade", "Champion Sword", "Colossus Sword", "Colossus Blade", "Bone Knife", "Mithril Point", "Fanged Knife", "Legend Spike", "Flying Knife", "Flying Axe", "Winged Knife", "Winged Axe", "Hyperion Javelin", "Stygian Pilum", "Balrog Spear", "Ghost Glaive", "Winged Harpoon", "Hyperion Spear", "Stygian Pike", "Mancatcher", "Ghost Spear", "War Pike", "Ogre Axe", "Colossus Voulge", "Thresher", "Cryptic Axe", "Great Poleaxe", "Giant Thresher", "Walking Stick", "Stalagmite", "Elder Staff", "Shillelagh", "Archon Staff", "Spider Bow", "Blade Bow", "Shadow Bow", "Great Bow", "Diamond Bow", "Crusader Bow", "Ward Bow", "Hydra Bow", "Pellet Bow", "Gorgon Crossbow", "Colossus Crossbow", "Demon Crossbow", "Eagle Orb", "Sacred Globe", "Smoked Sphere", "Clasped Orb", "Jared's Stone", "Stag Bow", "Reflex Bow", "Maiden Spear", "Maiden Pike", "Maiden Javelin", "Glowing Orb", "Crystalline Globe", "Cloudy Sphere", "Sparkling Ball", "Swirling Crystal", "Ashwood Bow", "Ceremonial Bow", "Ceremonial Spear", "Ceremonial Pike", "Ceremonial Javelin", "Heavenly Stone", "Eldritch Orb", "Demon Heart", "Vortex Orb", "Dimensional Shard", "Matriarchal Bow", "Grand Matron Bow", "Matriarchal Spear", "Matriarchal Pike", "Matriarchal Javelin", "Cap", "Skull Cap", "Helm", "Full Helm", "Great Helm", "Crown", "Mask", "Quilted Armor", "Leather Armor", "Hard Leather Armor", "Studded Leather", "Ring Mail", "Scale Mail", "Chain Mail", "Breast Plate", "Splint Mail", "Plate Mail", "Field Plate", "Gothic Plate", "Full Plate Mail", "Ancient Armor", "Light Plate", "Buckler", "Small Shield", "Large Shield", "Kite Shield", "Tower Shield", "Gothic Shield", "Leather Gloves", "Heavy Gloves", "Chain Gloves", "Light Gauntlets", "Gauntlets", "Boots", "Heavy Boots", "Chain Boots", "Light Plated Boots", "Greaves", "Sash", "Light Belt", "Belt", "Heavy Belt", "Plated Belt", "Bone Helm", "Bone Shield", "Spiked Shield", "War Hat", "Sallet", "Casque", "Basinet", "Winged Helm", "Grand Crown", "Death Mask", "Ghost Armor", "Serpentskin Armor", "Demonhide Armor", "Trellised Armor", "Linked Mail", "Tigulated Mail", "Mesh Armor", "Cuirass", "Russet Armor", "Templar Coat", "Sharktooth Armor", "Embossed Plate", "Chaos Armor", "Ornate Plate", "Mage Plate", "Defender", "Round Shield", "Scutum", "Dragon Shield", "Pavise", "Ancient Shield", "Demonhide Gloves", "Sharkskin Gloves", "Heavy Bracers", "Battle Gauntlets", "War Gauntlets", "Demonhide Boots", "Sharkskin Boots", "Mesh Boots", "Battle Boots", "War Boots", "Demonhide Sash", "Sharkskin Belt", "Mesh Belt", "Battle Belt", "War Belt", "Grim Helm", "Grim Shield", "Barbed Shield", "Wolf Head", "Hawk Helm", "Antlers", "Falcon Mask", "Spirit Mask", "Jawbone Cap", "Fanged Helm", "Horned Helm", "Assault Helmet", "Avenger Guard", "Targe", "Rondache", "Heraldic Shield", "Aerin Shield", "Crown Shield", "Preserved Head", "Zombie Head", "Unraveller Head", "Gargoyle Head", "Demon Head", "Circlet", "Coronet", "Tiara", "Diadem", "Shako", "Hydraskull", "Armet", "Giant Conch", "Spired Helm", "Corona", "Demonhead", "Dusk Shroud", "Wyrmhide", "Scarab Husk", "Wire Fleece", "Diamond Mail", "Loricated Mail", "Boneweave", "Great Hauberk", "Balrog Skin", "Hellforge Plate", "Kraken Shell", "Lacquered Plate", "Shadow Plate", "Sacred Armor", "Archon Plate", "Heater", "Luna", "Hyperion", "Monarch", "Aegis", "Ward", "Bramble Mitts", "Vampirebone Gloves", "Vambraces", "Crusader Gauntlets", "Ogre Gauntlets", "Wyrmhide Boots", "Scarabshell Boots", "Boneweave Boots", "Mirrored Boots", "Myrmidon Greaves", "Spiderweb Sash", "Vampirefang Belt", "Mithril Coil", "Troll Belt", "Colossus Girdle", "Bone Visage", "Troll Nest", "Blade Barrier", "Alpha Helm", "Griffon Headdress", "Hunter's Guise", "Sacred Feathers", "Totemic Mask", "Jawbone Visor", "Lion Helm", "Rage Mask", "Savage Helmet", "Slayer Guard", "Akaran Targe", "Akaran Rondache", "Protector Shield", "Gilded Shield", "Royal Shield", "Mummified Trophy", "Fetish Trophy", "Sexton Trophy", "Cantor Trophy", "Hierophant Trophy", "Blood Spirit", "Sun Spirit", "Earth Spirit", "Sky Spirit", "Dream Spirit", "Carnage Helm", "Fury Visor", "Destroyer Helm", "Conqueror Crown", "Guardian Crown", "Sacred Targe", "Sacred Rondache", "Kurast Shield", "Zakarum Shield", "Vortex Shield", "Minion Skull", "Hellspawn Skull", "Overseer Skull", "Succubus Skull", "Bloodlord Skull", "Elixir", "an evil force", "an evil force", "an evil force", "an evil force", "Stamina Potion", "Antidote Potion", "Rejuvenation Potion", "Full Rejuvenation Potion", "Thawing Potion", "Tome of Town Portal", "Tome of Identify", "Amulet", "Top of the Horadric Staff", "Ring", "Gold", "Scroll of Inifuss", "Key to the Cairn Stones", "Arrows", "Torch", "Bolts", "Scroll of Town Portal", "Scroll of Identify", "Heart", "Brain", "Jawbone", "Eye", "Horn", "Tail", "Flag", "Fang", "Quill", "Soul", "Scalp", "Spleen", "Key", "The Black Tower Key", "Right Click to permanently add 20 to Life Potion of Life", "A Jade Figurine", "The Golden Bird", "Lam Esen's Tome", "Horadric Cube", "Horadric Scroll", "Mephisto's Soulstone", "Right Click to learn skill of your choice Book of Skill", "Khalim's Eye", "Khalim's Heart", "Khalim's Brain", "Ear", "Chipped Amethyst", "Flawed Amethyst", "Amethyst", "Flawless Amethyst", "Perfect Amethyst", "Chipped Topaz", "Flawed Topaz", "Topaz", "Flawless Topaz", "Perfect Topaz", "Chipped Sapphire", "Flawed Sapphire", "Sapphire", "Flawless Sapphire", "Perfect Sapphire", "Chipped Emerald", "Flawed Emerald", "Emerald", "Flawless Emerald", "Perfect Emerald", "Chipped Ruby", "Flawed Ruby", "Ruby", "Flawless Ruby", "Perfect Ruby", "Chipped Diamond", "Flawed Diamond", "Diamond", "Flawless Diamond", "Perfect Diamond", "Minor Healing Potion", "Light Healing Potion", "Healing Potion", "Greater Healing Potion", "Super Healing Potion", "Minor Mana Potion", "Light Mana Potion", "Mana Potion", "Greater Mana Potion", "Super Mana Potion", "Chipped Skull", "Flawed Skull", "Skull", "Flawless Skull", "Perfect Skull", "Herb", "Small Charm", "Large Charm", "Grand Charm", "an evil force", "an evil force", "an evil force", "an evil force", "El Rune", "Eld Rune", "Tir Rune", "Nef Rune", "Eth Rune", "Ith Rune", "Tal Rune", "Ral Rune", "Ort Rune", "Thul Rune", "Amn Rune", "Sol Rune", "Shael Rune", "Dol Rune", "Hel Rune", "Io Rune", "Lum Rune", "Ko Rune", "Fal Rune", "Lem Rune", "Pul Rune", "Um Rune", "Mal Rune", "Ist Rune", "Gul Rune", "Vex Rune", "Ohm Rune", "Lo Rune", "Sur Rune", "Ber Rune", "Jah Rune", "Cham Rune", "Zod Rune", "Jewel", "Keep it to thaw Anya Malah's Potion", "Scroll of Knowledge", "Right Click to Cast Scroll of Resistance", "Key of Terror", "Key of Hate", "Key of Destruction", "Diablo's Horn", "Baal's Eye", "Mephisto's Brain", "Right-click to reset Stat/Skill Points Token of Absolution", "Twisted Essence of Suffering", "Charged Essence of Hatred", "Burning Essence of Terror", "Festering Essence of Destruction", "Standard of Heroes",
        };

        public int Class { get; set; }
        public string ItemName { get; set; }
        public string ItemBaseName { get; set; }
        public string QualityColor { get; set; }
        public List<string> Properties { get; set; }
        public ItemLocation Location { get; set; }
        public int GUID { get; set; }

        // backwards compatibility with D2ID
        public string BaseItem { get; set; }
        public string Quality { get; set; }

        public ItemInfo()
        {
        }

        public ItemInfo(
            Item item,
            D2Unit owner,
            UnitReader unitReader,
            IStringReader stringReader,
            IInventoryReader inventoryReader
        ) {
            Class = item.Unit.eClass;
            ItemName = unitReader.GetFullItemName(item);
            ItemBaseName = BaseItemName(item, unitReader);
            QualityColor = QualityColorDefault(item);
            Properties = unitReader.GetMagicalStrings(item, owner, inventoryReader);
            Location = item.Location;
            GUID = item.Unit.GUID;

            // Backward compatibility for D2ID:
            // TODO: add Slug/Image/EnglishBaseName or something like that, D2ID currently uses ItemName/BaseItem for
            // displaying images, which doesnt work for languages other than english, so BaseItem will be
            // the english version, to show at least some item. also Quality is set to "YELLOW" in case of
            // unique and set, if game is not english, so that D2ID falls back to the
            // BaseItem img and shows the "unique" / "set" name above the BaseItem
            if (stringReader.Language == Language.English)
            {
                BaseItem = ItemBaseName;
                Quality = QualityColor;
            }
            else
            {
                BaseItem = BaseItemNameFallback(item, unitReader);
                Quality = QualityColorFallback(item);
            }
        }

        public static bool AreEqual(ItemInfo itemA, ItemInfo itemB)
        {
            if (itemA == null && itemB == null)
            {
                return true;
            }

            if (itemA == null || itemB == null)
            {
                return false;
            }

            return itemA.Class == itemB.Class
                && itemA.ItemName == itemB.ItemName
                && itemA.ItemBaseName == itemB.ItemBaseName
                && itemA.QualityColor == itemB.QualityColor
                && itemA.Location == itemB.Location
                && PropertiesEqual(itemA, itemB);
        }

        private static bool PropertiesEqual(ItemInfo itemA, ItemInfo itemB)
        {
            if (itemA.Properties == null && itemB.Properties == null)
                return true;

            if (itemA.Properties == null || itemB.Properties == null)
                return false;

            return itemA.Properties.SequenceEqual(itemB.Properties);
        }

        private string BaseItemName(Item item, UnitReader unitReader)
        {
            var n = unitReader.GetGrammaticalName(unitReader.GetItemName(item));
            return n.Item1;
        }

        private string BaseItemNameFallback(Item item, UnitReader unitReader)
        {
            return item.Unit.eClass < BaseNames.Length ? BaseNames[item.Unit.eClass] : null;
        }

        private string QualityColorDefault(Item item)
        {
            switch (item.ItemQuality())
            {
                case ItemQuality.Low:
                case ItemQuality.Normal:
                case ItemQuality.Superior:
                    return "WHITE";
                case ItemQuality.Magic:
                    return "BLUE";
                case ItemQuality.Rare:
                    return "YELLOW";
                case ItemQuality.Crafted:
                case ItemQuality.Tempered:
                    return "ORANGE";
                case ItemQuality.Unique:
                    return "GOLD";
                case ItemQuality.Set:
                    return "GREEN";
                default:
                    return "";
            }
        }

        private string QualityColorFallback(Item item)
        {
            switch (item.ItemQuality())
            {
                case ItemQuality.Low:
                case ItemQuality.Normal:
                case ItemQuality.Superior:
                    return "WHITE";
                case ItemQuality.Magic:
                    return "BLUE";
                case ItemQuality.Rare:
                    return "YELLOW";
                case ItemQuality.Crafted:
                case ItemQuality.Tempered:
                    return "ORANGE";
                case ItemQuality.Unique:
                    return "YELLOW";
                case ItemQuality.Set:
                    return "YELLOW";
                default:
                    return "";
            }
        }
    }
}
