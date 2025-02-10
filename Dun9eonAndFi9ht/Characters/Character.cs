using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinition;

namespace Dun9eonAndFi9ht.Characters
{
    public class Character
    {
        public string Name { get; set; }
        public int MaxHp { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Level { get; set; }
        private int currentHp;
        public int CurrentHp
        {
            get
            {
                if (currentHp <= 0)
                {
                    return 0;
                }
                else
                {
                    return currentHp;
                }
            }
        }
        public bool IsDead { get; private set; }
        public int FinalAtk { get; private set; }

        public Character(string name, int maxHp, int atk, int def, int level)
        {
            this.Name = name;
            this.MaxHp = maxHp;
            this.Atk = atk;
            this.Def = def;
            this.Level = level;
            this.currentHp = maxHp;
            this.IsDead = false;
        }

        public virtual void Attack(Character target)
        {
            // 오차 범위 소수점 올림 처리
            int remain = (Atk % Constants.ERROR_RATE) > 0 ? 1 : 0;
            int error = Atk / Constants.ERROR_RATE + remain;

            Random random = new Random();
            FinalAtk = Atk + random.Next(-error, error + 1);
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
            IsDead = true;
        }
    }
}
