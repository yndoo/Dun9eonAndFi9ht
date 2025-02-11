﻿using System;
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
        public string Name { get; set; }
        public float MaxHp { get; set; }
        public float MaxMp { get; set; }
        public float Atk { get; set; }
        public float Def { get; set; }

        public float Crt { get; set; }
        public float Miss {  get; set; }

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

            set 
            {
                if (currentHp <= 0)
                {
                    currentHp = 0;
                }
                else if (currentHp > MaxHp)
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
                if (currentHp <= 0)
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
                else if (currentMp > MaxMp)
                {
                    currentMp = MaxHp;
                }
                else
                {
                    currentMp = value;
                }
            }
        }
        public bool IsDead { get; private set; }
        public float FinalAtk { get; private set; }

        public Character(string name, float maxHp, float maxMp, float atk, float def, int level)
        {
            this.Name = name;
            this.MaxHp = maxHp;
            this.MaxHp = maxMp;
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
