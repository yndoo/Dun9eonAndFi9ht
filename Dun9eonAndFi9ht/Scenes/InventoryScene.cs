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

        public InventoryScene()
        {
            inventoryManager = InventoryManager.Instance;
            Inventory = InventoryManager.Instance.Inventory;
            EquipSlot = InventoryManager.Instance.EquipSlot;
        }

        public override ESceneType Start()
        {
            base.Start();
            DisplayInventory();

            // To Do : 출력
            // 1. 장착관리
            // 2. 나가기

            int input = Utility.UserInput(1, 2);
            if (input == 1) 
            { 
                EquipManagement();
                return ESceneType.InventoryScene;
            }
            else return ESceneType.StartScene;
        }

        /// <summary>
        /// 인벤토리에 있는 아이템 보여주기
        /// </summary>
        void DisplayInventory()
        {
            for(int i = 0; i < Inventory.Count; i++)
            {
                Utility.PrintScene($"{Inventory[i].ItemDisplay()}");
            }
        }

        /// <summary>
        /// 아이템 장착관리
        /// </summary>
        void EquipManagement()
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                Utility.PrintScene($"{i + 1}. {Inventory[i].ItemDisplay()}");
            }
            // TO DO : 번호 선택시 아이템 장착 / 장착해제
 
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

            // 해당 부위 && 다른 아이템 장착 -> 기존 아이템 해제하고 장착 
            Item oldItem;
            if(EquipSlot.TryGetValue(select.EquipType, out oldItem))
            {
                inventoryManager.ItemUnEquip(oldItem);
                inventoryManager.ItemEquip(select);
            }
        }
    }
}
