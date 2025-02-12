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
using Dun9eonAndFi9ht.StaticClass;

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

        public List<Dictionary<int, int>> PotionSlot { get; set; }     //현재 갖고 있는 포션


        private int itemCount; // 아이템 전체 개수

        /// <summary>
        /// InventoryManager 생성자 - 전체 아이템 리스트 불러오기 
        /// </summary>
        public InventoryManager()
        {
            AllItem = new List<Item>();
            Inventory = new List<Item>();
            EquipSlot = new Dictionary<EItemEquipType, Item>();
            PotionSlot = new List <Dictionary<int, int>>();
            itemCount = 11;
            // 아이템 정보 불러오기
            
            for (int i = 0; i < itemCount; i++)
            {
                Dictionary<string, object> itemInfo = DataTableManager.Instance.GetDBData("item", i);
                string name = itemInfo["Name"].ToString();
                EItemEquipType type = (EItemEquipType)(Convert.ToInt32(itemInfo["EquipType"]));
                float maxHp = Convert.ToSingle(itemInfo["MaxHp"]);
                float maxMp = Convert.ToSingle(itemInfo["MaxMp"]);
                float atk = Convert.ToSingle(itemInfo["Atk"]);
                float def = Convert.ToSingle(itemInfo["Def"]);
                float critRate = Convert.ToSingle(itemInfo["CriticalRate"]);
                float critDmg = Convert.ToSingle(itemInfo["CriticalDamage"]);
                float missRate = Convert.ToSingle(itemInfo["MissRate"]);
                AllItem.Add(new Item(name, type, maxHp, maxMp, atk, def, critRate, critDmg, missRate));
            }

            GrantItem(0);
            // 인벤토리&아이템 테스트용 코드
            for(int i = 0; i < itemCount; i++)
            {
                GrantItem(i);
            }

            GrantPotion(0);
            GrantPotion(1);
            GrantPotion(2);
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

            QuestManager.Instance.CheckQuests();
        }
        /// <summary>
        /// 아이템을 장착 해제
        /// </summary>
        /// <param name="selectedItem">장착 해제할 아이템</param>
        public void ItemUnEquip(Item selectedItem)
        {
            player = GameManager.Instance.Player;
            selectedItem.IsEquipped = false;
            EquipSlot.Remove(selectedItem.EquipType);

            player.MaxHp -= selectedItem.MaxHp;
            player.MaxMp -= selectedItem.MaxMp;
            player.Atk -= selectedItem.Atk;
            player.Def -= selectedItem.Def;
            player.Crt -= selectedItem.CriticalRate;
            player.CrtDmg -= selectedItem.CriticalDamage;
            player.Miss -= selectedItem.MissRate;
        }

        /// <summary>
        /// 특정 ID의 포션을 지급
        /// </summary>
        public void GrantPotion(int potionID, int amount = 1)
        {
            foreach(var slot in PotionSlot)
            {
                if(slot.ContainsKey(potionID))
                {
                    slot[potionID] += amount;
                    return;
                }
            }
            PotionSlot.Add(new Dictionary<int, int> { { potionID, amount } });
        }

        /// <summary>
        /// 특정 ID의 포션을 사용
        /// </summary>
        public bool UsePotion(int slotIndex, Character character)
        {
            if (slotIndex < 0 || slotIndex >= PotionSlot.Count) return false;

            var potionEntry = PotionSlot[slotIndex];
            int potionID = potionEntry.Keys.First();
            Potion potion = GetPotionById(potionID);

            potion.UsePotion(character);
            potionEntry[potionID]--;

            if (potionEntry[potionID] <= 0) PotionSlot.RemoveAt(slotIndex);

            return true;
        }

        /// <summary>
        /// 특정 ID의 포션을 버리기
        /// </summary>
        public void DropPotion(int potionID, int amount)
        {
            for (int i = 0; i < PotionSlot.Count; i++)
            {
                if (PotionSlot[i].ContainsKey(potionID))
                {
                    PotionSlot[i][potionID] -= amount;

                    if (PotionSlot[i][potionID] <= 0)
                    {
                        PotionSlot.RemoveAt(i); // 개수가 0이면 슬롯에서 삭제
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 특정 포션의 개수를 반환
        /// </summary>
        public int GetPotionCount(int potionID)
        {
            int totalCount = 0;

            foreach (var slot in PotionSlot)
            {
                if (slot.ContainsKey(potionID))
                {
                    totalCount += slot[potionID];
                }
            }
            return totalCount;
        }

        /// <summary>
        /// 특정 ID의 포션 객체를 반환 (사용 시에만 생성)
        /// </summary>
        public Potion GetPotionById(int potionID)
        {
            Dictionary<string, object> potionData = DataTableManager.Instance.GetDBData("potion", potionID);

            return new Potion(
                potionData["name"].ToString(),
                Convert.ToBoolean(potionData["isPercent"]),
                Convert.ToSingle(potionData["changeHp"]),
                Convert.ToSingle(potionData["changeMp"]),
                Convert.ToSingle(potionData["changeAtk"]),
                Convert.ToSingle(potionData["changeDef"]),
                Convert.ToSingle(potionData["changeCrt"]),
                Convert.ToSingle(potionData["changeCrtDmg"]),
                Convert.ToSingle(potionData["changeMiss"]),
                Convert.ToInt32(potionData["duration"]),
                potionData["description"].ToString()
            );
        }

        /// <summary>
        /// 아이템을 ID로 검색해 이름을 돌려 받는 함수
        /// </summary>
        /// <param name="itemId">아이템 ID</param>
        /// <returns>아이템 이름</returns>
        public string GetItemNameById(int itemId)
        {
            if (itemId >= 0 && itemId < AllItem.Count)
            {
                return AllItem[itemId].Name;
            }
            return "알 수 없는 아이템";
        }
        /// <summary>
        /// 포션 목록을 출력하는 함수
        /// </summary>
        /// <param name="line">포션 목록을 출력할 줄</param>
        public void DisplayPotion(int line)
        {
            if (PotionSlot.Count == 0)
            {
                Utility.PrintFree("보유 포션 없음", line);
                return;
            }

            Utility.PrintFree("보유 포션 목록", line);
            for (int i = 0; i < PotionSlot.Count; i++)
            {
                int potionID = PotionSlot[i].Keys.First();
                int quantity = PotionSlot[i][potionID];
                Potion potion = GetPotionById(potionID);

                Utility.PrintFree($"{i + 1}. {potion.DisplayPotion()} x{quantity}", line+1+i);
            }
        }

    }
}
    
