using Dun9eonAndFi9ht.Characters;
namespace Dun9eonAndFi9ht.Skill
{
    public class Fireball : SkillBase
    {
        public Fireball() : base("파이어볼", 10, "적 하나에게 불덩이를 날려 공격력의 120% 피해를 줍니다.")
        {
        }

        /// <summary>
        /// 스킬 사용 "파이어볼"
        /// </summary>
        /// <param name="user">스킬을 사용하는 유저</param>
        /// <param name="targets">스킬 대상 1명</param>
        public override void UseSkill(Character user, List<Character> targets)
        {
            float damage = user.Atk * 1.2f;
            targets[0].Damaged(damage);
        }
    }
}
