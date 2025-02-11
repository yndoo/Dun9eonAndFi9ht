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
        public float MaxHp { get; set; }
        public int MaxMp { get; set; }
        public float Atk { get; set; }
        public float Def { get; set; }
        public int Level { get; set; }
        private float currentHp;
        public float CurrentHp
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
        public int CurrentMp { get; set; }
        public bool IsDead { get; private set; }
        public float FinalAtk { get; private set; }

        public Character(string name, float maxHp, int maxMp, float atk, float def, int level)
        {
            this.Name = name;
            this.MaxHp = maxHp;
            this.MaxMp = maxMp;
            this.Atk = atk;
            this.Def = def;
            this.Level = level;
            this.currentHp = maxHp;
            this.CurrentMp = maxMp;
            this.IsDead = false;
        }

        public virtual void Attack(Character target)
        {
            Random random = new Random();
            float error = Atk * (Constants.ERROR_RATE / 100f); // 10% 오차 계산
            FinalAtk = Atk + (float)(random.NextDouble() * 2 - 1) * error;
        }

        public virtual void Damaged(float damage)
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
