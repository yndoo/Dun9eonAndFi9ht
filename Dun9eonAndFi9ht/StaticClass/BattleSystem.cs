﻿using Dun9eonAndFi9ht.Characters;
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

        public BattleSystem(Player player, List<Monster> monsters)
        {
            this.player = player;
            this.monsterList = monsters;
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
                DisplayCharacterInfo();
                PlayerPhase();
                if (ClearCheck())
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
            bool isPlayerTurnEnd = false;
            while (!isPlayerTurnEnd)
            {
                isPlayerTurnEnd = PlayerAction();
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

        private bool PlayerAction()
        {
            Utility.ClearMenu();
            Utility.PrintMenu("1. 공격");
            Utility.PrintMenu("");
            Utility.PrintMenu("원하시는 행동을 입력해주세요.");
            Utility.PrintMenu(">>");
            int input = Utility.UserInput(1, 1);
            switch (input)
            {
                case 1:
                    return PlayerAttack();
                default:
                    return false;
            }
        }

        /// <summary>
        /// 몬스터 번호를 입력받아 해당 몬스터가 살아있으면 공격, 아니면 다시 선택
        /// </summary>
        private bool PlayerAttack()
        {
            Utility.ClearMenu();
            Utility.PrintMenu("0. 취소");
            Utility.PrintMenu("");
            Utility.PrintMenu("원하시는 행동을 입력해주세요.");
            Utility.PrintMenu(">>");
            int input = Utility.UserInput(0, monsterList.Count);
            switch (input)
            {
                case < 0:
                    Utility.ClearMenu();
                    Utility.PrintMenu("잘못된 입력입니다.");
                    Utility.PrintMenu("0. 다음");
                    Utility.PrintMenu("");
                    Utility.PrintMenu(">>");
                    Utility.UserInput(0, 0);
                    return false;
                case 0:
                    return false;
                default:
                    int monsterIndex = input - 1;
                    if (monsterList[monsterIndex].IsDead)
                    {
                        Utility.ClearMenu();
                        Utility.PrintMenu("잘못된 입력입니다.");
                        Utility.PrintMenu("0. 다음");
                        Utility.PrintMenu("");
                        Utility.PrintMenu(">>");
                        Utility.UserInput(0, 0);
                        return false;
                    }
                    else
                    {
                        Battle(player, monsterList[monsterIndex]);
                        return true;
                    }
            }
        }

        private void Battle(Character attacker, Character target)
        {
            int targetHP = target.CurrentHp;
            int finalAtk = attacker.Atk;    // 임시 구현
            attacker.Attack(target);
            target.Damaged(finalAtk);
            DisplayBattleInfo(attacker, target, finalAtk, targetHP);
        }

        /// <summary>
        /// 몬스터가 전부 사망했는지, 살아있는 몬스터가 있는지 확인하는 메서드
        /// </summary>
        /// <returns></returns>
        private bool ClearCheck()
        {
            for (int i = 0; i < monsterList.Count; i++)
            {
                if (!monsterList[i].IsDead)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 전투 정보 표시 메서드
        /// </summary>
        private void DisplayCharacterInfo()
        {
            Utility.ClearAll();
            Utility.PrintScene("Battle!!");
            Utility.PrintScene("");
            for (int i = 0; i < monsterList.Count; i++)
            {
                Utility.PrintScene($"Lv.{monsterList[i].Level} {monsterList[i].Name} HP {monsterList[i].CurrentHp}");
            }
            Utility.PrintScene("");
            Utility.PrintScene("[내 정보]");
            Utility.PrintScene($"Lv.{player.Level} {player.Name} ({player.Job})");
            Utility.PrintScene($"HP {player.CurrentHp}/{player.MaxHp}");
        }

        private void DisplayBattleInfo(Character attacker, Character target, int damage, int preHP)
        {
            Utility.ClearAll();
            Utility.PrintScene("Battle!!");
            Utility.PrintScene("");
            Utility.PrintScene($"{attacker.Name}의 공격!");
            Utility.PrintScene($"{target.Name}을(를) 맞췄습니다. [데미지: {damage}]");
            Utility.PrintScene("");
            Utility.PrintScene($"Lv.{target.Level} {target.Name}");
            string resultHP = target.IsDead ? "Dead" : target.CurrentHp.ToString();
            Utility.PrintScene($"HP {preHP.ToString()} -> {resultHP}");

            Utility.PrintMenu("0. 다음");
            Utility.PrintMenu("");
            Utility.PrintMenu(">>");
            Utility.UserInput(0, 0);
        }
    }
}
