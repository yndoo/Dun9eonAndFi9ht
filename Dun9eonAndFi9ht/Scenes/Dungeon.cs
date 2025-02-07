using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.StaticClass;
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
        public Player Player { get; set; }
        public List<Monster> MonsterList { get; set; }
        public bool IsPlayerWin { get; set; }

        /// <summary>
        /// 몬스터 랜덤 배치, 플레이어 데이터 가져오기
        /// </summary>
        private void EnterDungeon()
        {
            //player = GameManager.Instance.Player;

            //Monster 데이터 가져오기 예시
            // To Do : 기능 생긴 뒤에 수정해야 함
            //for(int i = 0; i < MonsterCount; i++)
            //{
            //    monsterList.Add(new Monster());
            //    monsterList[i].Name = GameManager.GetMonsterData[i][0];
            //    monsterList[i].MaxHp = GameManager.GetMonsterData[i][1];
            //}
        }

        /// <summary>
        /// Dungeon 시작 함수
        /// </summary>
        public override void Start()
        {
            base.Start();
            Utility.PrintScene("Battle!!");
            EnterDungeon();

            int hpBeforeDungeon = player.CurrentHp;

            // To Do : BattleTurn 호출
            Random random = new Random();
            //isPlayerWin = BattleSystem.BattleTurn(Player, MonsterList.OrderBy(x => random.Next(0, 3)).ToList());

            // 전투 결과 출력
            ResultScreen(hpBeforeDungeon);
        }

        /// <summary>
        /// 던전 결과 출력
        /// </summary>
        /// <param name="hpBeforeDungeon">던전 입장 전 플레이어 체력</param>
        private void ResultScreen(int hpBeforeDungeon)
        {
            Utility.ClearScene();
            Utility.PrintScene("Battle!! - Result");

            // To Do : 배틀 결과 (MonsterCount 생긴뒤에 주석 해제)
            Utility.PrintScene("");
            Utility.PrintScene(isPlayerWin ? "Victory" : "You Lose");
            Utility.PrintScene("");
            if (isPlayerWin)
            {
                // Utility.PrintScene($"던전에서 몬스터 {MonsterCount}마리를 잡았습니다.");
                Utility.PrintScene("");
            }
            Utility.PrintScene($"Lv.{player.Level} {Player.Name}");
            Utility.PrintScene($"HP {hpBeforeDungeon} -> {Player.CurrentHp}");

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
