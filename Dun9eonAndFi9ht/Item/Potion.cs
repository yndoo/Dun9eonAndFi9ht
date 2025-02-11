using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Item
{
    public class Potion
    {
        public string name;
        public bool isPercent;
        public float changeHp;
        public float changeMp;
        public float changeAtk;
        public float changeDef;
        public float changeCrt;
        public float changeMiss;
        int duration;

        public Potion(string name, bool isPercent, float hp, float mp, float atk, float def, float crt, float miss, int duration)
        {
            this.name = name;
            this.isPercent = isPercent;
            this.changeHp = hp;
            this.changeMp = mp;
            this.changeAtk = atk;
            this.changeDef = def;
            this.changeCrt = crt;
            this.changeMiss = miss;
            this.duration = duration;
        }

        public void UsePotion(Character character)
        {
            if (isPercent)
            {
                if(duration == 0)
                {
                    character.CurrentHp += changeHp;
                    character.CurrentMp += changeMp;
                    character.Atk += changeAtk;
                    character.Def += changeDef;
                    character.Crt += changeCrt;
                    character.Miss += changeMiss;
                }
                else
                {
                    character.ApplyBuff(changeHp, changeMp, changeAtk, changeDef, changeCrt, changeMiss, duration);
                }
            }
            else 
            {
                float hp = CalResult(character.CurrentHp, changeHp);
                float mp = CalResult(character.CurrentMp, changeMp);
                float atk = CalResult(character.Atk, changeAtk);
                float def = CalResult(character.Def, changeDef);
                float crt = CalResult(character.Crt, changeCrt);
                float miss = CalResult(character.Miss, changeMiss);
                if (duration == 0)
                {
                    character.CurrentHp += hp;
                    character.CurrentMp += mp;
                    character.Atk += atk;
                    character.Def += def;
                    character.Crt += crt;
                    character.Miss += miss;
                }
                else
                {
                    character.ApplyBuff(hp, mp, atk, def, crt, miss, duration);
                }
            }

        }

        private float CalResult(float chStat, float percent)
        {
            if (chStat == 0 || percent == 0) return 0;
            return chStat * percent;
        }

        public void PrintResult(float hp, float mp, float atk, float def, float crt, float miss, int duration)
        {
            List<string> changes = new List<string>();
            if (duration != 0) changes.Add($"지속 시간: {duration}턴");
            if (hp != 0) changes.Add($"HP: {(hp > 0 ? "+" : "")}{hp}");
            if (mp != 0) changes.Add($"MP: {(mp > 0 ? "+" : "")}{mp}");
            if (atk != 0) changes.Add($"ATK: {(atk > 0 ? "+" : "")}{atk}");
            if (def != 0) changes.Add($"DEF: {(def > 0 ? "+" : "")}{def}");
            if (crt != 0) changes.Add($"CRT: {(crt > 0 ? "+" : "")}{crt}");
            if (miss != 0) changes.Add($"MISS: {(miss > 0 ? "+" : "")}{miss}");

            if(changes.Count > 0)
            {
                Utility.PrintScene($"{name} 사용!\n"+string.Join("\n", changes));
            }
        }
    }
}
