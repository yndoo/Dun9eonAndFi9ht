using Dun9eonAndFi9ht.Characters;
using DataDefinition;

namespace Dun9eonAndFi9ht.Skill
{
    public class AlphaStrike : SkillBase
    {
        public AlphaStrike() : base("알파 스트라이크", 10, "공격력의 200%로 하나의 적을 공격합니다.", ESkillTargetType.Single) 
        {
        }

        /// <summary>
        /// 스킬 사용 "알파 스트라이크"
        /// </summary>
        /// <param name="user">스킬을 사용하는 유저</param>
        /// <param name="targets">스킬 대상 1명</param>
        public override List<Character> UseSkill(Character user, List<Character> targets)
        {
            user.SetFinalAtk(user.Atk * 2);

            return targets;
        }
    }
}