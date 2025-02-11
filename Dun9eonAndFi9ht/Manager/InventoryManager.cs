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
            EquipSlot = new Dictionary<EItemEquipType, Item>();

            // 아이템 정보 불러오기
            for (int i = 0; i < 6; i ++)
            {
                Dictionary<string, object> itemInfo = DataTableManager.Instance.GetDBData("item", i);
                string name = itemInfo["Name"].ToString();
                EItemEquipType type = (EItemEquipType)(Convert.ToInt32(itemInfo["EquipType"]));
                float maxHp     = Convert.ToSingle(itemInfo["MaxHp"]);
                float maxMp     = Convert.ToSingle(itemInfo["MaxMp"]);
                float atk       = Convert.ToSingle(itemInfo["Atk"]);
                float def       = Convert.ToSingle(itemInfo["Def"]);
                float critRate  = Convert.ToSingle(itemInfo["CriticalRate"]);
                float critDmg   = Convert.ToSingle(itemInfo["CriticalDamage"]);
                float missRate  = Convert.ToSingle(itemInfo["MissRate"]);
                AllItem.Add(new Item(name, type, maxHp, maxMp, atk, def, critRate, critDmg, missRate));
            }

            // 인벤토리&아이템 테스트용 코드
            //GrantItem(0);
            //GrantItem(1);
            //GrantItem(2);
            //GrantItem(3);
            //GrantItem(4);
            //GrantItem(5);
        }

        /// <summary>
        /// 플레이어에게 아이템 보상을 지급
        /// </summary>
        /// <param name="itemNum">지급할 아이템 번호</param>
        public void GrantItem(int itemNum)
        {
            try
            {
                Inventory.Add(AllItem[itemNum]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{itemNum}번 아이템이 존재하지 않습니다 : {ex.Message}");
                Console.WriteLine("아무 키 입력");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// 아이템을 플레이어에게 장착
        /// </summary>
        /// <param name="selectedItem">장착할 아이템</param>
        public void ItemEquip(Item selectedItem)
        {
            player = GameManager.Instance.Player;
            EquipSlot[selectedItem.EquipType] = selectedItem;
            selectedItem.IsEquipped = true;

            player.MaxHp += selectedItem.MaxHp;
            player.MaxMp += selectedItem.MaxMp;
            player.Atk += selectedItem.Atk;
            player.Def += selectedItem.Def;
            player.Crt += selectedItem.CriticalRate;
            player.CrtDmg += selectedItem.CriticalDamage;
            player.Miss += selectedItem.MissRate;
        }
        /// <summary>
        /// 아이템을 장착 해제
        /// </summary>
        /// <param name="selectedItem">장착 해제할 아이템</param>
        public void ItemUnEquip(Item selectedItem)
        {
            player = GameManager.Instance.Player;
            selectedItem.IsEquipped = false;

            player.MaxHp -= selectedItem.MaxHp;
            player.MaxMp -= selectedItem.MaxMp;
            player.Atk -= selectedItem.Atk;
            player.Def -= selectedItem.Def;
            player.Crt -= selectedItem.CriticalRate;
            player.CrtDmg -= selectedItem.CriticalDamage;
            player.Miss -= selectedItem.MissRate;
        }
    }
}
