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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utility.PrintScene("상태 보기");
            Console.ResetColor();
            Utility.PrintScene("캐릭터의 정보가 표시됩니다.\n");

            GameManager.Instance.Player.DisplayStatus();

            // 메뉴 출력
            while (true)
            {
                Utility.ClearMenu();
                Utility.PrintMenu("0. 나가기");

                int userInput = Utility.UserInput(0, 0);
                if (userInput == 0)
                {
                    return ESceneType.StartScene;
                }

                int nextInput = -1;
                while (nextInput != 0)
                {
                    Utility.ClearMenu();
                    Utility.PrintMenu("잘못된 입력입니다.");
                    Utility.PrintMenu("0. 확인");
                    Utility.PrintMenu("");
                    Utility.PrintMenu(">>");
                    nextInput = Utility.UserInput(0, 0);
                }
            }
        }
    }
}
