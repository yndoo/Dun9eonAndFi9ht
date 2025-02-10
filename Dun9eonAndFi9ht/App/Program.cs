using DataDefinition;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.Scenes;
using System.Runtime.CompilerServices;
using DataDefinition;

namespace Dun9eonAndFi9ht.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isPlayerSet = SetPlayer(0);
            if (isPlayerSet)
            {
                GameManager.Instance.Player.DisplayStatus();
                RunGame();
            }
            else
            {
                Console.WriteLine("게임을 종료합니다.");
            }
        }

        /// <summary>
        /// 게임 실행 함수
        /// </summary>
        static void RunGame()
        {
            ESceneType nextScene = GameManager.Instance.LoadScene(ESceneType.StartScene);
            while(true)
            {
                nextScene = GameManager.Instance.LoadScene(nextScene);
            }
        }
        /// <summary>
        /// 플레이어 정보 찾기, 찾지 못하면 에러
        /// </summary>
        /// <param name="ID">플레이어 ID에 따라 데이터 읽어 옴</param>
        /// <returns>플레이어 정보 로드에 성공 하였는지</returns>
        public static bool SetPlayer(int ID)
        {
            DataTableManager.Instance.Initialize("../../../database");
            Dictionary<string, object>? Info = DataTableManager.Instance.GetDBData("player", ID);

            if (Info == null || Info.Count < 7)
            {
                Console.WriteLine($"플레이어 데이터를 찾을 수 없습니다. ID: {ID}");
                return false;
            }

            int[] expTable = new int[10];
            for (int i = 0; i < 10; i++)
            {
                Dictionary<string, object>? Info_ExpTable = DataTableManager.Instance.GetDBData("playerExpTable", i);

                if (Info_ExpTable == null)
                {
                    Console.WriteLine($"플레이어 경험치 데이터를 찾을 수 없습니다. ID: {ID}");
                    return false;
                }
                expTable[i] = Convert.ToInt32(Info_ExpTable["MaxExp"]);
            }


            // EJobType 변환 예외 처리
            if (!Info.TryGetValue("job", out var jobValue) || jobValue == null || !Enum.TryParse<EJobType>(jobValue.ToString(), out var jobType))
            {
                Console.WriteLine($"잘못된 직업 값: {jobValue} → 기본 직업(Warrior)으로 설정");
                jobType = EJobType.Warrior;
            }
            try
            {
                int maxHp = Convert.ToInt32(Info["maxHp"]);
                int atk = Convert.ToInt32(Info["atk"]);
                int def = Convert.ToInt32(Info["def"]);
                int level = Convert.ToInt32(Info["level"]);
                int gold = Convert.ToInt32(Info["gold"]);
                GameManager.Instance.GetPlayerInfo(
                Info["name"].ToString(),    // string (이름)
                jobType,// 변환된 EJobType
                maxHp,
                atk,
                def,
                level,
                gold,
                expTable
                );
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("플레이어 데이터 형식 오류!"+ ex);
                return false;
            }
        }
    }
}
