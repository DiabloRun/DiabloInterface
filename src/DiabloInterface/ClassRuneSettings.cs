using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.DiabloInterface.Lib;

namespace Zutatensuppe.DiabloInterface
{
    [Serializable]
    public class ClassRuneSettings : IClassRuneSettings
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CharacterClass? Class { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public GameDifficulty? Difficulty { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IReadOnlyList<Rune> Runes { get; set; } = new List<Rune>();
    }
}
