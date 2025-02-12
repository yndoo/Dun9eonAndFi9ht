﻿using Dun9eonAndFi9ht.Characters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Manager
{
    internal class SaveLoadManager
    {
        private static SaveLoadManager? instance;
        public static SaveLoadManager Instance => instance ??= new SaveLoadManager();

        private string saveDataPath;

        private SaveLoadManager()
        {
            saveDataPath = @"../../../DataBase/SaveData";
            if (!Directory.Exists(saveDataPath))
            {
                Directory.CreateDirectory(saveDataPath);
            }
        }

        public void PlayerSave(Player player)
        {
            string json = JsonConvert.SerializeObject(player, Formatting.Indented);

            string filePath = Path.Combine(saveDataPath, "PlayerData.json");
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine(json);
            }
            Console.WriteLine("저장 완료");
        }

        //public void PlayerLoad()
        //{
        //    string filePath = Path.Combine(saveDataPath, "PlayerData.json");

        //    if (!File.Exists(filePath))
        //    {
        //        Console.WriteLine("파일이 존재하지 않습니다.");
        //        return;
        //    }

        //    string json = string.Empty;

        //    using (StreamReader sr = new StreamReader(filePath))
        //    {
        //        json = sr.ReadToEnd();
        //    }

        //    Player? player = JsonSerializer.Deserialize<Player>(json);

        //    if (player != null)
        //    {
        //        Console.WriteLine("불러오기 완료");
        //        Console.WriteLine($"HP: {player.CurrentHp}");
        //        Console.WriteLine($"이름: {player.Name}");
        //    }
        //    else
        //    {
        //        Console.WriteLine("불러오기 실패");
        //    }
        //}

        public void InventorySave()
        {

        }
        public void DungeonSave()
        {

        }

        public void GameSave()
        {

        }
    }
}
