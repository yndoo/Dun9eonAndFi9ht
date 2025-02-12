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
            float prevHp = character.CurrentHp;
            float prevMp = character.CurrentMp;
            float prevAtk = character.Atk;
            float prevDef = character.Def;
            float prevCrt = character.Crt;
            float prevCrtDmg = character.CrtDmg;
            float prevMiss = character.Miss;

            float hp = isPercent ? character.MaxHp * changeHp : changeHp;
            float mp = isPercent ? character.MaxMp * changeMp : changeMp;
            float atk = isPercent ? character.Atk * changeAtk : changeAtk;
            float def = isPercent ? character.Def * changeDef : changeDef;
            float crt = isPercent ? character.Crt * changeCrt : changeCrt;
            float crtDmg = isPercent ? character.CrtDmg * changeCrtDmg : changeCrtDmg;
            float miss = isPercent ? character.Miss * changeMiss : changeMiss;

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
                character.ApplyBuff(hp, mp, atk, def, crt, crtDmg, miss, duration);
            }

            PrintResult(character, this, prevHp, prevMp, prevAtk, prevDef, prevCrt, prevCrtDmg, prevMiss);
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
        /// <summary>
        /// 포션 사용 시 스탯 변동을 확인하고 결과 출력하는 함수
        /// </summary>
        /// <param name="character">사용자</param>
        /// <param name="potion">사용 포션</param>
        /// <param name="prevHp">변하기 전 Hp</param>
        /// <param name="prevMp">변하기 전 MP</param>
        /// <param name="prevAtk">변하기 전 ATK</param>
        /// <param name="prevDef">변하기 전 DEf</param>
        /// <param name="prevCrt">변하기 전 Crt</param>
        /// <param name="prevCrtDmg">변하기 전 CrtDmg</param>
        /// <param name="prevMiss">변하기 전 Miss</param>
        public void PrintResult(Character character, Potion potion, float prevHp, float prevMp, float prevAtk, float prevDef, float prevCrt, float prevCrtDmg, float prevMiss)
        {
            List<string> changes = new List<string>();
            List<string> statChanges = new List<string>();

            Utility.ClearAll();
            Utility.PrintScene($"{potion.name}을(를) 사용하였습니다!\n");

            if (potion.duration != 0)
                changes.Add($"지속 시간: {potion.duration}턴");

            if (potion.changeHp!=0)
            {
                changes.Add($"HP: +{potion.changeHp}");
                statChanges.Add($"HP: {prevHp:F2} → {character.CurrentHp:F2}");
            }
            if (potion.changeMp !=0)
            {
                changes.Add($"MP: +{potion.changeMp}");
                statChanges.Add($"MP: {prevMp:F2} → {character.CurrentMp:F2}");
            }
            if (potion.changeAtk!=0)
            {
                changes.Add($"ATK: +{potion.changeAtk}");
                statChanges.Add($"ATK: {prevAtk:F2} → {character.Atk:F2}");
            }
            if (potion.changeDef!=0)
            {
                changes.Add($"DEF: +{potion.changeDef}");
                statChanges.Add($"DEF: {prevDef:F2} → {character.Def:F2}");
            }
            if (potion.changeCrt!=0)
            {
                changes.Add($"CRT: +{potion.changeCrt}");
                statChanges.Add($"CRT: {prevCrt:F2} → {character.Crt:F2}");
            }
            if (potion.changeCrtDmg!=0)
            {
                changes.Add($"CRTDmg: +{potion.changeCrtDmg}");
                statChanges.Add($"CRTDmg: {prevCrtDmg:F2} → {character.CrtDmg:F2}");
            }
            if (potion.changeMiss!=0)
            {
                changes.Add($"MISS: +{potion.changeMiss}");
                statChanges.Add($"MISS: {prevMiss:F2} → {character.Miss:F2}");
            }

            if (changes.Count > 0)
                Utility.PrintScene(string.Join("\n", changes));

            if (statChanges.Count > 0)
            {
                Utility.PrintScene("스탯 변화:");
                Utility.PrintScene(string.Join("\n", statChanges));
            }

            Utility.PrintMenu("아무 키나 입력해주세요");
            Console.ReadKey();
        }

    }
}
