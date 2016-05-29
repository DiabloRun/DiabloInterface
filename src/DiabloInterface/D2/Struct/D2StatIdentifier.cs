namespace DiabloInterface.D2.Struct
{
    // see ItemStatCost.txt
    public enum D2StatIdentifier : ushort
    {
        STRENGTH = 0,
        VITALITY = 1,
        DEXTERITY = 2,
        ENERGY = 3,

        LIFE = 7, // 3 + 1 byte
        MANA = 9, // 3 + 1 byte

        STAMINA = 11, // 3 + 1 byte

        PERC_DEF = 16, // unknown.. ?

        ATTACK_RATING = 19,

        BLOCK_CHANCE = 20, // base block chance + X = chance to block ?

        ONE_HAND_DMG_MIN = 21,
        ONE_HAND_DMG_MAX = 22,
        TWO_HAND_DMG_MIN = 23,
        TWO_HAND_DMG_MAX = 24,

        PERC_MANA_REGEN = 27,
        PERC_STAMINA_REGEN = 28,

        Defense = 31,

        unknown32 = 32, // some pointer, maybe to set info?? set by arcannas sign amulet

        DMG_REDUCTION = 34,
        MAGIC_DMG_REDUCTION = 35,

        FIRE_RES = 39,
        FIRE_RES_ADD = 40,
        LIGHTNING_RES = 41,
        LIGHTNING_RES_ADD = 42,
        COLD_RES = 43,
        COLD_RES_ADD = 44,
        POISON_RES = 45,
        POISON_RES_ADD = 46,

        FIRE_DMG_MIN = 48,
        FIRE_DMG_MAX = 49,
        LIGHTNING_DMG_MIN = 50,
        LIGHTNING_DMG_MAX = 51,

        COLD_DMG_MIN = 54,
        COLD_DMG_MAX = 55,
        unknown56 = 56, // maybe cold duration/strength??  seen values: 25, 50

        // @see http://diablo2.diablowiki.net/Guide:Calculating_Poison_Damage_v1.10,_by_onderduiker
        //
        // these are set at stuff with poison dmg:
        // examples:
        // 150 dmg in 5 sek: 308|308|125|1
        //  21 dmg in 4 sek:  54| 54|100|1
        //  75 dmg in 5 sek: 154|154|125|1
        // poison damage:  POISON_DMG_BITRATE_MIN*POISON_DURATION/256 Poison damage over POISON_DURATION/25 seconds
        // and if bitrate differs: POISON_DMG_BITRATE_MIN*POISON_DURATION/256 - POISON_DMG_BITRATE_MAX*POISON_DURATION/256 Poison damage over POISON_DURATION/25 seconds
        POISON_DMG_BITRATE_MIN = 57, // poison bitrate min
        POISON_DMG_BITRATE_MAX = 58, // poison bitrate max
        POISON_DURATION = 59, // duration
        unknown11 = 326, // unknown, but seems always set when there is poison dmg

        LIFE_LEECH = 60,
        MANA_LEECH = 62,

        unknown3 = 67, // set to -5 at big shield (block speed?)
        unknown4 = 68, // set to 10 at cleglaws tooth.. (attack speed class? 10 = normal.. 10 = fast, 20 = very fast, -10 = slow, -10 = normal)

        COUNT = 70, // for example scroll tomes, keys, javelins,..

        DURABILITY_CURRENT = 72,
        DURABILITY_MAX = 73,
        RESPLENISH_LIFE = 74,

        ATTACKER_SELF_DMG = 78,
        PERC_GOLD_BY_MONSTERS = 79,
        PERC_MAGIC_FIND = 80,

        KNOCKBACK = 81,

        SKILLS_CLASS = 83, // + X to CLASS abilities (current class?)

        LIGHT_RADIUS = 89,

        FASTER_ATTACK_SPEED = 93,

        FASTER_RUN_WALK = 96,

        FASTER_HIT_RECOVERY = 99,

        FASTER_BLOCK_RATE = 102,

        unknown104 = 104, // set by arcannas sign amulet

        FASTER_CAST_RATE = 105,

        unknown7 = 107, // set when item has + to ability (can be set multiple times. seems not to matter what the ability is: examples: + 1 to energyshield, + 3 to fire wall, + 2 to jump)

        PERC_POISON_STRENGTH_REDUCTION = 110,

        unknown20 = 112, // maybe "cause monster to flee" 1 = 25%, 2 = 50%, 4 = 75%, 8 = 100%

        PERC_DAMAGE_TO_MANA = 114,
        IGNORE_TARGET_DEFENSE = 115, // not sure but likely

        PREVENT_MONSTER_HEAL = 117,
        HALF_FREEZE_DURATION = 118,
        PERC_ATTACK_RATING = 119,

        PERC_DMG_TO_DEMONS = 121,
        ATTACK_RATING_TO_DEMONS = 123,

        SKILLS_FIRE = 126, // +X to all fire skills (not 100% sure)
        SKILLS_ALL = 127, // +X to all skills (not 100% sure)
        ATTACKER_SELF_LIGHTNING_DMG = 128, // for example isenharts shield

        MANA_AFTER_KILL = 138,
        LIFE_AFTER_DEMON_KILL = 139,

        unknown13 = 140, // set at dirtfoot (boots)

        DEATH_STRIKE_CHANCE = 141,

        FIRE_ABSORBTION = 143,

        CANNOT_BE_FROZEN = 153, // unknown
        PERC_STAMINA_DRAIN_REDUCTION = 154,

        THROW_DAMAGE_MIN = 159,
        THROW_DAMAGE_MAX = 160,

        SKILLS_CLASS_SUB = 188, // +1 to fight skills barb / +1 to curses necro

        SOCKET_COUNT = 194,

        unknown14 = 201, // X perc chance for ABILITY on hit (seen for charged bolt, meteor,...)

        unknown15 = 204, // charges (for ability on item )  0x0000YYZZ   yy = max charges | zz = current charges

        // per level calculation: (int)(X*0.125*LEVEL) = shown value

        DEF_PER_LEVEL = 214, // (int)(X*0.125*LEVEL) = shown value
        LIFE_PER_LEVEL = 216, // 3+1bit instead 4bits! (int)(X*0.125*LEVEL)
        DMG_PER_LEVEL = 218, //

        ABSORB_FIRE_DMG_PER_LEVel = 235, // (int)(X*0.125*LEVEL) = shown value. set at rising sun amulet.. (maybe absorb fire dmg per level related)

        MAX_STAMINA_PER_LEVEL = 242, // (int)(X*0.125*LEVEL) = shown value. set at sandstorm treck boots.. (maybe max stamina per level related)

        SELF_REPAIR = 252, // self repair ?   3= 1 in 33 sek, 5 = 1 in 20 sek >>> 100/X = seconds in which 1 is repaired
    }
}
