using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Characters
{
    public struct Reward
    {
        struct Reward
        {
            public int exp;
            public int gold;

        public Reward(int exp, int gold)
        {
            this.exp = exp;
            this.gold = gold;
        }
    }

    internal class Monster : Character
    {
        private Reward reward;

        public Reward Reward => reward;

        public Monster(string name, int maxHp, int atk, int def, int level, int exp, int gold) : base(name, maxHp, atk, def, level)
        {
            reward = new Reward(exp, gold);
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
