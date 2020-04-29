namespace Zutatensuppe.D2Reader.Struct.Skill
{
    // when a value for a skill is read, this is used in a switch case to determine, 
    // where and how from skillData the value must be read
    enum SkillValueType
    {
        CUR_VALUE_BY_PARAM_1_AND_2 = 0, // param1 + (skillPts-1)*param2
        MIN_MAX_BY_PARAM_1_AND_2 = 1,
        CUR_VALUE_BY_PARAM_3_AND_4 = 2, // param3 + (skillPts-1)*param4
        MIN_MAX_BY_PARAM_3_AND_4 = 3,
        CUR_VALUE_BY_PARAM_5_AND_6 = 4, // param5 + (skillPts-1)*param6
        MIN_MAX_BY_PARAM_5_AND_6 = 5,
        // 6
        // 7
        PARAM_1 = 8,
        // 9
        // 10
        // 11
        // 12
        // 13
        PARAM_7 = 14,
        PARAM_8 = 15,
        TOTAL_SKILLPOINTS = 16, // returns just param 4, which is the curret total number of skill points for the skill
    }
}
