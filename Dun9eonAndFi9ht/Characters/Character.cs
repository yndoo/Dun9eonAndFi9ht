using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Characters
{
    internal class Character
    {
        const int ERROR_RATE = 10;

        protected string name;
        protected int maxHp;
        protected int atk;
        protected int def;
        protected int level;
        protected int currentHp;
        protected bool isDead;

        public string Name  => name;
        public int MaxHp { get; set; }
        public int Atk
        {
            get
            {
                // 오차 범위 소수점 올림 처리
                int remain = (atk % ERROR_RATE) > 0 ? 1 : 0;
                int error = atk / ERROR_RATE + remain;

                Random random = new Random();
                int finalAtk = atk + random.Next(-error, error + 1);
                return finalAtk;
            }
        }
        public int Def { get; set; }
        public int Level { get; set; }
        public int CurrentHp { get; set; }
        public bool IsDead => isDead;

        public Character(string name, int maxHp, int atk, int def, int level)
        {
            this.name = name;
            this.maxHp = maxHp;
            this.atk = atk;
            this.def = def;
            this.level = level;
            currentHp = maxHp;
            isDead = false;
        }

        public virtual void Attack(Character target)
        {
            target.Damaged(Atk);
        }

        public virtual void Damaged(int damage)
        {
            currentHp -= damage;
            if (currentHp <= 0)
            {
                Dead();
            }
        }

        public virtual void Dead()
        {
            isDead = true;
        }
    }
}
