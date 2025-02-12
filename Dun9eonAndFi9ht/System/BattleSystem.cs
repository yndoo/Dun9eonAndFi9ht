using DataDefinition;
using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.Quests;
using Dun9eonAndFi9ht.Scenes;
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
                    playerAction = PlayerPotionSelectPhase;
                    break;
                case 4:
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
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW("0");
            Console.ResetColor();
            Utility.PrintScene(". 취소");
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
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW("0");
            Console.ResetColor();
            Utility.PrintScene(". 취소");
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

        private void PlayerPotionSelectPhase()
        {
            Utility.ClearAll();
            DisplayCharaterInfoScene(false);
            InventoryManager.Instance.DisplayPotion(1);

            Utility.PrintScene("");
            Utility.PrintScene("사용할 포션을 선택하세요.");
            Utility.PrintScene("");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW("0");
            Console.ResetColor();
            Utility.PrintScene(". 취소");

            List<Dictionary<int, int>> map = InventoryManager.Instance.PotionSlot;

            if (map.Count == 0)
            {
                Utility.PrintScene("사용 가능한 포션이 없습니다.");
                Console.ReadKey();
                playerAction = PlayerActionPhase;
                return;
            }

            int input = Utility.UserInput(0, map.Count);

            if (input == 0)
            {
                playerAction = PlayerActionPhase;
                return;
            }

            if (input > 0 && input <= map.Count)
            {
                int slotIndex = input - 1;
                bool result = InventoryManager.Instance.UsePotion(slotIndex, player);

                if (!result)
                {
                    Utility.PrintScene("포션을 사용할 수 없습니다.");
                }
                else
                {
                    Utility.PrintScene("포션을 사용하였습니다.");
                    isPlayerTurnEnd = true; // ✅ 포션 사용 후 턴 종료
                }
            }
            else
            {
                Utility.PrintScene("잘못된 입력입니다.");
                Console.ReadKey();
            }

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
                    // 60% 확률로 스킬 사용
                    if (random.NextDouble() < 0.6f)
                    {
                        MonsterSkill(i);
                    }
                    else
                    {
                        MonsterAttack(i);
                    }

                    if (player.IsDead)
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 몬스터 공격 메서드
        /// </summary>
        /// <param name="monsterIndex">공격하는 몬스터 인덱스</param>
        private void MonsterAttack(int monsterIndex)
        {
            if (random.NextDouble() < player.Miss)
            {
                // 회피 출력
                DisplayMissScene(monsterList[monsterIndex], player);
                DisplayNextInputMenu();
            }
            else
            {
                Attack(monsterList[monsterIndex], player);
            }
        }

        /// <summary>
        /// 몬스터 스킬 사용 메서드
        /// </summary>
        /// <param name="monsterIndex">스킬 시전하는 몬스터 인덱스</param>
        private void MonsterSkill(int monsterIndex)
        {
            int skillIndex = random.Next(0, monsterList[monsterIndex].Skills.Count);
            if (monsterList[monsterIndex].Skills[skillIndex].MpCost <= monsterList[monsterIndex].CurrentMp)
            {
                List<Character> tmp = new List<Character>();
                tmp.Add(player);
                UseSkill(monsterList[monsterIndex], skillIndex, tmp);
            }
            else
            {
                MonsterAttack(monsterIndex);
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
            if (random.NextDouble() < attacker.Crt * 0.01f)
            {
                finalAtk = finalAtk * (1 + attacker.CrtDmg * 0.01f);
                isCritical = true;
            }
            float finalDMG = Math.Max(finalAtk - (target.Def * 0.5f), 1);
            target.Damaged(finalDMG);
            DisplayAttackResultScene(attacker, target, finalDMG, targetHP, isCritical);

            if (target.IsDead && target is Monster monster)
            {
                UpdateMonsterKillQuests(monster);
            }

            DisplayNextInputMenu();
        }

        /// <summary>
        /// 스킬을 사용하는 메서드
        /// </summary>
        /// <param name="caster">스킬 시전하는 캐릭터</param>
        /// <param name="skillIndex">선택한 스킬의 인덱스</param>
        /// <param name="targetList">스킬 대상 캐릭터들</param>
        private void UseSkill(Character caster, int skillIndex, List<Character> targetList)
        {
            List<Character> fianlTarget = caster.Skills[skillIndex].UseSkill(caster, targetList);

            for (int i = 0; i < fianlTarget.Count && !fianlTarget[i].IsDead; i++)
            {
                float targetHP = fianlTarget[i].CurrentHp;
                float finalAtk = caster.FinalAtk;
                float finalDMG = Math.Max(finalAtk - (fianlTarget[i].Def * 0.5f), 1);
                fianlTarget[i].Damaged(finalDMG);
                DisplaySkillResultScene(caster, fianlTarget[i], finalDMG, targetHP, caster.Skills[skillIndex].Name);
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
            Utility.PrintScene($"Battle!! - {Dungeon.stage}층");
            Console.ResetColor();
            Utility.PrintScene("");
            for (int i = 0; i < monsterList.Count; i++)
            {
                string monsterState = monsterList[i].IsDead ? "Dead" : $"{monsterList[i].CurrentHp:F2}";
                string index = isTargeting ? $"{i + 1}번 " : "";
                Console.ForegroundColor = ConsoleColor.Blue;
                Utility.PrintSceneW($"{index}");
                Console.ResetColor();
                Utility.PrintSceneW(" Lv. ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW($"{monsterList[i].Level} ");
                Console.ResetColor();
                Utility.PrintSceneW($"{monsterList[i].Name} HP ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintScene($"{monsterState}");
                Console.ResetColor();
            }
            Utility.PrintScene("");
            Utility.PrintScene("[내 정보]");
            Utility.PrintSceneW("Lv. ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{player.Level}");
            Console.ResetColor();
            Utility.PrintScene($" {player.Name} ({player.Job})");
            Utility.PrintSceneW("HP ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{player.CurrentHp:F2}");
            Console.ResetColor();
            Utility.PrintSceneW(" / ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{player.MaxHp:F2}");
            Console.ResetColor();
            Utility.PrintSceneW("    MP ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{player.CurrentMp}");
            Console.ResetColor();
            Utility.PrintSceneW(" / ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{player.MaxMp}");
            Console.ResetColor();
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
            Utility.PrintScene($"Battle!! - {Dungeon.stage}층");
            Console.ResetColor();
            Utility.PrintScene("");
            Utility.PrintScene($"{attacker.Name}의 공격!");
            string criticalTxt = isCritical ? " - 치명타 공격!!" : "";
            Utility.PrintSceneW($"{target.Name}을(를) 맞췄습니다. 데미지: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{damage:F2} {criticalTxt}");
            Console.ResetColor();
            Utility.PrintScene("");
            Utility.PrintSceneW("Lv.");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{target.Level}");
            Console.ResetColor();
            Utility.PrintScene($" {target.Name}");
            string resultHP = target.IsDead ? "Dead" : target.CurrentHp.ToString("F2");
            Utility.PrintSceneW("HP ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{targetPrevHP.ToString("F2")}");
            Console.ResetColor();
            Utility.PrintSceneW(" -> ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{resultHP}");
            Console.ResetColor();
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
            Utility.PrintScene($"Battle!! - {Dungeon.stage}층");
            Console.ResetColor();
            Utility.PrintScene("");
            Utility.PrintScene($"{attacker.Name}의 {skillName} 공격!");
            Utility.PrintSceneW($"{target.Name}을(를) 맞췄습니다. 데미지: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{damage:F2}");
            Console.ResetColor();
            Utility.PrintScene("");
            Utility.PrintSceneW("Lv. ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{target.Level}");
            Console.ResetColor();
            Utility.PrintScene($" {target.Name}");
            string resultHP = target.IsDead ? "Dead" : target.CurrentHp.ToString("F2");
            Utility.PrintSceneW("HP ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{targetPrevHP.ToString("F2")}");
            Console.ResetColor();
            Utility.PrintSceneW(" -> ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintScene($"{resultHP}");
            Console.ResetColor();
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
            Utility.PrintScene($"Battle!! - {Dungeon.stage}층");
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
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW($"{i + 1}");
                Console.ResetColor();
                Utility.PrintSceneW($". {player.Skills[i].Name} - MP ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintScene($"{player.Skills[i].MpCost}");
                Console.ResetColor();
                Utility.PrintScene($"   {player.Skills[i].Desc}");
            }
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW("0");
            Console.ResetColor();
            Utility.PrintScene(". 취소");
        }

        /// <summary>
        /// 도망가기에 실패했을 경우 씬 화면에 출력하는 메서드
        /// </summary>
        private void DisplayRunFailScene()
        {
            Utility.ClearAll();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utility.PrintScene($"Battle!! - {Dungeon.stage}층");
            Console.ResetColor();
            Utility.PrintScene("");
            Utility.PrintScene($"{player.Name}은(는) 도망에 실패했습니다.");
        }

        /// <summary>
        /// 플레이어 행동을 메뉴 화면에 출력하는 메서드
        /// </summary>
        private void DisplayPlayerActionMenu()
        {
            Utility.ClearMenu();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintMenuW("1");
            Console.ResetColor();
            Utility.PrintMenuW(". 공격          ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintMenuW("2");
            Console.ResetColor();
            Utility.PrintMenu(". 스킬");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintMenuW("3");
            Console.ResetColor();
            Utility.PrintMenuW(". 포션 사용     ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintMenuW("4");
            Console.ResetColor();
            Utility.PrintMenu(". 도망가기");
            Utility.PrintMenu("");
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
            Utility.ClearMenu();
            Utility.PrintMenu("잘못된 입력입니다.");
            Utility.PrintMenu("");
            Utility.PrintMenu("아무 키나 눌러주세요.");
            Utility.PrintMenu(">>");
            Console.ReadKey();
        }

        /// <summary>
        /// 다음으로 넘어가기 위한 안내를 메뉴 화면에 출력하는 메서드
        /// </summary>
        private void DisplayNextInputMenu()
        {
            Utility.ClearMenu();
            Utility.PrintMenu("다음");
            Utility.PrintMenu("");
            Utility.PrintMenu("아무 키나 눌러주세요.");
            Utility.PrintMenu(">>");
            Console.ReadKey();
        }

        /// <summary>
        /// 스킬 선택 시 마나가 부족할 경우 메뉴 화면에 출력하는 메서드
        /// </summary>
        private void DisplayNotEnoughManaMenu()
        {
            Utility.ClearMenu();
            Utility.PrintMenu("마나가 부족합니다.");
            Utility.PrintMenu("");
            Utility.PrintMenu("아무 키나 눌러주세요.");
            Utility.PrintMenu(">>");
            Console.ReadKey();
        }
        /// <summary>
        /// 퀘스트 진행 상태 업데이트 (몬스터 처치 퀘스트)
        /// </summary>
        private void UpdateMonsterKillQuests(Monster monster)
        {
            foreach (var quest in QuestManager.Instance.GetAcceptedQuests().OfType<KillMonsterQuest>())
            {
                quest.AddKillCount(monster.Name);
            }
        }

        #endregion
        #endregion
    }
}
