using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zutatensuppe.D2Reader.Models
{
    public class SkillInfo
    {
        public uint Id = 0;

        // hard points put into the skill (skills from items not included)
        public int Points = 0;

        public override bool Equals(object obj)
        {
            SkillInfo other = obj as SkillInfo;
            return Id == other.Id
                && Points == other.Points;
        }

        public override int GetHashCode()
        {
            int hashCode = -1339893796;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + Points.GetHashCode();
            return hashCode;
        }
    }
}
