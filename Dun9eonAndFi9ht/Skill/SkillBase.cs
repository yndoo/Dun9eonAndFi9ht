﻿using Dun9eonAndFi9ht.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinition;

namespace Dun9eonAndFi9ht.Skill
{
    public abstract class SkillBase
    {
        public string Name { get; private set; }
        public int MpCost { get; private set; }
        public string Desc { get; private set; }
        public ESkillTargetType Type { get; private set; }
        public float Value { get; private set; }


        public SkillBase(string name, int mpCost, string desc, ESkillTargetType type, float value)
        {
            this.Name = name;
            this.MpCost = mpCost;
            this.Desc = desc;
            this.Type = type;
            this.Value = value;
        }

        public abstract List<Character> UseSkill(Character user, List<Character> targets);

        public virtual void ConsumeMp(Character user)
        {
            user.UseMp(MpCost);
        }
    }
}