using DataDefinition;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.Scenes;
using Dun9eonAndFi9ht.Skill;
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
            this.MaxExp = MaxExpArray[level - 1];

            // 직업별 스킬 자동 세팅
            Skills = SkillManager.Instance.GetPlayerSkills(Job); 
        }

        /// <summary>
        /// 플레이어 스탯 살펴보기
        /// </summary>
        public void DisplayStatus()
        {
            Utility.PrintSceneW("Lv. ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{Level:D2}");
            Console.ResetColor();
            Utility.PrintScene($"{Name} ( {GetJobName(Job)} )");
            Utility.PrintSceneW("공격력  :  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{Atk:F2}");
            Console.ResetColor();
            Utility.PrintSceneW("방어력  :  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{Def:F2}");
            Console.ResetColor();
            Utility.PrintSceneW("체  력  :  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{CurrentHp:F2}");
            Console.ResetColor();
            Utility.PrintSceneW("마  나  :  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{CurrentMp}");
            Console.ResetColor();
            Utility.PrintSceneW("Gold   :  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{Gold}");
            Console.ResetColor();
            if (MaxExp < 0)
            {
                Utility.PrintScene($"{"EXP"}    : 최고레벨입니다.");
            }
            else
            {
                Utility.PrintSceneW("EXP  :  ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW($"{CurExp}");
                Console.ResetColor();
                Utility.PrintSceneW(" / ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintScene($"{MaxExp}");
                Console.ResetColor();
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

            CurrentHp = MaxHp;
            CurrentMp = MaxMp;
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
            InventoryManager.Instance.ClearInventory();
            QuestManager.Instance.InitializeQuest();
            Dungeon.InitializeStage();
            base.Dead();
        }
    }
}
