using DataDefinition;
using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinition;

namespace Dun9eonAndFi9ht.Manager
{
    internal class InventoryManager
    {
        private static InventoryManager? instance;
        public static InventoryManager Instance => instance ??= new InventoryManager();
        private Player player;

        public List<Item> AllItem { get; } // 모든 아이템 리스트
        public List<Item> Inventory { get; } // 현재 갖고 있는 아이템
        public Dictionary<EItemEquipType, Item> EquipSlot { get; } // 현재 장착 아이템

        /// <summary>
        /// InventoryManager 생성자 - 전체 아이템 리스트 불러오기
        /// </summary>
        public InventoryManager()
        {
            AllItem = new List<Item>();
            Inventory = new List<Item>();

            // TO DO : 전체 아이템 리스트 불러오기 (+테스트용 지우기)
            AllItem.Add(new Item("예시아이템", EItemEquipType.Weapon, 0.1f, 1.1f, 0, 0, 20f, 0, 0)); // 테스트용
            AllItem.Add(new Item("어쩌구무기", EItemEquipType.Weapon, 0, 0, 10f, 0, 0, 0, 0)); // 테스트용
            AllItem.Add(new Item("방어구", EItemEquipType.Weapon, 0, 0, 0, 0, 0, 41.1f, 2f)); // 테스트용
            AllItem.Add(new Item("방어구", EItemEquipType.Weapon, 0, 0, 0, 0, 0, 41.1f, 2f)); // 테스트용
            AllItem.Add(new Item("어쩌구템", EItemEquipType.Weapon, 30, 0, 0, 0, 0, 0, 0)); // 테스트용

            // 테스트용으로 아이템 갖고있게 함
            Inventory.Add(AllItem[0]);
            Inventory.Add(AllItem[1]);
            Inventory.Add(AllItem[2]);
            Inventory.Add(AllItem[4]);
        }

        /// <summary>
        /// 아이템을 플레이어에게 장착
        /// </summary>
        /// <param name="selectedItem">장착할 아이템</param>
        public void ItemEquip(Item selectedItem)
        {
            EquipSlot[selectedItem.EquipType] = selectedItem;
            selectedItem.IsEquipped = true;

            player.MaxHp += selectedItem.MaxHp;
            player.Atk += selectedItem.Atk;
            player.Def += selectedItem.Def;
            // To Do : 플레이어에 모든 속성 추가 된 후 추가 작업
        }
        public void ItemUnEquip(Item selectedItem)
        {
            selectedItem.IsEquipped = false;

            player.MaxHp -= selectedItem.MaxHp;
            player.Atk -= selectedItem.Atk;
            player.Def -= selectedItem.Def;
            // To Do : 플레이어에 모든 속성 추가 된 후 추가 작업
        }
    }
}
