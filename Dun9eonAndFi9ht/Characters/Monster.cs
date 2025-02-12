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

        public Monster(string name, float maxHp, int maxMp, float atk, float def, int level, int gold, EMonsterType monsterType, List<int> dropItemIDs) : base(name, maxHp, maxMp, atk, def, level)
        {
            // 확률 기반 아이템 드랍 리스트 생성
            List<int> selectedDropItems = new List<int>();
            Random random = new Random();

            foreach (int itemID in dropItemIDs)
            {
                float chance = (float)random.NextDouble();
                if (chance <= .5) // 50% 확률로 드랍
                {
                    selectedDropItems.Add(itemID);
                }
            }

            Reward = new Reward(level, gold, selectedDropItems);

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
