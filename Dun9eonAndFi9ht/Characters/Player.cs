using DataDefinition;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.StaticClass;

namespace Dun9eonAndFi9ht.Characters
{
    public class Player : Character
    {
        public EJobType Job { get; set; }
        public int Gold { get; set; }
        public int MaxExp { get; set; }
        public int CurExp { get; set; }

        private int[] MaxExpArray;


        public Player(string name, EJobType job, float maxHp, int maxMp, float atk, float def, int level, int gold, int[] maxExpArray) : base(name, maxHp, maxMp, atk, def, level)
        {
            this.Job = job;
            this.Gold = gold;
            this.CurExp = 0;

            MaxExpArray = maxExpArray;
            this.MaxExp = MaxExpArray[0];
        }

        /// <summary>
        /// 플레이어 스탯 살펴보기
        /// </summary>
        public void DisplayStatus()
        {
            Utility.PrintScene($"Lv. {Level:D2}");
            Utility.PrintScene($"{Name} ( {GetJobName(Job)} )");
            Utility.PrintScene($"{"공격력"} : {Atk:F2}");
            Utility.PrintScene($"{"방어력"} : {Def:F2}");
            Utility.PrintScene($"{"체  력"} : {CurrentHp:F2}");
            Utility.PrintScene($"{"마  나"} : {CurrentMp}");
            Utility.PrintScene($"{"Gold"}   : {Gold}");
            if(MaxExp < 0)
            {
                Utility.PrintScene($"{"EXP"}    : 최고레벨입니다.");
            }
            else
            {
                Utility.PrintScene($"{"EXP"}    : {CurExp} / {MaxExp}");
            }
        }
        /// <summary>
        /// EJobType에 따라 맞는 이름 반환
        /// </summary>
        /// <param name="job">EJobType 값</param>
        /// <returns></returns>
        private string GetJobName(EJobType job)
        {
            switch (job)
            {
                case EJobType.Warrior:
                    return "전사";
                case EJobType.Mage:
                    return "마법사";
                case EJobType.Rogue:
                    return "도적";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 골드 획득
        /// </summary>
        /// <param name="amount">골드 획득량</param>
        public void GainGold(int amount)
        {
            Gold += amount;
        }

        /// <summary>
        /// 경험치 획득
        /// </summary>
        /// <param name="amount">경험치 획득량</param>
        public bool GainExp(int amount)
        {
            bool isLevelUP = false;
            CurExp += amount;

            // 한번에 대량 경험치 획득 대비
            while (CurExp >= MaxExp)
            {
                LevelUP();
                isLevelUP = true;
            }
            return isLevelUP;
        }

        /// <summary>
        /// Player 레벨업
        /// </summary>
        private void LevelUP()
        {
            Level++;
            CurExp -= MaxExp;

            Atk += 0.5f;
            Def++;

            MaxExp = MaxExpArray[Level - 1];
        }

        public override void Attack(Character target)
        {
            base.Attack(target);
        }

        public override void Damaged(float damage)
        {
            base.Damaged(damage);
        }

        public override void Dead()
        {
            base.Dead();
        }
    }
}
