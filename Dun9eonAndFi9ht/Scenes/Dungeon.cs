using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.StaticClass;
using DataDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Scenes
{
    internal class Dungeon : Scene
    {
        private Player player;
        private List<Monster> monsterList;
        private bool isPlayerWin;
        private int MonsterTypeCount;
        public Player Player { get; set; }
        public List<Monster> MonsterList { get; set; }
        public bool IsPlayerWin { get; set; }

        public Dungeon()
        {
            DataTableManager.Instance.Initialize("../../../DataBase");
            MonsterTypeCount = 3; 

            monsterList = new List<Monster>(MonsterTypeCount);
        }

        /// <summary>
        /// 몬스터 랜덤 배치, 플레이어 데이터 가져오기
        /// </summary>
        private void EnterDungeon()
        {
            player = GameManager.Instance.Player;

            //Monster 데이터 가져오기
            DataTableManager dtManager = DataTableManager.Instance;
            for (int i = 0; i < MonsterTypeCount; i++)
            {
                try
                {
                    List<string> lst = dtManager.GetMonsterData("enemy", i);
                    monsterList.Add(new Monster(lst[1], int.Parse(lst[2]), int.Parse(lst[3]), int.Parse(lst[4]), int.Parse(lst[5]), int.Parse(lst[6]), int.Parse(lst[7])));
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
        public override void Start()
        {
            base.Start();
            Utility.PrintScene("Battle!!");
            EnterDungeon();

            // To Do : BattleTurn 호출
            Random random = new Random();
            //isPlayerWin = BattleSystem.BattleTurn(Player, MonsterList.OrderBy(x => random.Next(0, 3)).ToList());

            // 전투 결과 출력
            ResultScreen();
            GameManager.Instance.LoadScene(ESceneType.StartScene);
        }

        private void ResultScreen()
        {
            Utility.ClearScene();
            Utility.PrintScene("Battle!! - Result");

            // To Do : 배틀 결과
            

            Utility.ClearMenu();
            Utility.PrintMenu("0. 나가기");
            while (true)
            {
                int userInput = Utility.UserInput(0, 0);
                if (userInput == 0) return;
                Utility.PrintMenu("잘못된 입력입니다.");
            }
        }
    }
}
