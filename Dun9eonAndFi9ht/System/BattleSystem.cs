﻿using DataDefinition;
using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Dun9eonAndFi9ht.System
{
    public class BattleSystem
    {
        private Player player;
        private List<Monster> monsterList;

        private delegate void playerTurn();
        private playerTurn playerAction;

        private bool isPlayerTurnEnd;
        private bool isPlayerRun;

        private Random random;

        public BattleSystem(Player player, List<Monster> monsters)
        {
            this.player = player;
            monsterList = monsters;

            playerAction = PlayerActionPhase;

            isPlayerTurnEnd = false;
            isPlayerRun = false;

            random = new Random();
        }

        /// <summary>
        /// 전체적인 전투를 진행하는 메서드
        /// </summary>
        /// <returns>몬스터 전멸 시 Victory 반환
        /// 플레이어 도망가기 성공 시 Escaped 반환
        /// 플레이어 사망 시 Lose 반환</returns>
        public EDungeonResultType BattleProcess()
        {
            while (true)
            {
                isPlayerTurnEnd = false;
                PlayerTurn();
                if (IsAllMonsterDead())
                {
                    return EDungeonResultType.Victory;
                }
                else if (isPlayerRun)
                {
                    return EDungeonResultType.Escaped;
                }
                MonsterTurn();
                if (player.IsDead)
                {
                    return EDungeonResultType.Lose;
                }
            }
        }

        /// <summary>
        /// 플레이어 턴 시작 시 호출되는 메서드
        /// </summary>
        private void PlayerTurn()
        {
            playerAction = PlayerActionPhase;
            while (true)
            {
                playerAction();
                if (isPlayerTurnEnd)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 플레이어 행동 선택 메서드
        /// </summary>
        private void PlayerActionPhase()
        {
            DisplayCharaterInfoScene(false);
            Utility.ClearMenu();
            Utility.PrintMenu("1. 공격     2. 스킬");
            Utility.PrintMenu("3. 도망가기");

            int input = DisplaySelectMenu(1, 4, false);
            switch (input)
            {
                case 1:
                    playerAction = PlayerAttackPhase;
                    break;
                case 2:
                    playerAction = PlayerSkillSelectPhase;
                    break;
                case 3:
                    playerAction = PlayerRunPhase;
                    break;
                default:
                    DisplayWrongInputMenu();
                    break;
            }
        }

        /// <summary>
        /// 공격 선택 시 호출되는 메서드
        /// </summary>
        private void PlayerAttackPhase()
        {
            DisplayCharaterInfoScene(true);
            Utility.PrintScene("");
            Utility.PrintScene("0. 취소");
            int input = DisplaySelectMenu(0, monsterList.Count, true);
            switch (input)
            {
                case < 0:
                    // 잘못된 입력
                    DisplayWrongInputMenu();
                    break;
                case 0:
                    // 취소
                    playerAction = PlayerActionPhase;
                    break;
                default:
                    // 몬스터 선택
                    int monsterIndex = input - 1;
                    if (monsterList[monsterIndex].IsDead)
                    {
                        // 이미 죽은 몬스터
                        DisplayWrongInputMenu();
                        playerAction = PlayerAttackPhase;
                    }
                    else
                    {
                        // 10% 확률로 미스
                        if (random.Next(100) < Constants.MISS_RATE/*monsterList[monsterIndex].Miss*/)
                        {
                            // 회피 출력
                            DisplayMissAttackInfoScene(player, monsterList[monsterIndex]);
                            DisplayNextInputMenu();

                        }
                        else
                        {
                            Battle(player, monsterList[monsterIndex]);
                        }
                        isPlayerTurnEnd = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 스킬 선택 시 호출되는 메서드
        /// </summary>
        private void PlayerSkillSelectPhase()
        {
            DisplayCharaterInfoScene(false);
            DisplaySkillListScene();
            int input = DisplaySelectMenu(0, 2, false);
            switch (input)
            {
                case < 0:
                    // 잘못된 입력
                    DisplayWrongInputMenu();
                    break;
                case 0:
                    // 취소
                    playerAction = PlayerActionPhase;
                    break;
                default:
                    // 스킬 선택
                    playerAction = PlayerTargetSelectPhase;
                    break;
            }
        }

        private void PlayerTargetSelectPhase()
        {
            DisplayCharaterInfoScene(true);
            Utility.PrintScene("");
            Utility.PrintScene("0. 취소");
            int input = DisplaySelectMenu(0, monsterList.Count, true);
            switch (input)
            {
                case < 0:
                    // 잘못된 입력
                    DisplayWrongInputMenu();
                    break;
                case 0:
                    // 취소
                    playerAction = PlayerSkillSelectPhase;
                    break;
                default:
                    // 몬스터 선택
                    int monsterIndex = input - 1;
                    if (monsterList[monsterIndex].IsDead)
                    {
                        // 이미 죽은 몬스터
                        DisplayWrongInputMenu();
                        playerAction = PlayerTargetSelectPhase;
                    }
                    else
                    {
                        // 스킬 사용 임시 구현
                        Battle(player, monsterList[monsterIndex]);
                        isPlayerTurnEnd = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// 도망가기 선택 시 호출되는 메서드
        /// </summary>
        private void PlayerRunPhase()
        {
            int totalMonsterLevel = 0;
            for (int i = 0; i < monsterList.Count; i++)
            {
                totalMonsterLevel += monsterList[i].Level;
            }

            isPlayerRun = random.Next(100) > totalMonsterLevel ? true : false;

            if (!isPlayerRun)
            {
                DisplayRunScene();
                DisplayNextInputMenu();
            }

            isPlayerTurnEnd = true;
        }

        /// <summary>
        /// 몬스턴 턴일 때 ㅅ호출하는 메서드
        /// </summary>
        private void MonsterTurn()
        {
            for (int i = 0; i < monsterList.Count; i++)
            {
                if (!monsterList[i].IsDead)
                {
                    if (random.Next(100) < 10/*player.Miss*/)
                    {
                        // 회피 출력
                        DisplayMissAttackInfoScene(monsterList[i], player);
                        DisplayNextInputMenu();
                    }
                    else
                    {
                        Battle(monsterList[i], player);
                        if (player.IsDead)
                        {
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 실제 전투를 하는 메서드
        /// </summary>
        /// <param name="attacker">공격하는 캐릭터</param>
        /// <param name="target">공격 받는 캐릭터</param>
        private void Battle(Character attacker, Character target)
        {
            float targetHP = target.CurrentHp;
            attacker.Attack(target);
            float finalAtk = attacker.FinalAtk;
            bool isCritical = false;
            // 15% 확률로 크리티컬
            if (random.Next(100) < Constants.CRITICAL_RATE/*attacker.Critical*/)
            {
                finalAtk *= Constants.CRITICAL_DAMAGE_RATE;
                isCritical = true;
            }
            target.Damaged(finalAtk);
            DisplayBattleInfoScene(attacker, target, finalAtk, targetHP, isCritical);
            DisplayNextInputMenu();
        }

        /// <summary>
        /// 몬스터가 전부 사망했는지, 살아있는 몬스터가 있는지 확인하는 메서드
        /// </summary>
        /// <returns>True: 몬스터 전멸 False: 몬스터 존재</returns>
        private bool IsAllMonsterDead()
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

        #region Display Method
        private void DisplayCharaterInfoScene(bool isTargeting)
        {
            Utility.ClearAll();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utility.PrintScene("Battle!!");
            Console.ResetColor();
            Utility.PrintScene("");
            for (int i = 0; i < monsterList.Count; i++)
            {
                string monsterState = monsterList[i].IsDead ? "Dead" : $"HP {monsterList[i].CurrentHp:F2}";
                string index = isTargeting ? $"{i + 1}번 " : "";
                Utility.PrintScene($"{index}Lv.{monsterList[i].Level} {monsterList[i].Name} {monsterState}");
            }
            Utility.PrintScene("");
            Utility.PrintScene("[내 정보]");
            Utility.PrintScene($"Lv.{player.Level} {player.Name} ({player.Job})");
            Utility.PrintScene($"HP {player.CurrentHp:F2}/{player.MaxHp:F2}");
        }

        private void DisplayBattleInfoScene(Character attacker, Character target, float damage, float targetPrevHP, bool isCritical)
        {
            Utility.ClearAll();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utility.PrintScene("Battle!!");
            Console.ResetColor();
            Utility.PrintScene("");
            Utility.PrintScene($"{attacker.Name}의 공격!");
            string criticalTxt = isCritical ? " - 치명타 공격!!" : "";
            Utility.PrintScene($"{target.Name}을(를) 맞췄습니다. [데미지: {damage:F2}]{criticalTxt}");
            Utility.PrintScene("");
            Utility.PrintScene($"Lv.{target.Level} {target.Name}");
            string resultHP = target.IsDead ? "Dead" : target.CurrentHp.ToString("F2");
            Utility.PrintScene($"HP {targetPrevHP.ToString("F2")} -> {resultHP}");
        }

        private void DisplayMissAttackInfoScene(Character attacker, Character target)
        {
            Utility.ClearAll();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utility.PrintScene("Battle!!");
            Console.ResetColor();
            Utility.PrintScene("");
            Utility.PrintScene($"{attacker.Name}의 공격!");
            Utility.PrintScene($"{target.Name}을(를) 공격했지만 아무일도 일어나지 않았습니다.");
        }

        private void DisplaySkillListScene()
        {
            Utility.PrintScene("");
            //for (int i = 0; i < player.skillList; i++)
            //{

            //}
            // 임시 출력
            Utility.PrintScene("1. 알파 스트라이크 - MP 10");
            Utility.PrintScene("   공격력 * 2 로 하나의 적을 공격합니다.");
            Utility.PrintScene("2. 더블 스트라이크 - MP 15");
            Utility.PrintScene("   공격력 * 1.5 로 2명의 적을 랜덤으로 공격합니다.");
            Utility.PrintScene("0. 취소");
        }

        private void DisplayRunScene()
        {
            Utility.ClearAll();
            Utility.PrintScene("Battle!!");
            Utility.PrintScene("");
            Utility.PrintScene($"{player.Name}은(는) 도망에 실패했습니다.");
        }

        private int DisplaySelectMenu(int minIndex, int maxIndex, bool isTargeting)
        {
            string menuTxt = isTargeting ? "대상을 선택해주세요." : "원하시는 행동을 입력해주세요.";
            Utility.PrintMenu(menuTxt);
            Utility.PrintMenu(">>");
            return Utility.UserInput(minIndex, maxIndex);
        }

        private void DisplayWrongInputMenu()
        {
            int input = -1;
            while (input != 0)
            {
                Utility.ClearMenu();
                Utility.PrintMenu("잘못된 입력입니다.");
                Utility.PrintMenu("0. 확인");
                Utility.PrintMenu("");
                Utility.PrintMenu(">>");
                input = Utility.UserInput(0, 0);
            }
        }

        private void DisplayNextInputMenu()
        {
            int input = -1;
            while (input != 0)
            {
                Utility.ClearMenu();
                Utility.PrintMenu("0. 다음");
                Utility.PrintMenu("");
                Utility.PrintMenu(">>");
                input = Utility.UserInput(0, 0);
            }
        }
        #endregion
    }
}
