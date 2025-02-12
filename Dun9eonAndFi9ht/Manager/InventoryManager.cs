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

        public Dictionary<int, int> PotionSlot { get; }     //현재 갖고 있는 포션


        private int itemCount; // 아이템 전체 개수

        /// <summary>
        /// InventoryManager 생성자 - 전체 아이템 리스트 불러오기 
        /// </summary>
        public InventoryManager()
        {
            AllItem = new List<Item>();
            Inventory = new List<Item>();
            EquipSlot = new Dictionary<EItemEquipType, Item>();
            PotionSlot = new Dictionary<int, int>();
            itemCount = 11;
            // 아이템 정보 불러오기
            
            for (int id = 0; id < itemCount; id++)
            {
                Dictionary<string, object> itemInfo = DataTableManager.Instance.GetDBData("item", id);
                string name = itemInfo["Name"].ToString();
                EItemEquipType type = (EItemEquipType)(Convert.ToInt32(itemInfo["EquipType"]));
                float maxHp = Convert.ToSingle(itemInfo["MaxHp"]);
                float maxMp = Convert.ToSingle(itemInfo["MaxMp"]);
                float atk = Convert.ToSingle(itemInfo["Atk"]);
                float def = Convert.ToSingle(itemInfo["Def"]);
                float critRate = Convert.ToSingle(itemInfo["CriticalRate"]);
                float critDmg = Convert.ToSingle(itemInfo["CriticalDamage"]);
                float missRate = Convert.ToSingle(itemInfo["MissRate"]);
                AllItem.Add(new Item(id, name, type, maxHp, maxMp, atk, def, critRate, critDmg, missRate));
            }

            // 인벤토리&아이템 테스트용 코드
            //for (int i = 0; i < itemCount; i++)
            //{
            //    GrantItem(i);
            //}

            //GrantPotion(0);
            //GrantPotion(1);
            //GrantPotion(2);
        }
        /// <summary>
        /// 로드된 데이터를 인벤토리에 적용시키는 함수.
        /// </summary>
        /// <param name="loadData">json파일에서 읽어온 인벤토리 데이터</param>
        public void ApplyLoadedData(InventoryData loadData)
        {
            foreach (int id in loadData.Inventory)
            {
                GrantItem(id);
            }
            foreach (int id in loadData.EquipItems)
            {
                ItemEquip(AllItem[id]);
            }
            for(int i = 0; i < PotionSlot.Count; i++)
            {
                PotionSlot[i] = loadData.PotionSlot[i];
            }
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
            if (PotionSlot.ContainsKey(potionID))
            {
                PotionSlot[potionID] += amount;
            }
            else
            {
                PotionSlot[potionID] = amount;
            }
        }

        /// <summary>
        /// 특정 ID의 포션을 사용
        /// </summary>
        public bool UsePotion(int potionID, Character character)
        {
            if (PotionSlot.ContainsKey(potionID) && PotionSlot[potionID] > 0)
            {
                Potion potion = GetPotionById(potionID); // ✅ 포션 객체 생성
                potion.UsePotion(character);

                // 포션 개수 감소
                PotionSlot[potionID]--;
                if (PotionSlot[potionID] <= 0)
                {
                    PotionSlot.Remove(potionID); // 개수가 0이면 제거
                }

                return true;
            }
            else
            {
                Console.WriteLine("해당 포션이 없습니다.");
                return false;
            }
        }

        /// <summary>
        /// 특정 ID의 포션을 버리기
        /// </summary>
        public void DropPotion(int potionID, int amount)
        {
            if (PotionSlot.ContainsKey(potionID))
            {
                PotionSlot[potionID] -= amount;
                if (PotionSlot[potionID] <= 0)
                {
                    PotionSlot.Remove(potionID); // 개수가 0이면 삭제
                }
            }
        }

        /// <summary>
        /// 특정 포션의 개수를 반환
        /// </summary>
        public int GetPotionCount(int potionID)
        {
            return PotionSlot.ContainsKey(potionID) ? PotionSlot[potionID] : 0;
        }

        public void DisplayPotions()
        {
            if (PotionSlot.Count == 0)
            {
                Utility.PrintScene("보유 포션 없음");
                return;
            }
            Utility.PrintScene("보유 포션 목록:");
            int potionNum = 0;
            foreach (var dict in PotionSlot)
            {
                potionNum++;
                int potionID = dict.Key;  // 포션 ID
                int quantity = dict.Value; // 보유 개수
                Potion potion = InventoryManager.Instance.GetPotionById(potionID); // 포션 객체 가져오기
                Utility.PrintSceneW($"{potionNum}: ");
                potion.DisplayPotion();
                Utility.PrintScene($" x{quantity}");
            }
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

    }
}
    
