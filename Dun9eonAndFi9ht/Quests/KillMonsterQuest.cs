using DataDefinition;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Quests
{
    public class KillMonsterQuest: Quest
    {
        public string TargetMonster { get; set; }
        public int NeedKills { get; set; } // 잡아야 할 몬스터 수
        public int CurrentKills { get; set; } // 현재 잡은 몬스터 수

        public KillMonsterQuest(string title, string[] description, int rewardItem, int rewardMoney, string targetMonster, int needKills)
         : base(title, description, rewardItem, rewardMoney, EQuestType.KillMonster)
        {
            this.TargetMonster = targetMonster;
            this.NeedKills = needKills;
            this.CurrentKills = 0;
        }
        /// <summary>
        /// 퀘스트 킬 카운트를 세는 함수
        /// </summary>
        /// <param name="monsterName"></param>
        public void AddKillCount(string monsterName)
        {
           if (HasAccepted && monsterName == TargetMonster)
            {
                CurrentKills++;
                Utility.PrintScene($"[{QuestTitle}] {TargetMonster} 처치 수: {CurrentKills}/{NeedKills}");

                if (CurrentKills >= NeedKills)
                {
                    Utility.PrintScene($"'{QuestTitle}' 퀘스트 목표 달성!");
                }
            }
        }
        /// <summary>
        /// 퀘스트 완료 확인 함수
        /// </summary>
        /// <returns></returns>
        public override bool CheckCompletion()
        {
            return CurrentKills >= NeedKills;
        }

        public override void ResetProgress()
        {
            CurrentKills = 0;
        }

        /// <summary>
        /// 퀘스트 정보 출력 함수
        /// </summary>
        public override void ShowQuestInfo()
        {
            Utility.ClearAll();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utility.PrintScene("Quest!!");
            Console.ResetColor();
            Utility.PrintScene(" ");

            Utility.PrintScene($"{QuestTitle}"+(IsCleared ? "(수행 완료)" : ""));
            Utility.PrintScene(" ");
            Utility.PrintScene(QuestDescription);
            Utility.PrintScene(" ");
            Utility.PrintSceneW($"- {TargetMonster} ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{NeedKills}");
            Console.ResetColor();
            Utility.PrintSceneW("마리 처치 (");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{CurrentKills}");
            Console.ResetColor();
            Utility.PrintSceneW("/");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{NeedKills}");
            Console.ResetColor();
            Utility.PrintScene(")");
            Utility.PrintScene(" ");


            Utility.PrintScene($"보상: {InventoryManager.Instance.GetItemNameById(RewardItem)}, {RewardMoney}G\n");

            ShowSelect();
        }

    }
}
