﻿using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Skill;

namespace Dun9eonAndFi9ht.Skill
{
    public class SlashFrenzy : SkillBase
    {
        public SlashFrenzy() : base("난도질", 15, "빠른 연속 공격으로 공격력의 170% 피해를 줍니다.")
        {
        }

        /// <summary>
        /// 스킬 사용 "난도질"
        /// </summary>
        /// <param name="user">스킬을 사용하는 유저</param>
        /// <param name="targets">스킬 대상 1명</param>
        public override void UseSkill(Character user, List<Character> targets)
        {
            float damage = user.Atk * 1.7f;
            targets[0].Damaged(damage);
        }
    }
}