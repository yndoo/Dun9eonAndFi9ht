using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Skill;
using System;

namespace Dun9eonAndFi9ht.Skill
{
    public class AlphaStrike : Skill
    {

        public AlphaStrike() : base("알파 스트라이크", 10, "공격력 * 2 로 하나의 적을 공격합니다.") 
        {
        }


        /// <summary>
        /// 스킬 사용 "알파 스트라이크"
        /// </summary>
        /// <param name="user">스킬을 사용하는 유저</param>
        /// <param name="targets">스킬 대상 1명</param>
        public override void UseSkill(Character user, List<Character> targets)
        {
            float damage = user.Atk * 2;
            targets[0].Damaged(damage);
        }


    }
}

