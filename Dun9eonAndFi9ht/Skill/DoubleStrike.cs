using Dun9eonAndFi9ht.Characters;
using DataDefinition;

namespace Dun9eonAndFi9ht.Skill
{
    public class DoubleStrike : SkillBase
    {
        public DoubleStrike(string name, int mpCost, string desc, ESkillTargetType targetType, float value) : base(name, mpCost, desc, targetType, value)
        {
        }

        /// <summary>
        /// 스킬 사용 "더블 스트라이크"
        /// 현재 모든 적을 받아와서 살아있는 적들만 선택
        /// </summary>
        /// <param name="user">스킬을 사용하는 유저</param>
        /// <param name="targets">모든 적 리스트</param>
        public override List<Character> UseSkill(Character user, List<Character> targets)
        {
            ConsumeMp(user);

            List<Character> aliveTargets = new List<Character>();
            foreach (Character target in targets)
            {
                if (!target.IsDead)
                {
                    aliveTargets.Add(target);
                }
            }

            Random random = new Random();
            // 랜덤하게 2명 선택
            List<Character> selectedTargets = aliveTargets.OrderBy(x => random.Next()).Take(2).ToList();

            // 선택된 대상 반환 및 최종 공격력 결정
            user.SetFinalAtk(user.Atk * 1.5f);

            return selectedTargets;
        }
    }
}
