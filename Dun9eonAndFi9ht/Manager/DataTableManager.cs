using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Manager
{
    public class DataTableManager
    {
        //Database에 적 정보 뿐만 아니라 추후 추가될 데이터들을 위해 Dictionary의 key 값으로 List 구분 시도.
        private Dictionary<string, List<string[]>> database = new Dictionary<string, List<string[]>>();

        /// <summary>
        /// 실행 시 Dictionary<DB 종류, 데이터 리스트>
        /// </summary>
        /// <param name="folderPath">DB csv 파일이 든 폴더 경로</param>
        public void Initialize(string folderPath)
        {
            try
            {
                string[] files = Directory.GetFiles(folderPath, "*.csv");
                foreach (string file in files)
                {
                    string tableName = Path.GetFileNameWithoutExtension(file);

                    if (!database.ContainsKey(tableName))
                    {
                        database[tableName] = new List<string[]>();
                    }

                    string[] lines = File.ReadAllLines(file);
                    foreach (string line in lines)
                    {
                        database[tableName].Add(line.Split(','));
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
        /// <returns>List<string></returns>
        public List<string>? GetMonsterData(string tableName, int id)
        {
            if (database.ContainsKey(tableName))
            {
                foreach (string[] Data in database[tableName])
                {
                    if (int.TryParse(Data[0], out int rowID) && rowID == id)
                    { 
                        return new List<string>(Data);
                    }
                }
            }
            return null;
        }
    }
}
