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

            Utility.PrintMenu(new string[] {"1. 상태보기", "2. 전투 시작", "\n원하시는 행동을 입력해주세요.\n>>"});
            while(true)
            {
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
                    Utility.PrintMenu("잘못된 입력입니다.");
                }
            }            
        }
    }
}
