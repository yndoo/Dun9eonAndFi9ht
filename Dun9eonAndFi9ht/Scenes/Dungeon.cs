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
            Console.WriteLine("Battle!!");
            EnterDungeon();

            Random random = new Random();
            // To Do : BattleTurn 기능 생긴 뒤에 수정해야 함
            //isPlayerWin = BattleSystem.BattleTurn(Player, MonsterList.OrderBy(x => random.Next(0, 3)).ToList());

            // To Do : Result 
            if(isPlayerWin == true)
            {

            }
            else
            {

            }
        }
    }
}
