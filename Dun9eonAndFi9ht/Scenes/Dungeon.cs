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
using Dun9eonAndFi9ht.Items;

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

        /// <summary>
        /// 죽었을 때 스테이지 초기화
        /// </summary>
        public static void InitializeStage()
        {
            stage = 1;
        }

        
        public Dungeon()
        {
            DataTableManager.Instance.Initialize("../../../DataBase");
            MonsterTypeCount = 3;
            stage = 1;

            MonsterList = new List<Monster>(MonsterTypeCount);
        }

        /// <summary>
        /// 몬스터 랜덤 배치, 플레이어 데이터 가져오기
        /// </summary>
        private void EnterDungeon()
        {
            Player = GameManager.Instance.Player;
            CheckPotionUse();
            if (MonsterList!=null&&MonsterList.Count != 0) MonsterList.Clear();
            AddMonster();
        }

        /// <summary>
        /// 몬스터를 생성하여 MonsterList에 넣기
        /// </summary>
        private void AddMonster()
        {
            for (int i = 0; i < MonsterTypeCount; i++)
            {
                try
                {
                    //Monster 데이터 가져오기
                    DataTableManager dtManager = DataTableManager.Instance;
                    Dictionary<string, object> lst = dtManager.GetDBData($"enemy_stage{stage}", i);
                    string monsterTypeStr = lst["type"].ToString();
                    EMonsterType monsterType = (EMonsterType)Enum.Parse(typeof(EMonsterType), monsterTypeStr);

                    List<int> dropItemIDs = new List<int>();
                    string[] dropItemIDArray = lst["dropItemIDs"].ToString().Split(',');
                    foreach (string id in dropItemIDArray)
                    {
                        if (int.TryParse(id, out int itemID))
                        {
                            dropItemIDs.Add(itemID);
                        }
                    }

                    List<int> dropPotionIDs = new List<int>();
                    string[] dropPotionIDArray = lst["dropPotionIDs"].ToString().Split(',');
                    foreach (string id in dropPotionIDArray)
                    {
                        if (int.TryParse(id, out int potionID))
                        {
                            dropPotionIDs.Add(potionID);
                        }
                    }

                    MonsterList.Add(new Monster(lst["name"].ToString(),
                        Convert.ToInt32(lst["maxHp"]),
                        Convert.ToInt32(lst["maxMp"]),
                        Convert.ToInt32(lst["atk"]),
                        Convert.ToInt32(lst["def"]),
                        Convert.ToInt32(lst["level"]),
                        Convert.ToInt32(lst["gold"]),
                        monsterType,
                        dropItemIDs,
                        dropPotionIDs));
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
                gold = MonsterList.Sum(m => m.Reward.gold),
                dropItemIDs = MonsterList.SelectMany(m => m.Reward.dropItemIDs).ToList(),
                dropPotionIDs = MonsterList.SelectMany(m => m.Reward.dropPotionIDs).ToList()
            };

            if (resultType == EDungeonResultType.Victory)
            {
                GainItem(sumReward);
                QuestManager.Instance.CheckQuests();
            }

            DisplayDungeonResult(hpBeforeDungeon, sumReward);

            bool isDead = GameManager.Instance.Player.CurrentHp <= 0;
            bool isEscaped = resultType == EDungeonResultType.Escaped;

            // 결과 메시지를 단 한 번만 출력하도록 변경
            Utility.ClearMenu();
            if (isDead)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Utility.PrintScene("You Died");
                Console.ResetColor();
                Utility.PrintScene("\n당신은 전투에서 패배했습니다...");
                Utility.PrintScene("모든 HP를 소진하여 전투를 지속할 수 없습니다.");
            }
            else if (isEscaped)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Utility.PrintScene("몬스터로부터 무사히 도망치는 데 성공했습니다.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Utility.PrintScene("층 클리어!");
                Console.ResetColor();
                Utility.PrintScene("계속 던전을 진행하시겠습니까?");
            }

           

            while (true)
            {
                Utility.ClearMenu();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("0");
                Console.ResetColor();
                Utility.PrintMenu(". 나가기");

                if (!isDead && !isEscaped) // 도망친 경우에는 던전으로 돌아가기 버튼 제거
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintMenuW("1");
                    Console.ResetColor();
                    Utility.PrintMenu(". 다시 던전으로...");
                }

                Utility.PrintMenu(">> ");
                int userInput = isDead || isEscaped ? Utility.UserInput(0, 0) : Utility.UserInput(0, 1);

                if (userInput == 0)
                {
                    return ESceneType.StartScene;
                }
                if (userInput == 1 && !isDead && !isEscaped)
                {
                    return ESceneType.Dungeon;
                }

                Utility.ClearMenu();
                Utility.PrintMenu("잘못된 입력입니다.");
                Utility.PrintMenu("");
                Utility.PrintMenu("아무 키나 눌러주세요.");
                Utility.PrintMenu(">>");
                Console.ReadKey();
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
                    Utility.PrintSceneW("Lv.");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintSceneW($"{Player.Level}");
                    Console.ResetColor();
                    Utility.PrintScene($" {Player.Name}");
                }
            }
            else
            {
                Utility.PrintSceneW("Lv.");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW($"{Player.Level}");
                Console.ResetColor();
                Utility.PrintScene($" {Player.Name}");
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
                Utility.PrintSceneW("EXP ");
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

                var groupedItems = sumReward.dropItemIDs.GroupBy(id => id);
                var groupedPotions = sumReward.dropPotionIDs.GroupBy(id => id);


                bool firstItem = true;
                foreach (var group in groupedItems)
                {
                    string itemName = InventoryManager.Instance.AllItem[group.Key].Name;
                    int itemCount = group.Count();
                    if (!firstItem)
                    {
                        Utility.PrintSceneW(" / "); // 두 번째 아이템부터 '/' 추가
                    }
                    firstItem = false;
                    Utility.PrintSceneW($"{itemName} - ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintSceneW($"{itemCount}");
                    Console.ResetColor();
                }

                foreach (var group in groupedPotions)
                 {
                    var potion = InventoryManager.Instance.GetPotionById(group.Key);
                    if (potion == null) continue; // 존재하지 않는 포션이면 건너뛰기

                    string potionName = potion.name;
                    int potionCount = group.Count();

                    if (!firstItem)
                    {
                        Utility.PrintSceneW(" / ");
                    }
                    firstItem = false;

                    Utility.PrintSceneW($"{potionName} - ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintSceneW($"{potionCount}");
                    Console.ResetColor();
                 }
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
            foreach (int id in sumReward.dropItemIDs)
            {
                InventoryManager.Instance.GrantItem(id);
            }
            foreach (int id in sumReward.dropPotionIDs)
            {
                InventoryManager.Instance.GrantPotion(id);
            }
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
                if (Player.MaxStageCleared <= stage)
                {
                    Player.MaxStageCleared = stage;
                }
                stage++;
            }
        }
        /// <summary>
        /// 스테이지 시작 전 포션 사용 질문 메소드
        /// </summary>
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
                    Utility.PrintMenuW("원하시는 포션을 선택 해주세요. (");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintMenuW("0.");
                    Console.ResetColor();
                    Utility.PrintMenu(" 나가기)");
                    Utility.PrintMenuW(">>> ");
                    List<Dictionary<int, int>> map = InventoryManager.Instance.PotionSlot;  

                    input = Utility.UserInput(0, map.Count);
                    if (input > 0 && input <= map.Count)
                    {
                        int index = input - 1;
                        bool result = InventoryManager.Instance.UsePotion(index, Player);
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

                        Utility.ClearMenu();
                        Utility.PrintMenu("잘못된 입력입니다.");
                        Utility.PrintMenu("");
                        Utility.PrintMenu("아무 키나 눌러주세요.");
                        Utility.PrintMenu(">>");
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
