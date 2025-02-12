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
        /// <summary>
        /// 퀘스트 완료 확인 메소드
        /// </summary>
        /// <returns></returns>
        public override bool CheckCompletion()
        {
            return InventoryManager.Instance.EquipSlot.Values.Any(item => item.Name == RequiredItemName);
        }
        /// <summary>
        /// 퀘스트 정보 확인 메소드
        /// </summary>
        public override void ShowQuestInfo()
        {
            Utility.ClearAll();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utility.PrintScene("Quest!!");
            Console.ResetColor();
            Utility.PrintScene(" ");

            Utility.PrintScene($"{QuestTitle}");
            Utility.PrintScene(" ");
            Utility.PrintScene(QuestDescription);
            Utility.PrintScene(" ");

           

            Utility.PrintScene($"목표: {RequiredItemName} 착용\n");
            Utility.PrintScene($"보상: {InventoryManager.Instance.GetItemNameById(RewardItem)}, {RewardMoney}G\n");

            ShowSelect();
        }
        /// <summary>
        /// 아이템을 가지고 있는지 확인하는 함수
        /// </summary>
        /// <returns></returns>
        private string HasItem()
        {
            return InventoryManager.Instance.Inventory.Any(item => item.Name == RequiredItemName) ? "O" : "X";
        }
    }
}
