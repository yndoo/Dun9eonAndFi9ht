using DataDefinition;
using Dun9eonAndFi9ht.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Skill
{
    public class MonsterSkill : SkillBase
    {
        public MonsterSkill(string name, int mpCost, string desc, ESkillTargetType targetType, float value) : base(name, mpCost, desc, ESkillTargetType.Single, value)
        {
        }

        /// <summary>
        /// 몬스터의 스킬 사용
        /// </summary>
        /// <param name="user">스킬 사용자</param>
        /// <param name="targets">스킬 대상자</param>
        /// <returns>스킬 대상자</returns>
        public override List<Character> UseSkill(Character user, List<Character> targets)
        {
            ConsumeMp(user);

            user.SetFinalAtk(user.Atk * Value);

            return targets;
        }
    }
}
