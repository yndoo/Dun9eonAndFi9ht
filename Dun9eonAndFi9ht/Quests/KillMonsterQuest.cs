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

        public void AddKillCount(string monsterName)
        {
           if (monsterName == TargetMonster)
            {
                CurrentKills++;
                if(CurrentKills >= NeedKills)
                {
                    IsCleared = true;
                    Utility.PrintScene($"'{QuestTitle}' 퀘스트 목표 달성!");
                }
                //QuestManager.Instance.
            }
        }

        public override bool CheckCompletion()
        {
            return IsCleared;
        }

        public override void ShowQuestInfo()
        {
            Utility.PrintSceneW($"- {TargetMonster} ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{NeedKills}");
            Console.ResetColor();
            Utility.PrintScene("마리 처치 (");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{CurrentKills}");
            Console.ResetColor();
            Utility.PrintSceneW("/");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW($"{NeedKills}");
            Console.ResetColor();
            Utility.PrintScene(")\n");

            ShowSelect();
        }

    }
}
