using Dun9eonAndFi9ht.Characters;
using DataDefinition;

namespace Dun9eonAndFi9ht.Skill
{
    public class IceSpear : SkillBase
    {
        public IceSpear(string name, int mpCost, string desc, ESkillTargetType targetType, float value) : base(name, mpCost, desc, targetType, value)
        {
        }

        /// <summary>
        /// 스킬 사용 "아이스 스피어"
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

            // 선택된 대상 반환 및 최종 공격력 결정
            user.SetFinalAtk(user.Atk * 0.8f);

            return aliveTargets;
        }
    }
}
