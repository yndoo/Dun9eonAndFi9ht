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
        /// Scene을 시작하는 함수
        /// </summary>
        public override void Start() 
        {
            base.Start();
            Console.WriteLine("Dun9eon & Fi9ht에 오신 여러분 환영합니다.");
            Console.WriteLine("이제 전투를 시작할 수 있습니다.");

            // To Do : PrintMenu가 변경된 이후에 맞춰서 다시 작성할 예정.
            //Utility.PrintMenu(new List<string> {"상태 보기", "전투 시작"});
            int userInput = Utility.UserInput(1, 2);
            if(userInput == 1)
            {
                StatScreen();
            }
            else
            {
                //GameManager.Instance.LoadScene(userInput);
            }
        }

        /// <summary>
        /// 상태 보기 창을 출력
        /// </summary>
        private void StatScreen()
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            //GameManager.Instance.Player.DisplayStatus();

            // To Do : PrintMenu가 변경된 이후에 맞춰서 다시 작성할 예정.
            //Utility.PrintMenu(new List<string> {"나가기"});
            int userInput = Utility.UserInput(1, 2);
            if (userInput == 0)
            {
                //GameManager.Instance.LoadScene(ESceneType.StartScene);
            }
        }
    }
}
