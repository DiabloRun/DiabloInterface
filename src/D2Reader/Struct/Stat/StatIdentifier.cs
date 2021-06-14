namespace Zutatensuppe.D2Reader.Struct.Stat
{
    // see ItemStatCost.txt
    public enum StatIdentifier : ushort
    {
        Strength                = 0x00,
        Energy                  = 0x01,
        Dexterity               = 0x02,
        Vitality                = 0x03,
        StatPoints              = 0x04,
        NewSkills               = 0x05,
        Hitpoints               = 0x06,
        HitpointsMax            = 0x07,
        Mana                    = 0x08,
        ManaMax                 = 0x09,
        Stamina                 = 0x0A,
        StaminaMax              = 0x0B,
        Level                   = 0x0C,
        Experience              = 0x0D,
        Gold                    = 0x0E,
        GoldStash               = 0x0F,
        ItemArmorPercent        = 0x10,
        ItemDamageMaxPercent    = 0x11,
        ItemDamageMinPercent    = 0x12,
        AttackRating            = 0x13,
        BlockRating             = 0x14,
        DamageMin               = 0x15,
        DamageMax               = 0x16,
        SecondaryDamageMin      = 0x17,
        SecondaryDamageMax      = 0x18,
        DamagePercent           = 0x19,
        ManaRecovery            = 0x1A,
        ManaRecoveryBonus       = 0x1B,
        StaminaRecoveryBonus    = 0x1C,
        ExperienceLast          = 0x1D,
        ExperienceNext          = 0x1E,
        Defense                 = 0x1F,
        DefenseVsMissile        = 0x20,
        DefenseVsHTH            = 0x21,
        DamageReduction         = 0x22,
        MagicDamageReduction    = 0x23,
        ResistPhysical          = 0x24,
        ResistMagic             = 0x25,
        ResistMagicMax          = 0x26,
        ResistFire              = 0x27,
        ResistFireMax           = 0x28,
        ResistLightning         = 0x29,
        ResistLightningMax      = 0x2A,
        ResistCold              = 0x2B,
        ResistColdMax           = 0x2C,
        ResistPoison            = 0x2D,
        ResistPoisonMax         = 0x2E,
        DamageAura              = 0x2F,
        FireDamageMin           = 0x30,
        FireDamageMax           = 0x31,
        LightningDamageMin      = 0x32,
        LightningDamageMax      = 0x33,
        MagicDamageMin          = 0x34,
        MagicDamageMax          = 0x35,
        ColdDamageMin           = 0x36,
        ColdDamageMax           = 0x37,
        ColdDamageLength        = 0x38,
        PoisonDamageMin         = 0x39, // Bitrate
        PoisonDamageMax         = 0x3A, // Bitrate
        PoisonDamageDuration    = 0x3B,
        LifeLeechMin            = 0x3C,
        LifeLeechMax            = 0x3D,
        ManaLeechMin            = 0x3E,
        ManaLeechMax            = 0x3F,
        StaminaDrainMin         = 0x40,
        StaminaDrainMax         = 0x41,
        StunLength              = 0x42,

        VelocityPercent         = 0x43, // this is +frw from skills
        AttackRate              = 0x44, // this is +ias from skills

        OtherAnimRate           = 0x45,
        Quantity                = 0x46,
        Value                   = 0x47,
        Durability              = 0x48, // Current Durability of the item
        DurabilityMax           = 0x49, //
        HealthRegen             = 0x4A,

        Unknown_4B              = 0x4B, // something to do with Durability?
                                        // maybe used in game.484E90 (1.14d)
                                        // if that refers to a statidentifier
        Unknown_4C              = 0x4C,
        Unknown_4D              = 0x4D,

        AttackerSelfDamage      = 0x4E,
        MonsterGold             = 0x4F, // +%
        MagicFind               = 0x50, // +%
        Knockback               = 0x51,

        Unknown_52              = 0x52,
        Unknown_53              = 0x53,
        Unknown_54              = 0x54,
        Unknown_55              = 0x55,
        Unknown_56              = 0x56,
        Unknown_57              = 0x57,
        Unknown_58              = 0x58,

        LightRadius             = 0x59,

        Unknown_5A              = 0x5A,
        Unknown_5B              = 0x5B, // seen in game.62EAF0

        Unknown_5C              = 0x5C,

        IncreasedAttackSpeed    = 0x5D,

        Unknown_5E              = 0x5E,
        Unknown_5F              = 0x5F,

        FasterRunWalk           = 0x60,

        Unknown_61              = 0x61,
        Unknown_62              = 0x62,

        FasterHitRecovery       = 0x63,

        Unknown_64              = 0x64,

        FastBlockRate           = 0x65, // dunno what is that
        FasterBlockRate         = 0x66,

        Unknown_67              = 0x67,
        Unknown_68              = 0x68,

        FasterCastRate          = 0x69,

        Unknown_6A              = 0x6A,
        Unknown_6B              = 0x6B,
        Unknown_6C              = 0x6C,
        Unknown_6D              = 0x6D,
        Unknown_6E              = 0x6E,
        Unknown_6F              = 0x6F,
        Unknown_70              = 0x70,
        Unknown_71              = 0x71,
        Unknown_72              = 0x72,
        Unknown_73              = 0x73,
        Unknown_74              = 0x74,
        Unknown_75              = 0x75,

        HalfFreezeDuration      = 0x76,
        AttackRatingPercent     = 0x77, // +%

        Unknown_78              = 0x78,

        DamageToDemonsPercent   = 0x79, // +%

        Unknown_7A              = 0x7A,

        AttackRatingToDemons    = 0x7B,

        Unknown_7C              = 0x7C,
        Unknown_7D              = 0x7D,
        Unknown_7E              = 0x7E,
        Unknown_7F              = 0x7F,

        AttackerSelfLightningDamage = 0x80, // for example isenharts shield

        Unknown_81              = 0x81,
        Unknown_82              = 0x82,
        Unknown_83              = 0x83,
        Unknown_84              = 0x84,
        Unknown_85              = 0x85,
        Unknown_86              = 0x86,
        Unknown_87              = 0x87,
        Unknown_88              = 0x88,
        Unknown_89              = 0x89,
        Unknown_8A              = 0x8A,
        Unknown_8B              = 0x8B,
        Unknown_8C              = 0x8C,
        Unknown_8D              = 0x8D,
        Unknown_8E              = 0x8E,
        Unknown_8F              = 0x8F,
        Unknown_90              = 0x90,
        Unknown_91              = 0x91,
        Unknown_92              = 0x92,
        Unknown_93              = 0x93,
        Unknown_94              = 0x94,
        Unknown_95              = 0x95,
        Unknown_96              = 0x96,
        Unknown_97              = 0x97,

        Unknown_98              = 0x98, // something to do with durability
                                        // 1.14d: used in fn game.629930

        CannotBeFrozen          = 0x99,

        SocketCount             = 0xC2,

        // Not listed in ItemStatCost.txt, but used for poison calculations.
        PoisonDivisor           = 0x146

        // some things that came up but we dont define yet:
        // 0xFC 3   (seen on: Felloak, lvl req?)
        // 0xC9 (values: 10, 10)
        //
    }
}
