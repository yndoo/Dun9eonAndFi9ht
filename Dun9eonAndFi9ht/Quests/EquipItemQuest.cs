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
    public class EquipItemQuest: Quest
    {
        public string RequiredItemName { get; set; }

        public EquipItemQuest(string title, string[] description, string requiredItemName, int rewardItem, int rewardMoney)
        : base(title, description, rewardItem, rewardMoney, EQuestType.EquipItem)
        {
            RequiredItemName = requiredItemName;
        }

        public override bool CheckCompletion()
        {
            return InventoryManager.Instance.EquipSlot.Values.Any(item => item.Name == RequiredItemName);
        }

        public override void ShowQuestInfo()
        {
            string itemName = " ";
            Utility.PrintScene($"{QuestTitle}\n");
            Utility.PrintScene($"목표: {itemName} 착용\n");
            Utility.PrintScene($"보상: {RewardItem}, {RewardMoney}G\n");
        }
    }
}
