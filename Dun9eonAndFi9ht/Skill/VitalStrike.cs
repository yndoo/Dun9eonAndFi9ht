using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Skill;
using DataDefinition;

namespace Dun9eonAndFi9ht.Skill
{
    public class VitalStrike : SkillBase
    {
        public VitalStrike() : base("급소 찌르기", 10, "적의 급소를 찔러 공격력의 150% 피해를 줍니다.", ESkillTargetType.Single)
        {
        }

        /// <summary>
        /// 스킬 사용 "급소 찌르기"
        /// </summary>
        /// <param name="user">스킬을 사용하는 유저</param>
        /// <param name="targets">스킬 대상 1명</param>
        public override void UseSkill(Character user, List<Character> targets)
        {
            float damage = user.Atk * 1.5f;
            targets[0].Damaged(damage);
        }
    }
}
