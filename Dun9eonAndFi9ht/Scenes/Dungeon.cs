using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.StaticClass;
using DataDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dun9eonAndFi9ht.System;

namespace Dun9eonAndFi9ht.Scenes
{
    internal class Dungeon : Scene
    {
        private EDungeonResultType resultType;
        private int MonsterTypeCount;
        public Player Player { get; set; }
        public List<Monster> MonsterList { get; set; }
        public bool IsPlayerWin { get; set; }
        
        public static int stage { get; set; }

        public static int maxStageCleared { get; set; }

        
        public Dungeon()
        {
            DataTableManager.Instance.Initialize("../../../DataBase");
            MonsterTypeCount = 3;
            stage = 1;
            maxStageCleared = 0;

            MonsterList = new List<Monster>(MonsterTypeCount);
        }

        /// <summary>
        /// 몬스터 랜덤 배치, 플레이어 데이터 가져오기
        /// </summary>
        private void EnterDungeon()
        {
            Player = GameManager.Instance.Player;
            CheckPotionUse();
            //Monster 데이터 가져오기
            DataTableManager dtManager = DataTableManager.Instance;
            if (MonsterList!=null&&MonsterList.Count != 0) MonsterList.Clear();

            for (int i = 0; i < MonsterTypeCount; i++)
            {
                try
                {
                    Dictionary<string, object> lst = dtManager.GetDBData($"enemy_stage{stage}", i);
                    string monsterTypeStr = lst["type"].ToString();
                    EMonsterType monsterType = (EMonsterType)Enum.Parse(typeof(EMonsterType), monsterTypeStr);
                    MonsterList.Add(new Monster(lst["name"].ToString(), Convert.ToInt32(lst["maxHp"]), Convert.ToInt32(lst["maxMp"]), Convert.ToInt32(lst["atk"]), Convert.ToInt32(lst["def"]), Convert.ToInt32(lst["level"]), Convert.ToInt32(lst["gold"]), monsterType));
                }   
                catch (Exception ex)
                {
                    Utility.PrintMenu($"\n{i} 인덱스 Monster 데이터 로드 오류 : {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Dungeon 시작 함수
        /// </summary>
        public override ESceneType Start()
        {
            base.Start();
            EnterDungeon();

            Utility.PrintSceneW("현재 스테이지: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{Dungeon.stage}");
            Console.ResetColor();
            float hpBeforeDungeon = Player.CurrentHp;

            Random random = new Random();
            int rMonsterCnt = random.Next(1, 4);
            BattleSystem battleSystem;
            battleSystem = new BattleSystem(Player, MonsterList.OrderBy(x => random.Next(0, rMonsterCnt)).ToList());

            // To Do : BattleProcess()의 return 값을 EDungeonResultType으로 변환해야함.
            resultType = battleSystem.BattleProcess();

            // 전투 결과 출력
            return ResultScreen(hpBeforeDungeon);
        }

        /// <summary>
        /// 던전 결과 출력
        /// </summary>
        /// <param name="hpBeforeDungeon">던전 입장 전 플레이어 체력</param>
        private ESceneType ResultScreen(float hpBeforeDungeon)
        {
            Utility.ClearScene();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utility.PrintScene("Battle!! - Result");
            Console.ResetColor();


            // 몬스터 보상 합산
            Reward sumReward = new Reward
            {
                exp = MonsterList.Sum(m => m.Reward.exp),
                gold = MonsterList.Sum(m => m.Reward.gold)
            };

            if (resultType == EDungeonResultType.Victory)
            {
                GainItem(sumReward);
                QuestManager.Instance.CheckQuests();
            }
            DisplayDungeonResult(hpBeforeDungeon, sumReward);

            while (true)
            {
                Utility.ClearMenu();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("0");
                Console.ResetColor();
                Utility.PrintMenu(". 나가기");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("1");
                Console.ResetColor();
                Utility.PrintMenu(". 다시 던전으로...");

                int userInput = Utility.UserInput(0, 1);
                if (userInput == 0)
                {
                    return ESceneType.StartScene;
                }
                if(userInput == 1)
                {
                    return ESceneType.Dungeon;
                }
                int nextInput = -1;
                while (nextInput != 0)
                {
                    Utility.ClearMenu();
                    Utility.PrintMenu("잘못된 입력입니다.");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintMenuW("0");
                    Console.ResetColor();
                    Utility.PrintMenu(". 확인");
                    Utility.PrintMenu("");
                    Utility.PrintMenu(">>");
                    nextInput = Utility.UserInput(0, 1);
                }
            }
        }

        /// <summary>
        /// 던전 결과 출력 핵심 기능
        /// </summary>
        /// <param name="hpBeforeDungeon">던전 입장 전 플레이어 체력</param>
        private void DisplayDungeonResult(float hpBeforeDungeon, Reward sumReward)
        {
            Utility.PrintScene("");

            switch (resultType)
            {
                case EDungeonResultType.Victory:
                    Utility.PrintScene("Victory");
                    Utility.PrintScene("");
                    Utility.PrintSceneW("던전에서 몬스터 ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintSceneW($"{MonsterList.Count}");
                    Console.ResetColor();
                    Utility.PrintScene("마리를 잡았습니다.");
                    StageClear();
                    break;

                case EDungeonResultType.Lose:
                    Utility.PrintScene("You Lose");
                    break;

                case EDungeonResultType.Escaped:
                    Utility.PrintScene("Escape");
                    Utility.PrintScene("");
                    Utility.PrintScene("무사히 도망에 성공했습니다.");
                    break;
            }
            Utility.PrintScene("");

            // 캐릭터 정보 출력
            Utility.PrintScene("[캐릭터 정보]");
            int prevLevel = Player.Level;
            int prevExp = Player.CurExp;

            // 경험치 획득
            if (resultType == EDungeonResultType.Victory)
            {
                bool isLevelUp = Player.GainExp(sumReward.exp);
                if (isLevelUp)
                {
                    Utility.PrintSceneW("Lv.");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintSceneW($"{prevLevel} ");
                    Console.ResetColor();
                    Utility.PrintSceneW($"{Player.Name} -> Lv. ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintSceneW($"{Player.Level}");
                    Console.ResetColor();
                    Utility.PrintScene($" {Player.Name}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintSceneW($"{Player.Level}");
                    Console.ResetColor();
                    Utility.PrintScene($" {Player.Name}");
                }
            }
            
            Utility.PrintSceneW("HP ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{hpBeforeDungeon:F2}");
            Console.ResetColor();
            Utility.PrintSceneW(" -> ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{Player.CurrentHp:F2}");
            Console.ResetColor();

            if (resultType == EDungeonResultType.Victory)
            {
                Utility.PrintSceneW("EXP");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW($"{prevExp}");
                Console.ResetColor();
                Utility.PrintSceneW(" -> ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintScene($"{sumReward.exp + prevExp}");
                Console.ResetColor();
            }

            // 획득한 보상 출력
            if (resultType == EDungeonResultType.Victory)
            {
                Utility.PrintScene("");
                Utility.PrintScene("[획득 아이템]");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW($"{sumReward.gold}");
                Console.ResetColor();
                Utility.PrintScene(" Gold");
            }
            Utility.PrintScene("");

        }

        /// <summary>
        /// 던전 결과 아이템 획득
        /// </summary>
        /// <param name="sumReward">던전 보상</param>
        private void GainItem(Reward sumReward)
        {
            Player.GainGold(sumReward.gold);
        }
        /// <summary>
        /// 스테이지 클리어 처리 메시지 출력
        /// </summary>
        private void StageClear()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{stage}");
            Console.ResetColor();
            Utility.PrintScene("층 클리어!");
            if (stage<5)
            {
                if (maxStageCleared <= stage)
                {
                    maxStageCleared = stage;
                }
                stage++;
            }
        }

        private void CheckPotionUse()
        {
            try
            {
                bool isFirst = true;
                int input = -1;
                while (input != 0)
                {
                    Utility.ClearAll();
                    Utility.PrintScene(isFirst ? "던전 입장 전, 포션을 사용하시겠습니까?" : "추가로 포션을 사용하시겠습니까?");
                    InventoryManager.Instance.DisplayPotion(2);
                    Utility.PrintMenu("원하시는 포션을 선택 해주세요 (0. 나가기)");
                    Utility.PrintMenuW(">>> ");
                    List<Dictionary<int, int>> map = InventoryManager.Instance.PotionSlot;

                    input = Utility.UserInput(0, map.Count);
                    if (input > 0 && input <= map.Count)
                    {
                        bool result = InventoryManager.Instance.UsePotion(map[input - 1].Keys.First(), Player);
                        isFirst = false;
                    }
                    else if (input == 0)
                    {
                        Utility.ClearAll();
                        Utility.PrintScene("전투를 시작합니다.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Utility.PrintScene("올바르지 않은 입력입니다.");
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine("오류" + e);
            }
        }
    }
}
