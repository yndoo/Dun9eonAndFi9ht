using DataDefinition;
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

        public BattleSystem(Player player, List<Monster> monsters)
        {
            this.player = player;
            monsterList = monsters;
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
                // 플레이어 턴
                PlayerPhase();
                // 몬스터 전멸 검사
                if (IsAllMonsterDead())
                {
                    return player.IsDead;
                }
                // 몬스터 턴
                MonsterPhase();
                // 플레이어 생존 검사
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
        /// 한 번 전투 한 후 바로 플레이어 생존 검사
        /// </summary>
        private void MonsterPhase()
        {
            for (int i = 0; i < monsterList.Count; i++)
            {
                if (!monsterList[i].IsDead)
                {
                    Random random = new Random();
                    if (random.Next(100) < 10)
                    {
                        // 회피 출력
                        DisplayMissAttackInfo(monsterList[i], player);
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
        /// 플레이어 턴에 호출하는 플레이어 행동 메서드
        /// </summary>
        /// <returns>행동 입력 시 True 반환
        /// 잘못된 입력 시 False 반환</returns>
        private bool PlayerAction()
        {
            bool isPlayerActionEnd = false;
            while (!isPlayerActionEnd)
            {
                DisplayCharacterInfo();
                Utility.ClearMenu();
                Utility.PrintMenu("1. 공격");
                Utility.PrintMenu("");
                Utility.PrintMenu("원하시는 행동을 입력해주세요.");
                Utility.PrintMenu(">>");
                int input = Utility.UserInput(1, 1);
                switch (input)
                {
                    case 1:
                        isPlayerActionEnd = PlayerAttack();
                        break;
                    default:
                        isPlayerActionEnd = DisplayWrongInput();
                        break;
                }
            }
            return isPlayerActionEnd;
        }

        /// <summary>
        /// 공격 선택 시 호출하는 메서드
        /// 몬스터 번호를 입력받아 해당 몬스터가 살아있으면 공격, 아니면 다시 선택
        /// </summary>
        /// <returns>몬스터 선택 시 True 반환
        /// 취소 입력 시 False 반환</returns>
        private bool PlayerAttack()
        {
            bool isPlayerAttackEnd = false;
            while (!isPlayerAttackEnd)
            {
                for (int i = 0; i < monsterList.Count; i++)
                {
                    Utility.PrintFree($"{i + 1}번 Lv.{monsterList[i].Level} {monsterList[i].Name} HP {monsterList[i].CurrentHp:F2}/{monsterList[i].MaxHp:F2}", i + 2);
                }
                Utility.ClearMenu();
                Utility.PrintMenu("0. 취소");
                Utility.PrintMenu("");
                Utility.PrintMenu("대상을 선택해주세요.");
                Utility.PrintMenu(">>");
                int input = Utility.UserInput(0, monsterList.Count);
                switch (input)
                {
                    case < 0:
                        isPlayerAttackEnd = DisplayWrongInput();
                        break;
                    case 0:
                        return false;
                    default:
                        int monsterIndex = input - 1;
                        if (monsterList[monsterIndex].IsDead)
                        {
                            isPlayerAttackEnd = DisplayWrongInput();
                        }
                        else
                        {
                            Random random = new Random();
                            // 10% 확률로 미스
                            if (random.Next(100) < Constants.MISS_RATE)
                            {
                                // 회피 출력
                                DisplayMissAttackInfo(player, monsterList[monsterIndex]);
                            }
                            else
                            {
                                Battle(player, monsterList[monsterIndex]);
                            }
                            isPlayerAttackEnd = true;
                        }
                        break;
                }
            }
            return isPlayerAttackEnd;
        }

        /// <summary>
        /// 스킬 선택 시 호출하는 메서드
        /// 몬스터 번호를 입력받아 해당 몬스터가 살아있으면 공격, 아니면 다시 선택
        /// </summary>
        /// <returns>몬스터 선택 시 True 반환
        /// 취소 입력 시 False 반환</returns>
        private bool PlayerSkillSelect()
        {
            bool isPlayerSkillSelectEnd = false;
            while (!isPlayerSkillSelectEnd)
            {

            }
            return isPlayerSkillSelectEnd;
        }

        /// <summary>
        /// 공격 대상에게 공격을 하는 메서드
        /// </summary>
        /// <param name="attacker">공격하는 캐릭터</param>
        /// <param name="target">공격받는 캐릭터</param>
        private void Battle(Character attacker, Character target)
        {
            float targetHP = target.CurrentHp;
            attacker.Attack(target);
            float finalAtk = attacker.FinalAtk;
            Random random = new Random();
            bool isCritical = false;
            // 15% 확률로 크리티컬
            if (random.Next(100) < Constants.CRITICAL_RATE)
            {
                finalAtk *= Constants.CRITICAL_DAMAGE_RATE;
                isCritical = true;
            }
            target.Damaged(finalAtk);
            DisplayBattleInfo(attacker, target, finalAtk, targetHP, isCritical);
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

        /// <summary>
        /// 화면 상단 전투 정보 표시 메서드
        /// </summary>
        private void DisplayCharacterInfo()
        {
            Utility.ClearAll();
            Utility.PrintScene("Battle!!");
            Utility.PrintScene("");
            for (int i = 0; i < monsterList.Count; i++)
            {
                string dead = monsterList[i].CurrentHp == 0 ? "Dead" : "";
                Utility.PrintScene($"Lv.{monsterList[i].Level} {monsterList[i].Name} HP {monsterList[i].CurrentHp:F2}/{monsterList[i].MaxHp:F2} {dead}");
            }
            Utility.PrintScene("");
            Utility.PrintScene("[내 정보]");
            Utility.PrintScene($"Lv.{player.Level} {player.Name} ({player.Job})");
            Utility.PrintScene($"HP {player.CurrentHp:F2}/{player.MaxHp:F2}");
        }

        /// <summary>
        /// 화면 상단 공격 시 전투 정보를 출력하는 메서드
        /// </summary>
        /// <param name="attacker">공격하는 캐릭터</param>
        /// <param name="target">공격 받는 캐릭터</param>
        /// <param name="damage">최종 공격 데미지</param>
        /// <param name="targetPrevHP">공격 받는 캐릭터의 공격 받기 이전의 HP</param>
        private void DisplayBattleInfo(Character attacker, Character target, float damage, float targetPrevHP, bool isCritical)
        {
            Utility.ClearAll();
            Utility.PrintScene("Battle!!");
            Utility.PrintScene("");
            Utility.PrintScene($"{attacker.Name}의 공격!");
            string criticalTxt = isCritical ? " - 치명타 공격!!" : "";
            Utility.PrintScene($"{target.Name}을(를) 맞췄습니다. [데미지: {damage:F2}]{criticalTxt}");
            Utility.PrintScene("");
            Utility.PrintScene($"Lv.{target.Level} {target.Name}");
            string resultHP = target.IsDead ? "Dead" : target.CurrentHp.ToString("F2");
            Utility.PrintScene($"HP {targetPrevHP.ToString("F2")} -> {resultHP}");

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

        /// <summary>
        /// 공격 회피 시 화면 상단에 전투 정보를 출력하는 메서드
        /// </summary>
        /// <param name="attacker">공격하는 캐릭터</param>
        /// <param name="target">공격 받는 캐릭터</param>
        private void DisplayMissAttackInfo(Character attacker, Character target)
        {
            Utility.ClearAll();
            Utility.PrintScene("Battle!!");
            Utility.PrintScene("");
            Utility.PrintScene($"{attacker.Name}의 공격!");
            Utility.PrintScene($"{target.Name}을(를) 공격했지만 아무일도 일어나지 않았습니다.");

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

        /// <summary>
        /// 잘못된 입력 시 화면 하단에 출력하는 메서드
        /// </summary>
        /// <returns>0번 선택 시 True
        /// 잘못 입력 시 False</returns>
        private bool DisplayWrongInput()
        {
            bool isCorrectInput = false;
            while (!isCorrectInput)
            {
                Utility.ClearMenu();
                Utility.PrintMenu("잘못된 입력입니다.");
                Utility.PrintMenu("0. 확인");
                Utility.PrintMenu("");
                Utility.PrintMenu(">>");
                int nextInput = Utility.UserInput(0, 0);
                isCorrectInput = nextInput == 0 ? true : false;
            }
            return !isCorrectInput;
        }
    }
}
