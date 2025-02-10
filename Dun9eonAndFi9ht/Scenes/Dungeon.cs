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
        private bool isPlayerLose;
        private int MonsterTypeCount;
        public Player Player { get; set; }
        public List<Monster> MonsterList { get; set; }
        public bool IsPlayerWin { get; set; }

        public Dungeon()
        {
            DataTableManager.Instance.Initialize("../../../DataBase");
            MonsterTypeCount = 3;

            MonsterList = new List<Monster>(MonsterTypeCount);
        }

        /// <summary>
        /// 몬스터 랜덤 배치, 플레이어 데이터 가져오기
        /// </summary>
        private void EnterDungeon()
        {
            Player = GameManager.Instance.Player;

            //Monster 데이터 가져오기
            DataTableManager dtManager = DataTableManager.Instance;
            if (MonsterList!=null&&MonsterList.Count != 0) MonsterList.Clear();

            int j = 1;
            for (int i = 0; i < MonsterTypeCount; i++)
            {
                try
                {
                    Dictionary<string, object> lst = dtManager.GetDBData($"enemy_stage{j}", i);
                    MonsterList.Add(new Monster(lst["name"].ToString(), Convert.ToInt32(lst["maxHp"]), Convert.ToInt32(lst["atk"]), Convert.ToInt32(lst["def"]), Convert.ToInt32(lst["level"]), Convert.ToInt32(lst["gold"])));
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

            float hpBeforeDungeon = Player.CurrentHp;

            Random random = new Random();
            int rMonsterCnt = random.Next(1, 4);
            BattleSystem battleSystem;
            battleSystem = new BattleSystem(Player, MonsterList.OrderBy(x => random.Next(0, rMonsterCnt)).ToList());
            isPlayerLose = battleSystem.BattleProcess();

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
            Utility.PrintScene("Battle!! - Result");

            Utility.PrintScene("");
            Utility.PrintScene(isPlayerLose ? "You Lose" : "Victory");
            Utility.PrintScene("");
            if (!isPlayerLose)
            {
                Utility.PrintScene($"던전에서 몬스터 {MonsterList.Count}마리를 잡았습니다.");
                Utility.PrintScene("");
            }
            Utility.PrintScene($"Lv.{Player.Level} {Player.Name}");
            Utility.PrintScene($"HP {hpBeforeDungeon:F2} -> {Player.CurrentHp:F2}");

            while (true)
            {
                Utility.ClearMenu();
                Utility.PrintMenu("0. 나가기");
                int userInput = Utility.UserInput(0, 0);
                if (userInput == 0)
                {
                    return ESceneType.StartScene;
                }
                int nextInput = -1;
                while (nextInput != 0)
                {
                    Utility.ClearMenu();
                    Utility.PrintMenu("잘못된 입력입니다.");
                    Utility.PrintMenu("0. 확인");
                    Utility.PrintMenu("");
                    Utility.PrintMenu(">>");
                    nextInput = Utility.UserInput(0, 0);
                }
            }
        }
    }
}
