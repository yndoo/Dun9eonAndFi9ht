using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinition;
using Dun9eonAndFi9ht.Items;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.StaticClass;

namespace Dun9eonAndFi9ht.Scenes
{
    internal class InventoryScene : Scene
    {
        private List<Item> Inventory { get; } // 현재 갖고 있는 아이템 
        private Dictionary<EItemEquipType, Item> EquipSlot { get; } // 현재 장착 아이템
        private InventoryManager inventoryManager { get; }
        private List<Dictionary<int, int>> PotionInventory { get; } // 현재 가지고 있는 포션
        private int CurPage { get; set; }

        public InventoryScene()
        {
            inventoryManager = InventoryManager.Instance;
            Inventory = InventoryManager.Instance.Inventory;
            EquipSlot = InventoryManager.Instance.EquipSlot;
            PotionInventory = InventoryManager.Instance.PotionSlot;
        }

        public override ESceneType Start()
        {
            base.Start();
            CurPage = 1;
            // 메뉴 출력
            while (true)
            {
                Utility.ClearAll();
                Utility.PrintScene("인벤토리");
                Utility.PrintScene("이곳에서 아이템을 한 눈에 볼 수 있습니다.\n");
                DisplayInventory(CurPage);
                Utility.PrintScene($"<    {CurPage}페이지    >");

                // 포션 출력
                Utility.PrintScene("\n");
                InventoryManager.Instance.DisplayPotion(11);

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("1");
                Console.ResetColor();
                Utility.PrintMenu(". 장착관리");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("2");
                Console.ResetColor();
                Utility.PrintMenu(". 이전 페이지");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("3");
                Console.ResetColor();
                Utility.PrintMenu(". 다음 페이지");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("0");
                Console.ResetColor();
                Utility.PrintMenu(". 나가기 ");
                Utility.PrintMenu("\n\n원하시는 행동을 입력해주세요.\n>>");

                int userInput = Utility.UserInput(0, 3);
                switch(userInput)
                {
                    case 1:
                        EquipManagement();
                        return ESceneType.InventoryScene;
                    case 2:
                        CurPage = int.Max(--CurPage, 1);
                        break;
                    case 3:
                        CurPage = int.Min(++CurPage, 3);
                        break;
                    case 0:
                        return ESceneType.StartScene;
                    default:
                        Utility.ClearMenu();
                        Utility.PrintMenu("잘못된 입력입니다.");
                        Utility.PrintMenu("\n아무 키나 눌러주세요.\n>>");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// 인벤토리에 있는 아이템 보여주기
        /// </summary>
        /// <param name="page">보여줄 페이지</param>
        void DisplayInventory(int page)
        {
            int startIdx = (page - 1) * 5;
            for (int i = startIdx; i < startIdx + 5; i++)
            {
                int invenIdx = 5 * (page - 1) + i % 5;
                if (invenIdx >= Inventory.Count) break;
                Utility.PrintScene($"- {Inventory[invenIdx].ItemDisplay()}");
            }
        }

        /// <summary>
        /// 아이템 장착관리
        /// </summary>
        void EquipManagement()
        {
            // 번호 선택 시 장착
            while (true)
            {
                Utility.ClearAll();
                Utility.PrintScene("인벤토리 - 장착 관리");
                Utility.PrintScene("이곳에서 아이템을 장착하거나 해제할 수 있습니다.\n");
                for (int i = 0; i < Inventory.Count; i++)
                {
                    Utility.PrintScene($"{i + 1}. {Inventory[i].ItemDisplay()}");
                }

                Utility.PrintMenu("아이템 번호를 입력하세요.");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("0");
                Console.ResetColor();
                Utility.PrintMenu(". 나가기");
                Utility.PrintMenu("\n\n원하시는 행동을 입력해주세요.\n>>");

                int userInput = Utility.UserInput(0, Inventory.Count);
                if (userInput == 0)
                {
                    return;
                }
                else if(userInput < 0)
                {
                    Utility.ClearMenu();
                    Utility.PrintMenu("잘못된 입력입니다.");
                    Utility.PrintMenu("\n아무 키나 눌러주세요.\n>>");
                    Console.ReadKey();
                }
                else
                {
                    Equip(userInput);
                }
            }
        }
        /// <summary>
        /// 아이템 장착 / 장착 해제
        /// </summary>
        /// <param name="input">선택한 아이템 인덱스</param>
        void Equip(int input)
        {
            Item select = Inventory[input - 1];

            // 장착 중이면 장착해제
            if (select.IsEquipped)
            {
                inventoryManager.ItemUnEquip(select);
                return;
            }

            // 해당 부위 다른 아이템 장착 -> 기존 아이템 해제하고 장착 
            Item oldItem;
            if(EquipSlot.TryGetValue(select.EquipType, out oldItem))
            {
                inventoryManager.ItemUnEquip(oldItem);
                inventoryManager.ItemEquip(select);
            }
            // 해당 부위 비어있으면 장착 
            else
            {
                inventoryManager.ItemEquip(select);
            }
        }
    }
}
