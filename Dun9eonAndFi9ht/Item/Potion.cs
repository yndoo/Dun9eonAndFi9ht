using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Items
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
        public float changeCrtDmg;
        public float changeMiss;
        public string description;
        int duration;

        public Potion(string name, bool isPercent, float hp, float mp, float atk, float def, float crt, float crtDmg, float miss, int duration, string description)
        {
            this.name = name;
            this.isPercent = isPercent;
            this.changeHp = hp;
            this.changeMp = mp;
            this.changeAtk = atk;
            this.changeDef = def;
            this.changeCrt = crt;
            this.changeCrtDmg = crtDmg;
            this.changeMiss = miss;
            this.duration = duration;
            this.description = description;
        }
        /// <summary>
        /// 포션 사용 시 어떤 식으로 작동하는지를 담은 메소드
        /// </summary>
        /// <param name="character">포션 사용하는 캐릭터</param>
        public void UsePotion(Character character)
        {
            if (!isPercent)
            {
                if(duration == 0)
                {
                    character.CurrentHp += changeHp;
                    character.CurrentMp += changeMp;
                    character.Atk += changeAtk;
                    character.Def += changeDef;
                    character.Crt += changeCrt;
                    character.CrtDmg += changeCrtDmg;
                    character.Miss += changeMiss;
                }
                else
                {
                    character.ApplyBuff(changeHp, changeMp, changeAtk, changeDef, changeCrt, changeCrtDmg, changeMiss, duration);
                }
                PrintResult(changeHp, changeMp, changeAtk, changeDef, changeCrt, changeCrtDmg, changeMiss, duration);
            }
            else 
            {
                float hp = CalResult(character.MaxHp, changeHp);
                float mp = CalResult(character.MaxMp, changeMp);
                float atk = CalResult(character.Atk, changeAtk);
                float def = CalResult(character.Def, changeDef);
                float crt = CalResult(character.Crt, changeCrt);
                float crtDmg = CalResult(character.CrtDmg, changeCrtDmg);
                float miss = CalResult(character.Miss, changeMiss);
                if (duration == 0)
                {
                    character.CurrentHp += hp;
                    character.CurrentMp += mp;
                    character.Atk += atk;
                    character.Def += def;
                    character.Crt += crt;
                    character.CrtDmg += crtDmg;
                    character.Miss += miss;
                }
                else
                {
                    character.ApplyBuff(hp, mp, atk, def, crt,crtDmg, miss, duration);
                }
                PrintResult(hp, mp, atk, def, crt,crtDmg, miss, duration);
            }
        }
        /// <summary>
        /// 간단히 얼마를 추가, 제거해야하는지를 표시하는 함수
        /// </summary>
        /// <param name="chStat"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        private float CalResult(float chStat, float percent)
        {
            if (chStat == 0 || percent == 0) return 0;
            return chStat * percent;
        }
        /// <summary>
        /// 퍼센트 회복, 수치 회복에 따라 결과를 다르게 내놓는 함수
        /// </summary>
        public string DisplayPotion()
        {
            List<string> changes = new List<string>();
            if (duration != 0) changes.Add($" {duration}턴 지속");
            if (changeHp != 0) changes.Add($" 체력 {(changeHp >= 0 ? "+" : "-")}{(isPercent ? changeHp * 100 : changeHp)}{(isPercent ? "%" : "")}");
            if (changeMp != 0) changes.Add($" 마나 {(changeMp >= 0 ? "+" : "-")}{(isPercent ? changeMp * 100 : changeMp)}{(isPercent ? "%" : "")}");
            if (changeAtk != 0) changes.Add($" 공격력 {(changeAtk >= 0 ? "+" : "-")}{(isPercent ? changeAtk * 100 : changeAtk)}{(isPercent ? "%" : "")}");
            if (changeDef != 0) changes.Add($" 방어력 {(changeDef >= 0 ? "+" : "-")}{(isPercent ? changeDef * 100 : changeDef)}{(isPercent ? "%" : "")}");
            if (changeCrt != 0) changes.Add($" 치명타 확률 {(changeCrt >= 0 ? "+" : "-")}{(isPercent ? changeCrt * 100 : changeCrt)}{(isPercent ? "%" : "")}");
            if (changeCrtDmg != 0) changes.Add($" 치명타 데미지 {(changeCrtDmg >= 0 ? "+" : "-")}{(isPercent ? changeCrtDmg * 100 : changeCrtDmg)}{(isPercent ? "%" : "")}");
            if (changeMiss != 0) changes.Add($" 회피 {(changeMiss >= 0 ? "+" : "-")}{(isPercent ? changeMiss * 100 : changeMiss)}{(isPercent ? "%" : "")}");

            string message = $"{name} | {description} |" + string.Join(" | ", changes);
            return message;
        }

        public void PrintResult(float hp, float mp, float atk, float def, float crt, float crtDmg, float miss, int duration)
        {
            List<string> changes = new List<string>();
            if (duration != 0) changes.Add($"지속 시간: {duration}턴");
            if (hp != 0) changes.Add($"HP: {(hp > 0 ? "+" : "")} {hp}");
            if (mp != 0) changes.Add($"MP: {(mp > 0 ? "+" : "")}{mp}");
            if (atk != 0) changes.Add($"ATK: {(atk > 0 ? "+" : "")}{atk}");
            if (def != 0) changes.Add($"DEF: {(def > 0 ? "+" : "")}{def}");
            if (crt != 0) changes.Add($"CRT: {(crt > 0 ? "+" : "")}{crt}");
            if (crtDmg != 0) changes.Add($"CRTDmg: {(crtDmg > 0 ? "+" : "")}{crtDmg}");
            if (miss != 0) changes.Add($"MISS: {(miss > 0 ? "+" : "")}{miss}");

            if (changes.Count > 0)
            {
                Utility.PrintScene($"{name} 사용!\n" + string.Join("\n", changes));
            }
        }
    }
}
