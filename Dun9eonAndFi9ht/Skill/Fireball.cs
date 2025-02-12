using Dun9eonAndFi9ht.Characters;
using DataDefinition;

namespace Dun9eonAndFi9ht.Skill
{
    public class Fireball : SkillBase
    {
        public Fireball(string name, int mpCost, string desc, ESkillTargetType targetType, float value) : base(name, mpCost, desc, targetType, value)
        {
        }

        /// <summary>
        /// 스킬 사용 "파이어볼"
        /// </summary>
        /// <param name="user">스킬을 사용하는 유저</param>
        /// <param name="targets">스킬 대상 1명</param>
        public override List<Character> UseSkill(Character user, List<Character> targets)
        {
            ConsumeMp(user);

            user.SetFinalAtk(user.Atk * 1.2f);

            return targets;
        }
    }
}
