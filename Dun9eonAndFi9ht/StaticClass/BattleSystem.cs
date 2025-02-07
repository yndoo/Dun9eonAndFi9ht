using Dun9eonAndFi9ht.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.StaticClass
{
    public class BattleSystem
    {
        private Player player;
        private List<Monster> monsterList;
        private bool isAllMonsterDead;

        public BattleSystem(Player player, List<Monster> monsters)
        {
            this.player = player;
            this.monsterList = monsters;
            isAllMonsterDead = false;
        }

        /// <summary>
        /// Battle 시작 시 사용할 메서드
        /// 전투가 지속되는 동안 PlayerPhase와 MonsterPhase 계속 반복
        /// </summary>
        /// <returns>플레이어의 사망 시 Ture 반환
        /// 모든 몬스터 처치 시 False 반환</returns>
        public bool BattleProcess()
        {
            while (true)
            {
                PlayerPhase();
                if (isAllMonsterDead)
                {
                    return player.IsDead;
                }

                MonsterPhase();
                if (player.IsDead)
                {
                    return player.IsDead;
                }
            }
        }

        /// <summary>
        /// 플레이어 턴에 실행되는 메서드
        /// </summary>
        private void PlayerPhase()
        {
            int input = Utility.UserInput(1, 1);
            switch (input)
            {
                case 1:
                    // 1. 공격 선택
                    Attack();
                    break;
            }
        }

        /// <summary>
        /// 몬스터 턴에 실행되는 메서드
        /// </summary>
        private void MonsterPhase()
        {
            for (int i = 0; i < monsterList.Count; i++)
            {
                if (!monsterList[i].IsDead)
                {
                    Battle(monsterList[i], player);
                }
            }
        }

        /// <summary>
        /// 몬스터 번호를 입력받아 해당 몬스터가 살아있으면 공격, 아니면 다시 선택
        /// </summary>
        private void Attack()
        {
            int input = Utility.UserInput(0, monsterList.Count);
            switch (input)
            {
                case < 0:
                    /* 일치하는 몬스터를 선택하지 않았다면
                         * 잘못된 입력입니다 출력
                         */
                    break;
                case 0:
                    // 0. 취소 선택
                    break;
                default:
                    int monsterIndex = input - 1;
                    if (monsterList[monsterIndex].IsDead)
                    {
                        /* 이미 죽은 몬스터를 공격했다면
                         * 잘못된 입력입니다 출력
                         */
                    }
                    else
                    {
                        Battle(player, monsterList[monsterIndex]);
                    }
                    break;
            }

            // 몬스터 전멸 여부 검사
        }

        private void Battle(Character attacker, Character target)
        {

        }
    }
}
