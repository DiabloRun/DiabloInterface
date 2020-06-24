using System;
using System.Collections.Generic;
using Zutatensuppe.D2Reader.Models;

namespace Zutatensuppe.DiabloInterface.Lib
{
    public interface IClassRuneSettings
    {
        CharacterClass? Class { get; set; }

        GameDifficulty? Difficulty { get; set; }

        IReadOnlyList<Rune> Runes { get; set; }
    }
}
