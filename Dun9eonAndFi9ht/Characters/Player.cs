using DataDefinition;
using Dun9eonAndFi9ht.StaticClass;

namespace Dun9eonAndFi9ht.Characters
{
    public class Player : Character
    {
        public EJobType Job { get; set; }
        public int Gold { get; set; }
        public int MaxExp { get; set; }
        public int CurExp { get; set; }
        public Player(string name, EJobType job, int maxHp, int atk, int def, int level, int gold) : base(name, maxHp, atk, def, level)
        {
            this.Job = job;
            this.Gold = gold;
            this.CurExp = 0;
            
            // 예제 값 (레벨업 필요 경험치)
            this.MaxExp = 100; 
        }

        /// <summary>
        /// 플레이어 스탯 살펴보기
        /// </summary>
        public void DisplayStatus()
        {
            Utility.PrintScene($"Lv. {Level:D2}");
            Utility.PrintScene($"{Name} ( {GetJobName(Job)} )");
            Utility.PrintScene($"{"공격력"} : {Atk}");
            Utility.PrintScene($"{"방어력"} : {Def}");
            Utility.PrintScene($"{"체  력"} : {CurrentHp}");
            Utility.PrintScene($"{"Gold"}   : {Gold}");
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

        public override void Attack(Character target)
        {
            base.Attack(target);
        }

        public override void Damaged(int damage)
        {
            base.Damaged(damage);
        }

        public override void Dead()
        {
            base.Dead();
        }
    }
}
