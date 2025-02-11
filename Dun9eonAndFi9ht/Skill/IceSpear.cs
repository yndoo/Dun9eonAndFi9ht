using Dun9eonAndFi9ht.Characters;
using DataDefinition;

namespace Dun9eonAndFi9ht.Skill
{
    public class IceSpear : SkillBase
    {
        public IceSpear() : base("아이스 스피어", 12, "여러개의 얼음 창을 소환해 적 모두에게 공격력의 80% 피해를 줍니다.", ESkillTargetType.Multi)
        {
        }

        /// <summary>
        /// 스킬 사용 "아이스 스피어"
        /// 현재 모든 적을 받아와서 살아있는 적들만 선택
        /// </summary>
        /// <param name="user">스킬을 사용하는 유저</param>
        /// <param name="targets">모든 적 리스트</param>
        public override void UseSkill(Character user, List<Character> targets)
        {
            List<Character> aliveTargets = new List<Character>();
            foreach (Character target in targets)
            {
                if (!target.IsDead)
                {
                    aliveTargets.Add(target);
                }
            }

            // 선택된 대상 공격
            foreach (Character target in aliveTargets)
            {
                float damage = user.Atk * 0.8f;
                target.Damaged(damage);
            }
        }
    }
}
