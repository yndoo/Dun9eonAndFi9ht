using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
namespace Dun9eonAndFi9ht.Manager
{
    public class DataTableManager
    {
        private static DataTableManager? instance;
        public static DataTableManager Instance => instance ??= new DataTableManager();

        //Database에 적 정보 뿐만 아니라 추후 추가될 데이터들을 위해 Dictionary의 key 값으로 List 구분 시도.
        private Dictionary<string, Dictionary<string, Dictionary<string, object>>>  database;

        private DataTableManager()
        {
            database = new Dictionary<string, Dictionary<string, Dictionary<string, object>>>();
        }


        /// <summary>
        /// 실행 시 Dictionary<DB 종류, 데이터 리스트>
        /// </summary>
        /// <param name="folderPath">DB json 파일이 든 폴더 경로</param>
        public void Initialize(string folderPath)
        {
            try
            {
                string[] files = Directory.GetFiles(folderPath, "*.json");

                foreach (string file in files)
                {
                    string jsonContent = File.ReadAllText(file);

                    try
                    {
                        // 먼저 Dictionary<string, JsonElement>로 역직렬화
                        var jsonData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, JsonElement>>>(jsonContent);

                        if (jsonData != null)
                        {
                            foreach (var table in jsonData)
                            {
                                // JsonElement를 Dictionary<string, Dictionary<string, object>>로 변환
                                if (!database.ContainsKey(table.Key))
                                {
                                    database[table.Key] = new Dictionary<string, Dictionary<string, object>>();
                                }

                                foreach (var row in table.Value)
                                {
                                    database[table.Key][row.Key] = ConvertJsonElementToDictionary(row.Value);
                                }
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON 파싱 오류 (파일: {Path.GetFileName(file)}): {ex.Message}");
                    }
                }
                Console.WriteLine("DB 로딩 완료");
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB 로딩 오류 : " + ex.Message);
            }
        }

        /// <summary>
        /// 특정 DB 테이블에서 ID를 검색하면 찾아서 데이터를 돌려주는 함수
        /// </summary>
        /// <param name="tableName">DB 테이블 이름</param>
        /// <param name="id">찾을 대상의 ID</param>
        /// <returns>List<string?>(해당 ID의 데이터, 없으면 null)</returns>
        public Dictionary<string, object> GetDBData(string tableName, int ID)
        {
            string key = ID.ToString();
            if (database.TryGetValue(tableName, out var tableData))
            {
                if(tableData.TryGetValue(key, out var row))
                {
                    Console.WriteLine($"{row["name"]}");
                    return row;
                }
            }
            return null;
        }

        public void PrintDatabase()
{
    Console.WriteLine("\n===== 데이터베이스 전체 출력 =====");

    if (database.Count == 0)
    {
        Console.WriteLine("데이터베이스가 비어 있습니다.");
        return;
    }

    foreach (var table in database)
    {
        Console.WriteLine($"\n 테이블: {table.Key}");
        Console.WriteLine(new string('-', 40)); // 구분선

        if (table.Value.Count == 0)
        {
            Console.WriteLine("데이터 없음");
            continue;
        }

        foreach (var row in table.Value)
        {
            Console.WriteLine($" ID: {row.Key}");
            foreach (var field in row.Value)
            {
                Console.WriteLine($"    - {field.Key}: {field.Value}");
            }
            Console.WriteLine(new string('-', 40)); // 구분선
        }
    }

    Console.WriteLine("================================\n");
}

        /// <summary>
        /// JsonElement를 Dictionary<string, object>로 변환하는 유틸리티 함수
        /// </summary>
        private Dictionary<string, object> ConvertJsonElementToDictionary(JsonElement element)
        {
            var dict = new Dictionary<string, object>();

            foreach (var property in element.EnumerateObject())
            {
                switch (property.Value.ValueKind)
                {
                    case JsonValueKind.String:
                        dict[property.Name] = property.Value.GetString();
                        break;
                    case JsonValueKind.Number:
                        dict[property.Name] = property.Value.GetDouble(); // int일 수도 있지만 안전하게 double로 변환
                        break;
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        dict[property.Name] = property.Value.GetBoolean();
                        break;
                    case JsonValueKind.Object:
                        dict[property.Name] = ConvertJsonElementToDictionary(property.Value);
                        break;
                    case JsonValueKind.Array:
                        dict[property.Name] = ConvertJsonElementToList(property.Value);
                        break;
                    case JsonValueKind.Null:
                        dict[property.Name] = null;
                        break;
                }
            }
            return dict;
        }

        /// <summary>
        /// JsonElement가 배열일 경우 List<object>로 변환
        /// </summary>
        private List<object> ConvertJsonElementToList(JsonElement element)
        {
            var list = new List<object>();

            foreach (var item in element.EnumerateArray())
            {
                switch (item.ValueKind)
                {
                    case JsonValueKind.String:
                        list.Add(item.GetString());
                        break;
                    case JsonValueKind.Number:
                        list.Add(item.GetDouble());
                        break;
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        list.Add(item.GetBoolean());
                        break;
                    case JsonValueKind.Object:
                        list.Add(ConvertJsonElementToDictionary(item));
                        break;
                    case JsonValueKind.Array:
                        list.Add(ConvertJsonElementToList(item));
                        break;
                    case JsonValueKind.Null:
                        list.Add(null);
                        break;
                }
            }

            return list;
        }

    }
}
