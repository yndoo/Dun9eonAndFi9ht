namespace Dun9eonAndFi9ht.Characters
{
    enum EJobType
    {
        None,
        Warrior
    }
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

        /// <summary>
        /// 플레이어 스탯 살펴보기
        /// </summary>
        public void DisplayStatus()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("상태 보기");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv. {Level:D2}");
            Console.WriteLine($"{Name} ( {GetJobName(Job)} )");
            Console.WriteLine($"{"공격력"} : {Atk}");
            Console.WriteLine($"{"방어력"} : {Def}");
            Console.WriteLine($"{"체  력"} : {CurrentHp}");
            Console.WriteLine($"{"Gold"}   : {Gold}");
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
