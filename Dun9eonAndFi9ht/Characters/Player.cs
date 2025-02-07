using DataDefinition;
using Dun9eonAndFi9ht.StaticClass;

namespace Dun9eonAndFi9ht.Characters
{
    internal class Player : Character
    {
        private int gold;
        private EJobType job;
        private int maxExp;
        private int curExp;

        public int Gold { get; set; }
        public EJobType Job { get; set; }
        public int MaxExp { get; set; }
        public int CurExp { get; set; }
        public Player(string name, EJobType job, int maxHp, int atk, int def, int level, int gold) : base(name, maxHp, atk, def, level)
        {
            this.job = job;
            this.gold = gold;
            this.curExp = 0;
            
            // 예제 값 (레벨업 필요 경험치)
            this.maxExp = 100; 
        }

        /// <summary>
        /// 플레이어 스탯 살펴보기
        /// </summary>
        public void DisplayStatus()
        {
            Utility.ClearScene();
            Utility.PrintScene($"Lv. {level:D2}");
            Utility.PrintScene($"{Name} ( {GetJobName(job)} )");
            Utility.PrintScene($"{"공격력"} : {atk}");
            Utility.PrintScene($"{"방어력"} : {def}");
            Utility.PrintScene($"{"체  력"} : {currentHp}");
            Utility.PrintScene($"{"Gold"}   : {gold}");
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
