using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Scenes
{
    internal class ResultScene : Scene
    {
        /// <summary>
        /// ResultScene 시작 함수. 배틀 결과 출력.
        /// </summary>
        public override void Start()
        {
            Console.Clear();
            Console.WriteLine("Battle!! - Result");
            // To Do : 배틀 결과를 어떻게 가져올지 논의 필요
            Console.WriteLine("");

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
