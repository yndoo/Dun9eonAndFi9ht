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
            RunGame();            
        }

        /// <summary>
        /// 게임 실행 함수
        /// </summary>
        static void RunGame()
        {
            bool isPlayerSet = SetPlayer(0);
            if(isPlayerSet)
            {
                GameManager.Instance.Player.DisplayStatus();
                GameManager.Instance.LoadScene(ESceneType.StartScene);
            }
            else
            {
                Console.WriteLine("게임을 종료합니다.");
            }
        }
        /// <summary>
        /// 플레이어 정보 찾기, 찾지 못하면 에러
        /// </summary>
        /// <param name="ID">플레이어 ID에 따라 데이터 읽어 옴</param>
        /// <returns>플레이어 정보 로드에 성공 하였는지</returns>
        static bool SetPlayer(int ID)
        {
            DataTableManager.Instance.Initialize("../../../database");
            List<string>? Info = DataTableManager.Instance.GetMonsterData("player", ID);

            if (Info == null || Info.Count < 7)
            {
                Console.WriteLine($"플레이어 데이터를 찾을 수 없습니다. ID: {ID}");
                return false;
            }

            // EJobType 변환 예외 처리
            if (!Enum.TryParse<EJobType>(Info[2], out var jobType))
            {
                Console.WriteLine($"잘못된 직업 값: {Info[1]} → 기본 직업(Warrior)으로 설정");
                jobType = EJobType.Warrior; // 기본값 설정
            }
            try
            {
                GameManager.Instance.GetPlayerInfo(
                Info[1],     // string (이름)
                jobType,           // 변환된 EJobType
                int.Parse(Info[3]),
                int.Parse(Info[4]),
                int.Parse(Info[5]),
                int.Parse(Info[6]),
                int.Parse(Info[7])
                );
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("플레이어 데이터 형식 오류!");
                return false;
            }
        }
    }
}
