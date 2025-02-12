using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinition;
using Dun9eonAndFi9ht.Skill;

namespace Dun9eonAndFi9ht.Characters
{
    public class Character
    {
        public string Name
        {
            get; set;
        }
        public float MaxHp
        {
            get; set;
        }
        public float MaxMp
        {
            get; set;
        }
        public float Atk
        {
            get; set;
        }
        public float Def
        {
            get; set;
        }

        public float Crt
        {
            get; set;
        }
        public float CrtDmg
        {
            get; set;
        }
        public float Miss
        {
            get; set;
        }

        public int Level
        {
            get; set;
        }
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
            set
            {
                if (currentHp <= 0)
                {
                    currentHp = 0;
                }
                else if (currentHp + value > MaxHp)
                {
                    currentHp = MaxHp;
                }
                else
                {
                    currentHp = value;
                }
            }
        }
        private float currentMp;
        public float CurrentMp
        {
            get
            {
                if (currentMp <= 0)
                {
                    return 0;
                }
                else
                {
                    return currentMp;
                }
            }

            set
            {
                if (currentMp <= 0)
                {
                    currentMp = 0;
                }
                else if (currentMp + value > MaxMp)
                {
                    currentMp = MaxMp;
                }
                else
                {
                    currentMp = value;
                }
            }
        }
        public bool IsDead
        {
            get; private set;
        }
        public float FinalAtk
        {
            get; private set;
        }
        public List<SkillBase> Skills
        {
            get; protected set;
        }

        private int buffDuration = 0;
        private float buffHp = 0, buffMp = 0, buffAtk = 0, buffDef = 0, buffCrt = 0, buffCrtDmg = 0, buffMiss = 0;

        public Character(string name, float maxHp, float maxMp, float atk, float def, int level)
        {
            this.Name = name;
            this.MaxHp = maxHp;
            this.MaxMp = maxMp;
            this.Atk = atk;
            this.Def = def;
            this.Level = level;
            this.currentHp = maxHp;
            this.currentMp = maxMp;
            this.IsDead = false;
        }

        public virtual void Attack(Character target)
        {
            Random random = new Random();
            float error = Atk * (Constants.ERROR_RATE / 100f); // 10% 오차 계산
            FinalAtk = Atk + (float)(random.NextDouble() * 2 - 1) * error;
        }

        public virtual void SetFinalAtk(float value)
        {
            FinalAtk = value;
        }

        public void UseMp(float amount)
        {
            if (CurrentMp >= amount)
            {
                CurrentMp -= amount;
            }
            else
            {
                CurrentMp = 0;
            }
        }

        public virtual void Damaged(float damage)
        {
            float finalDMG = Math.Max(damage - (Def * 0.5f), 1);
            currentHp -= finalDMG;
            if (currentHp <= 0)
            {
                Dead();
            }
        }

        public virtual void Dead()
        {
            IsDead = true;
        }


        // 버프 적용
        public void ApplyBuff(float hp, float mp, float atk, float def, float crt, float crtDmg, float miss, int duration)
        {
            buffHp = hp;
            buffMp = mp;
            buffAtk = atk;
            buffDef = def;
            buffCrt = crt;
            buffCrtDmg = crtDmg;
            buffMiss = miss;
            buffDuration = duration;

            // 능력치에 즉시 반영
            MaxHp += buffHp;
            MaxMp += buffMp;
            Atk += buffAtk;
            Def += buffDef;
            Crt += buffCrt;
            CrtDmg += buffCrtDmg;
            Miss += buffMiss;
        }

        public void DurationReduce()
        {
            buffDuration--;
        }

        // 턴이 지나면 버프 해제
        public void UpdateTurn()
        {
            if (buffDuration > 0)
            {
                buffDuration--;

                if (buffDuration == 0)
                {
                    RemoveBuff();
                }
            }
        }

        // 버프 해제
        private void RemoveBuff()
        {
            MaxHp -= buffHp;
            MaxMp -= buffMp;
            Atk -= buffAtk;
            Def -= buffDef;
            Crt -= buffCrt;
            CrtDmg -= buffCrtDmg;
            Miss -= buffMiss;

            // 초기화
            buffHp = buffMp = buffAtk = buffDef = buffCrt = buffMiss = 0;
        }


    }
}
