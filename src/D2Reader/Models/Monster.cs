using Zutatensuppe.D2Reader.Struct.Monster;

namespace Zutatensuppe.D2Reader.Models
{
    public class Monster
    {
        public int Class; // eClass value of the unit, determines what monster this is
                           // (Fallen, Zombie, Charsie, ...)

        public MonsterTypeFlags TypeFlags; // Flags describing the monster further
                                           // eg. Champion, Possessed, Ghostly, ...
                                           // Determined by current Game State

        public MonsterType Type; // Type of monster (None, Demon, Undead)
                                 // D2 stores this (differently) in monStats.
                                 // Determined by eClass

        public bool IsDemon() => Type == MonsterType.Demon;

        public bool IsUndead() => Type == MonsterType.Undead;

        public bool IsChampion() => TypeFlags.HasFlag(MonsterTypeFlags.Champion);

        public bool IsMinion() => TypeFlags.HasFlag(MonsterTypeFlags.Minion);

        public bool IsUnique() => TypeFlags.HasFlag(MonsterTypeFlags.Unique);

        public bool IsSuperunique() => TypeFlags.HasFlag(MonsterTypeFlags.SuperUnique);
    }

    public enum MonsterType
    {
        None = 0, // None, or Beast
        Demon,
        Undead,
    }
}
