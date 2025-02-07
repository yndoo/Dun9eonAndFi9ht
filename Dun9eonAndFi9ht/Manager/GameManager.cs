using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinition;
using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Scenes;
using Dun9eonAndFi9ht.StaticClass;

namespace Dun9eonAndFi9ht.Manager
{
    internal class GameManager
    {
        
        private static GameManager? instance;
        public static GameManager Instance => instance ??= new GameManager();

        private Player player;
        private List<Scene> sceneList;
        public List<Scene> SceneList { get; set; }
        public Player Player { get; set; }

        private GameManager()
        {
            sceneList = new List<Scene>();
            sceneList.Add(new StartScene());
        }
        /// <summary>
        /// 시작할 씬을 string 값으로 받아 해당 씬 Start() 실행
        /// </summary>
        /// <param name="type">씬 이름</param>
        public void LoadScene(ESceneType type)
        {
            int sceneIndex = (int)type;
            if (sceneIndex >= 0 && sceneIndex <= sceneList.Count)
            {
                try
                {
                    Console.Clear();
                    sceneList[sceneIndex].Start();
                }
                catch (Exception ex)
                {
                    Utility.PrintScene($"씬 실행 실패: {ex.Message}");
                }
            }
            else
            {
                Utility.PrintScene("잘못된 씬입니다.");
            }
        }
        /// <summary>
        /// 시작할 씬을 int 값으로 해당 씬 Start() 실행
        /// </summary>
        /// <param name="type"></param>
        public void LoadScene(int type)
        {
            if (type >= 0 && type < sceneList.Count)
            {
                try
                {
                    Console.Clear();
                    sceneList[type].Start();
                }
                catch (Exception ex)
                {
                    Utility.PrintScene($"씬 실행 실패: {ex.Message}");
                }

            }
            else
            {
                Console.WriteLine("잘못된 씬입니다.");
            }
        }

        /// <summary>
        /// GameManager의 Player 정보를 줘 새 플레이어 인스턴스를 만드는 메서드
        /// </summary>
        /// <param name="name">이름</param>
        /// <param name="job">직업</param>
        /// <param name="maxHp">최대 Hp</param>
        /// <param name="atk">공격력</param>
        /// <param name="def">방어력</param>
        /// <param name="level">레벨</param>
        /// <param name="gold">골드</param>
        public void GetPlayerInfo(string name, DataDefinition.EJobType job, int maxHp, int atk, int def, int level, int gold)
        {
            this.Player = new Player(name, job, maxHp, atk, def, level, gold);
        }
    }
}
