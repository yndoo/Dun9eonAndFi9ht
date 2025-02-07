using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinition;

namespace Dun9eonAndFi9ht.Characters
{
    internal class Monster : Character
    {
        public Reward Reward { get; private set; }

        public Monster(string name, int maxHp, int atk, int def, int level, int exp, int gold) : base(name, maxHp, atk, def, level)
        {
            Reward = new Reward(exp, gold);
        }

        public override void Attack(Character target)
        {
            base.Attack(target);
        }

        public override void Damaged(int damage)
        {
            base.Damaged(damage);
        }

        public override void Dead()
        {
            base.Dead();
        }
    }
}
