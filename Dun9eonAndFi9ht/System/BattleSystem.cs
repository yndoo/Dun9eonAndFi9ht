using DataDefinition;
using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Skill;
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
        #region Variables
        private Player player;
        private List<Monster> monsterList;

        private delegate void playerTurn();
        private playerTurn playerAction;

        private bool isPlayerTurnEnd;
        private bool isPlayerRun;

        private Random random;
        #endregion

        #region Method
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

        #region Player Method
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
            DisplayPlayerActionMenu();

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
                        if (random.NextDouble() < monsterList[monsterIndex].Miss)
                        {
                            // 회피 출력
                            DisplayMissScene(player, monsterList[monsterIndex]);
                            DisplayNextInputMenu();

                        }
                        else
                        {
                            Attack(player, monsterList[monsterIndex]);
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
            int input = DisplaySelectMenu(0, player.Skills.Count, false);
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
                    int skillIndex = input - 1;
                    if (player.Skills[skillIndex].MpCost <= player.CurrentMp)
                    {
                        if (player.Skills[skillIndex].Type.Equals(ESkillTargetType.Single))
                        {
                            playerAction = (() => PlayerSkillTargetSelectPhase(skillIndex));
                        }
                        else
                        {
                            // 다중 타겟
                            List<Character> tmp = new List<Character>();
                            foreach (Monster monster in monsterList)
                            {
                                tmp.Add(monster);
                            }
                            UseSkill(player, skillIndex, tmp);
                            isPlayerTurnEnd = true;
                        }
                    }
                    else
                    {
                        // 마나 부족
                        DisplayNotEnoughManaMenu();
                        playerAction = PlayerSkillSelectPhase;
                    }
                    break;
            }
        }

        /// <summary>
        /// 단일 대상 스킬 선택 시 호출되는 메서드
        /// </summary>
        /// <param name="skillIndex">선택한 스킬 인덱스</param>
        private void PlayerSkillTargetSelectPhase(int skillIndex)
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
                        playerAction = (() => PlayerSkillTargetSelectPhase(skillIndex));
                    }
                    else
                    {
                        List<Character> tmp = new List<Character>();
                        tmp.Add(monsterList[monsterIndex]);
                        UseSkill(player, skillIndex, tmp);
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
                DisplayRunFailScene();
                DisplayNextInputMenu();
            }

            isPlayerTurnEnd = true;
        }
        #endregion

        /// <summary>
        /// 몬스턴 턴일 때 호출하는 메서드
        /// </summary>
        private void MonsterTurn()
        {
            for (int i = 0; i < monsterList.Count; i++)
            {
                if (!monsterList[i].IsDead)
                {
                    if (random.NextDouble() < player.Miss)
                    {
                        // 회피 출력
                        DisplayMissScene(monsterList[i], player);
                        DisplayNextInputMenu();
                    }
                    else
                    {
                        Attack(monsterList[i], player);
                        if (player.IsDead)
                        {
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 기본 공격을 하는 메서드
        /// </summary>
        /// <param name="attacker">공격하는 캐릭터</param>
        /// <param name="target">공격 받는 캐릭터</param>
        private void Attack(Character attacker, Character target)
        {
            float targetHP = target.CurrentHp;
            attacker.Attack(target);
            float finalAtk = attacker.FinalAtk;
            bool isCritical = false;
            // 15% 확률로 크리티컬
            if (random.NextDouble() < attacker.Crt)
            {
                finalAtk *= attacker.CrtDmg;
                isCritical = true;
            }
            target.Damaged(finalAtk);
            DisplayAttackResultScene(attacker, target, finalAtk, targetHP, isCritical);
            DisplayNextInputMenu();
        }

        /// <summary>
        /// 스킬을 사용하는 메서드
        /// </summary>
        /// <param name="caster">스킬 시전하는 캐릭터</param>
        /// <param name="index">선택한 스킬의 인덱스</param>
        /// <param name="targetList">스킬 대상 캐릭터들</param>
        private void UseSkill(Character caster, int index, List<Character> targetList)
        {
            List<Character> fianlTarget = caster.Skills[index].UseSkill(caster, targetList);

            for (int i = 0; i < fianlTarget.Count && !fianlTarget[i].IsDead; i++)
            {
                float targetHP = fianlTarget[i].CurrentHp;
                float finalAtk = caster.FinalAtk;
                fianlTarget[i].Damaged(finalAtk);
                DisplaySkillResultScene(caster, fianlTarget[i], finalAtk, targetHP, caster.Skills[index].Name);
                DisplayNextInputMenu();
            }
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
        /// <summary>
        /// 캐릭터의 정보를 씬 화면에 출력하는 메서드
        /// </summary>
        /// <param name="isTargeting">대상을 지정해야 할 경우 True 값을 넣어서 호출</param>
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
            Utility.PrintScene($"HP {player.CurrentHp:F2}/{player.MaxHp:F2}    MP {player.CurrentMp:F2}/{player.MaxMp:F2}");
        }

        /// <summary>
        /// 공격 시 씬 화면에 전투 결과를 출력하는 메서드
        /// </summary>
        /// <param name="attacker">공격하는 캐릭터</param>
        /// <param name="target">공격 받는 캐릭터</param>
        /// <param name="damage">최종 데미지</param>
        /// <param name="targetPrevHP">공격 받기 이전의 HP</param>
        /// <param name="isCritical">크리티컬 여부</param>
        private void DisplayAttackResultScene(Character attacker, Character target, float damage, float targetPrevHP, bool isCritical)
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

        /// <summary>
        /// 스킬 사용 시 씬 화면에 전투 결과를 출력하는 메서드
        /// </summary>
        /// <param name="attacker">공격하는 캐릭터</param>
        /// <param name="target">공격 받는 캐릭터</param>
        /// <param name="damage">최종 데미지</param>
        /// <param name="targetPrevHP">공격 받기 이전의 HP</param>
        /// <param name="skillName">시전한 스킬 이름</param>
        private void DisplaySkillResultScene(Character attacker, Character target, float damage, float targetPrevHP, string skillName)
        {
            Utility.ClearAll();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utility.PrintScene("Battle!!");
            Console.ResetColor();
            Utility.PrintScene("");
            Utility.PrintScene($"{attacker.Name}의 {skillName} 공격!");
            Utility.PrintScene($"{target.Name}을(를) 맞췄습니다. [데미지: {damage:F2}]");
            Utility.PrintScene("");
            Utility.PrintScene($"Lv.{target.Level} {target.Name}");
            string resultHP = target.IsDead ? "Dead" : target.CurrentHp.ToString("F2");
            Utility.PrintScene($"HP {targetPrevHP.ToString("F2")} -> {resultHP}");
        }

        /// <summary>
        /// 공격이 빗나갔을 경우 씬 화면에 출력하는 메서드
        /// </summary>
        /// <param name="attacker">공격하는 캐릭터</param>
        /// <param name="target">공격 받는 캐릭터</param>
        private void DisplayMissScene(Character attacker, Character target)
        {
            Utility.ClearAll();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utility.PrintScene("Battle!!");
            Console.ResetColor();
            Utility.PrintScene("");
            Utility.PrintScene($"{attacker.Name}의 공격!");
            Utility.PrintScene($"{target.Name}을(를) 공격했지만 아무일도 일어나지 않았습니다.");
        }

        /// <summary>
        /// 씬 화면에 사용 가능한 플레이어의 스킬 목록을 출력하는 메서드
        /// </summary>
        private void DisplaySkillListScene()
        {
            Utility.PrintScene("");
            for (int i = 0; i < player.Skills.Count; i++)
            {
                Utility.PrintScene($"{i + 1}. {player.Skills[i].Name} - MP {player.Skills[i].MpCost}");
                Utility.PrintScene($"   {player.Skills[i].Desc}");
            }
            Utility.PrintScene("0. 취소");
        }

        /// <summary>
        /// 도망가기에 실패했을 경우 씬 화면에 출력하는 메서드
        /// </summary>
        private void DisplayRunFailScene()
        {
            Utility.ClearAll();
            Utility.PrintScene("Battle!!");
            Utility.PrintScene("");
            Utility.PrintScene($"{player.Name}은(는) 도망에 실패했습니다.");
        }

        /// <summary>
        /// 플레이어 행동을 메뉴 화면에 출력하는 메서드
        /// </summary>
        private void DisplayPlayerActionMenu()
        {
            Utility.ClearMenu();
            Utility.PrintMenu("1. 공격     2. 스킬");
            Utility.PrintMenu("3. 도망가기");
        }

        /// <summary>
        /// 선택 안내를 메뉴 화면에 출력하는 메서드
        /// </summary>
        /// <param name="minIndex">선택 가능한 최소값</param>
        /// <param name="maxIndex">선택 가능한 최대값</param>
        /// <param name="isTargeting">대상을 선택해야하는 경우 True 넣어서 호출</param>
        /// <returns>사용자가 입력한 번호</returns>
        private int DisplaySelectMenu(int minIndex, int maxIndex, bool isTargeting)
        {
            string menuTxt = isTargeting ? "대상을 선택해주세요." : "원하시는 행동을 입력해주세요.";
            Utility.PrintMenu(menuTxt);
            Utility.PrintMenu(">>");
            return Utility.UserInput(minIndex, maxIndex);
        }

        /// <summary>
        /// 잘못된 입력을 했을 경우 메뉴 화면에 출력하는 메서드
        /// </summary>
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

        /// <summary>
        /// 다음으로 넘어가기 위한 안내를 메뉴 화면에 출력하는 메서드
        /// </summary>
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

        /// <summary>
        /// 스킬 선택 시 마나가 부족할 경우 메뉴 화면에 출력하는 메서드
        /// </summary>
        private void DisplayNotEnoughManaMenu()
        {
            int input = -1;
            while (input != 0)
            {
                Utility.ClearMenu();
                Utility.PrintMenu("마나가 부족합니다.");
                Utility.PrintMenu("0. 확인");
                Utility.PrintMenu("");
                Utility.PrintMenu(">>");
                input = Utility.UserInput(0, 0);
            }
        }
        #endregion
        #endregion
    }
}
