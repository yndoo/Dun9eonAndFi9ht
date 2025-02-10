using DataDefinition;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Scenes
{
    internal class PlayerStat : Scene
    {
        /// <summary>
        /// 상태 보기 창을 출력
        /// </summary>
        public override ESceneType Start()
        {
            Utility.ClearScene();
            Utility.PrintScene("상태 보기");
            Utility.PrintScene("캐릭터의 정보가 표시됩니다.\n");

            GameManager.Instance.Player.DisplayStatus();

            // 메뉴 출력
            Utility.ClearMenu();
            Utility.PrintMenu("0. 나가기");
            while (true)
            {
                int userInput = Utility.UserInput(0, 0);
                if (userInput == 0)
                {
                    return ESceneType.StartScene;
                }
                Utility.PrintMenu("잘못된 입력입니다.");
            }
        }
    }
}
