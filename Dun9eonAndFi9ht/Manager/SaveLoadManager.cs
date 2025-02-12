using DataDefinition;
using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dun9eonAndFi9ht.Manager
{
    public class InventoryData
    {
        public List<int> Inventory { get; set; }
        public List<int> EquipItems { get; set; }
        public Dictionary<int, int> PotionSlot { get; set; }
        public InventoryData()
        {
            Inventory = new List<int>();
            EquipItems = new List<int>();
            PotionSlot = new Dictionary<int, int>();
        }
    }

    internal class SaveLoadManager
    {
        private static SaveLoadManager? instance;
        public static SaveLoadManager Instance => instance ??= new SaveLoadManager();
        public void PlayerSave()
        {

        }
        /// <summary>
        /// 인벤토리 데이터 저장
        /// </summary>
        public void InventorySave()
        {
            string filePath = "../../../DataBase/SaveData/InventoryData.json";
            InventoryManager inventoryManager = InventoryManager.Instance;
            InventoryData data = new InventoryData();
            foreach (Item it in inventoryManager.Inventory)         // 소지 아이템 Id 저장
            {
                data.Inventory.Add(it.Id);
            }
            foreach (Item it in inventoryManager.EquipSlot.Values)  // 장착 아이템 Id 저장
            {
                data.EquipItems.Add(it.Id);
            }
            data.PotionSlot = inventoryManager.PotionSlot;          // 소지 포션 Id와 개수 저장

            try
            {
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true /*줄바꿈, 들여쓰기 정렬 해줌*/});
                File.WriteAllText(filePath, json);
            }
            catch(Exception ex)
            {
                Console.WriteLine("인벤토리 데이터 저장 오류 {0}", ex.Message);
            }
        }
        public void DungeonSave()
        {

        }

        public void GameSave()
        {

        }

        /// <summary>
        /// 인벤토리 데이터 로드
        /// </summary>
        public void InventoryLoad()
        {
            string filePath = "../../../DataBase/SaveData/InventoryData.json";
            if(!File.Exists(filePath))
            {
                // 저장된 데이터 없을 때 처리
                return;
            }
            string json = File.ReadAllText(filePath);
            try
            {
                InventoryManager.Instance.ApplyLoadedData(JsonSerializer.Deserialize<InventoryData>(json));
            }
            catch (Exception ex)
            {
                Console.WriteLine("인벤토리 데이터 로드 오류 {0}", ex.Message);
            }

        }
    }
}
