using Dun9eonAndFi9ht.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Skill
{
    public abstract class SkillBase
    {
        public string Name { get; private set; }
        public int MpCost { get; private set; }
        public string Desc { get; private set; }

        public SkillBase(string name, int mpCost, string desc)
        {
            this.Name = name;
            this.MpCost = mpCost;
            this.Desc = desc;
        }

        public abstract void UseSkill(Character user, List<Character> targets);
    }
}