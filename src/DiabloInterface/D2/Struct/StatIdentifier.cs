namespace DiabloInterface.D2.Struct
{
    // see ItemStatCost.txt
    public enum StatIdentifier : ushort
    {
        Strength                = 0,
        Energy                  = 1,
        Dexterity               = 2,
        Vitality                = 3,
        StatPoints              = 4,
        NewSkills               = 5,
        Hitpoints               = 6,
        HitpointsMax            = 7,
        Mana                    = 8,
        ManaMax                 = 9,
        Stamina                 = 10,
        StaminaMax              = 11,
        Level                   = 12,
        Experience              = 13,
        Gold                    = 14,
        GoldStash               = 15,
        ItemArmorPercent        = 16,
        ItemDamageMaxPercent    = 17,
        ItemDamageMinPercent    = 18,
        AttackRating            = 19,
        BlockRating             = 20,
        DamageMin               = 21,
        DamageMax               = 22,
        SecondaryDamageMin      = 23,
        SecondaryDamageMax      = 24,
        DamagePercent           = 25,
        ManaRecovery            = 26,
        ManaRecoveryBonus       = 27,
        StaminaRecoveryBonus    = 28,
        ExperienceLast          = 29,
        ExperienceNext          = 30,
        Defense                 = 31,
        DefenseVsMissile        = 32,
        DefenseVsHTH            = 33,
        DamageReduction         = 34,
        MagicDamageReduction    = 35,
        ResistPhysical          = 36,
        ResistMagic             = 37,
        ResistMagicMax          = 38,
        ResistFire              = 39,
        ResistFireMax           = 40,
        ResistLightning         = 41,
        ResistLightningMax      = 42,
        ResistCold              = 43,
        ResistColdMax           = 44,
        ResistPoison            = 45,
        ResistPoisonMax         = 46,
        DamageAura              = 47,
        FireDamageMin           = 48,
        FireDamageMax           = 49,
        LightningDamageMin      = 50,
        LightningDamageMax      = 51,
        MagicDamageMin          = 52,
        MagicDamageMax          = 53,
        ColdDamageMin           = 54,
        ColdDamageMax           = 55,
        ColdDamageLength        = 56,
        PoisonDamageMin         = 57, // Bitrate
        PoisonDamageMax         = 58, // Bitrate
        PoisonDamageDuration    = 59,
        LifeLeechMin            = 60,
        LifeLeechMax            = 61,
        ManaLeechMin            = 62,
        ManaLeechMax            = 63,
        StaminaDrainMin         = 64,
        StaminaDrainMax         = 65,
        StunLength              = 66,
        VelocityPercent         = 67,
        AttackRate              = 68,
        OtherAnimRate           = 69,
        Quantity                = 70,
        Value                   = 71,
        Durability              = 72,
        DurabilityMax           = 73,
        HealthRegen             = 74,

        AttackerSelfDamage      = 78,
        MonsterGold             = 79, // +%
        MagicFind               = 80, // +%
        Knockback               = 81,

        LightRadius             = 89,
        IncreasedAttackSpeed    = 93,
        FasterRunWalk           = 96,
        FasterHitRecovery       = 99,
        FasterBlockRate         = 102,
        FasterCastRate          = 105,
        HalfFreezeDuration      = 118,
        AttackRatingPercent     = 119, // +%
        DamageToDemonsPercent   = 121, // +%
        AttackRatingToDemons    = 123,

        AttackerSelfLightningDamage = 128, // for example isenharts shield

        CannotBeFrozen          = 153,

        SocketCount             = 194,

        // Not listed in ItemStatCost.txt, but used for poison calculations.
        PoisonDivisor           = 326
    }
}
