﻿using DataDefinition;
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
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("0");
                Console.ResetColor();
                Utility.PrintMenu(". 나가기");
                Utility.PrintMenu("");
                Utility.PrintMenu("");
                Utility.PrintMenu("원하시는 행동을 입력해주세요.");
                Utility.PrintMenu(">>");

                int userInput = Utility.UserInput(0, 0);
                if (userInput == 0)
                {
                    return ESceneType.StartScene;
                }

                int nextInput = -1;

                Utility.ClearMenu();
                Utility.PrintMenu("잘못된 입력입니다.");
                Utility.PrintMenu("");
                Utility.PrintMenu("아무 키나 눌러주세요.");
                Utility.PrintMenu(">>");
                Console.ReadKey();
            }
        }
    }
}
