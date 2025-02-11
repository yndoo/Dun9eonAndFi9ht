using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinition;
using Dun9eonAndFi9ht.Skill;

namespace Dun9eonAndFi9ht.Characters
{
    public class Monster : Character
    {
        public Reward Reward { get; private set; }
        public EMonsterType MonsterType { get; private set; }

        public Monster(string name, float maxHp, int maxMp, float atk, float def, int level, int gold, EMonsterType monsterType) : base(name, maxHp, maxMp, atk, def, level)
        {
            Reward = new Reward(level, gold);

            // 몬스터 유형별 스킬 자동 세팅
            Skills = SkillManager.Instance.GetMonsterSkills(monsterType);
        }

        public override void Attack(Character target)
        {
            base.Attack(target);
        }

        public override void Damaged(float damage)
        {
            base.Damaged(damage);
        }

        public override void Dead()
        {
            base.Dead();
        }
    }
}
