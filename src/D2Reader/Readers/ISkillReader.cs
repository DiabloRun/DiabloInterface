using System.Collections.Generic;
using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Skill;

namespace Zutatensuppe.D2Reader.Readers
{
    public interface ISkillReader
    {
        D2SkillData GetSkillData(ushort skillIdentifier);

        string GetSkillName(ushort skillIdentifier);

        IEnumerable<D2Skill> EnumerateSkills(D2Unit unit);

        int GetTotalNumberOfSkillPoints(D2Skill skill);

        D2SkillData ReadSkillData(D2Skill skill);
    }
}
