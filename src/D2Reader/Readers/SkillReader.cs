using Zutatensuppe.D2Reader.Struct;
using Zutatensuppe.D2Reader.Struct.Skill;
using System;
using System.Collections.Generic;

namespace Zutatensuppe.D2Reader.Readers
{
    internal class SkillReader: ISkillReader
    {
        IProcessMemoryReader reader;
        protected IStringReader stringReader;
        D2GlobalData globals;

        public SkillReader(IProcessMemoryReader reader, GameMemoryTable memory)
        {
            this.reader = reader;

            globals = reader.Read<D2GlobalData>(reader.ReadAddress32(memory.GlobalData));
            stringReader = new StringReader(reader, memory);
        }

        public IEnumerable<D2Skill> EnumerateSkills(D2Unit unit)
        {
            // first skill comes first
            var skill = reader.Read<D2Skill>(unit.pSkills.Address);
            if (skill == null) yield break;
            // seems like the first item that is found in that address is not an actual skill
            // yield return skill;

            // all other skills follow
            while ( !skill.pNextSkill.IsNull )
            {
                skill = reader.Read<D2Skill>(skill.pNextSkill);
                if (skill == null) yield break;
                yield return skill;
            }
        }

        private int GetAdditionalNumberOfSkillPoints(D2Skill skill)
        {
            // not finished implementation..
            throw new NotImplementedException();

            // get points in skill from items ...
            /*
            int skillPoints = 0;

            D2SkillData skillData = ReadSkillData(skill);
            if (skillData == null)
                return skillPoints; // 0

            if (skillData.SkillId >= globals.SkillCount)
                return skillPoints; // 0

            int x = skill.__UnknownUsed;
            
            return skillPoints;
            */
        }

        public int GetTotalNumberOfSkillPoints(D2Skill skill)
        {
            int skillPoints = 0;

            // real points in the skill:
            skillPoints = skill.numberOfSkillPoints;

            // skillPoints += GetAdditionalNumberOfSkillPoints(skill);

            return skillPoints;
        }

        public D2SkillData ReadSkillData(D2Skill skill)
        {
            return reader.Read<D2SkillData>(skill.pSkillData);
        }

        public D2SkillData GetSkillData(ushort skillIdentifier)
        {
            // Skills.txt
            return reader.IndexIntoArray<D2SkillData>(globals.Skills, skillIdentifier, globals.SkillCount);
        }

        // TODO: can likely be cached for as long as one D2 instance is running
        public string GetSkillName(ushort skillIdentifier)
        {
            D2SkillData skillData = GetSkillData(skillIdentifier);
            if (skillData == null) return null;

            // Skill description table (unknown).
            if (skillData.SkillDescriptionId == 0 || skillData.SkillDescriptionId >= globals.SkillDescriptionCount)
                return null;
            IntPtr skillDescriptionAddress = globals.SkillDescriptions.Address + (int)skillData.SkillDescriptionId * 0x120;
            UInt16 skillNameID = reader.ReadUInt16(skillDescriptionAddress + 8);

            if (skillNameID == 0) return null;
            return stringReader.GetString(skillNameID);
        }

    }
}
