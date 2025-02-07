using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.StaticClass;
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
        public override void Start() 
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
                    StatScreen();
                    return;
                }
                else if (userInput == 2) 
                {
                    // To Do : Dungeon씬으로 이동
                    //GameManager.Instance.LoadScene(userInput);
                    return;
                }
                else
                {
                    Utility.PrintMenu("잘못된 입력입니다.");
                }
            }            
        }

        /// <summary>
        /// 상태 보기 창을 출력
        /// </summary>
        private void StatScreen()
        {
            Utility.ClearScene();
            Utility.PrintScene("상태 보기");
            Utility.PrintScene("캐릭터의 정보가 표시됩니다.");

            // To Do : Player의 DisplayStatus 실행
            //GameManager.Instance.Player.DisplayStatus();

            // 메뉴 출력
            Utility.ClearMenu();
            Utility.PrintMenu("0. 나가기");
            while (true)
            {
                int userInput = Utility.UserInput(0, 0);
                if (userInput == 0) return;
                Utility.PrintMenu("잘못된 입력입니다.");
            }
        }
    }
}
