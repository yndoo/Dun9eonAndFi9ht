using DataDefinition;
using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Quests
{
    public class ReachLevelQuest : Quest
    {
        public int TargetLevel { get; private set; }

        public ReachLevelQuest(string title, string[] description, int rewardItem, int rewardMoney, int targetLevel) 
            :base(title, description, rewardItem, rewardMoney, EQuestType.ReachLevel)
        {
            TargetLevel = targetLevel;
        }

        public override bool CheckCompletion()
        {
            return GameManager.Instance.Player.Level >= TargetLevel; // 플레이어 레벨이 목표 도달했는지 확인
        }

        public override void ShowQuestInfo()
        {
            Utility.PrintScene($"{QuestTitle}\n");
            Utility.PrintScene($"목표: 레벨 {TargetLevel} 도달 (현재: {GameManager.Instance.Player.Level})\n");
            Utility.PrintScene($"보상: {RewardItem}, {RewardMoney}G\n");
        }
    }
}
