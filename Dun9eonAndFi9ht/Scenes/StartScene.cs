using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.StaticClass;
using DataDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Scenes
{
    internal class StartScene : Scene 
    {
        /// <summary>
        /// StartScene을 시작하는 함수
        /// </summary>
        public override ESceneType Start() 
        {
            base.Start();
            Utility.PrintScene("Dun9eon & Fi9ht에 오신 여러분 환영합니다.");
            Utility.PrintScene("이제 전투를 시작할 수 있습니다.");
            
            while(true)
            {
                Utility.ClearMenu();
                Utility.PrintMenu(new string[] { "1. 상태보기", "2. 전투 시작", "\n원하시는 행동을 입력해주세요.\n>>" });
                int userInput = Utility.UserInput(1, 2);
                if (userInput == 1)
                {
                    return ESceneType.PlayerStat;
                }
                else if (userInput == 2) 
                {
                    return ESceneType.Dungeon;
                }
                else
                {
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
}
