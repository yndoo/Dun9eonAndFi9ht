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
            // 새로 생성했거나 죽어서 돌아온 경우 새로 시작
            if (GameManager.Instance.Player.Name == "" || GameManager.Instance.Player.CurrentHp == 0)
            {
                NewGame();
            }

            Utility.PrintScene("Dun9eon & Fi9ht에 오신 여러분 환영합니다.");
            Utility.PrintScene("이제 전투를 시작할 수 있습니다.");

            // 메뉴 출력
            while(true)
            {
                Utility.ClearMenu();
                Utility.PrintMenu(new string[] { "1. 상태보기", $"2. 전투 시작 (현재 : {Dungeon.stage}층)", "3. 층 이동하기", "\n원하시는 행동을 입력해주세요.\n>>" });
                int userInput = Utility.UserInput(1, 3);
                if (userInput == 1)
                {
                    return ESceneType.PlayerStat;
                }
                else if (userInput == 2) 
                {
                    return ESceneType.Dungeon;
                }
                else if(userInput == 3)
                {
                    Dungeon.MovingStage();
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
        /// <summary>
        /// 새로 시작하는 Character의 이름과 직업을 입력받는 함수 
        /// </summary>
        private void NewGame()
        {
            Utility.ClearScene();
            string nameInput;
            EJobType jobType;
            // 이름 설정
            while (true)
            {
                Utility.ClearAll();
                Utility.PrintScene("[ Dun9eon & Fi9ht - 새로 시작 ]");
                Utility.PrintScene("원하시는 이름을 설정해주세요.");
                Utility.PrintMenu("이름을 입력하세요.\n>>");
                nameInput = Console.ReadLine();
                Utility.PrintScene($"입력하신 이름은 '{nameInput}'입니다. 저장하시겠습니까?");


                bool isSaved = false;
                while(true)
                {
                    Utility.ClearMenu();
                    Utility.PrintMenu("1. 저장\n2. 다시 입력");
                    Utility.PrintMenu("원하시는 행동을 입력해주세요.\n>>");
                    int command = Utility.UserInput(1, 2);
                    if (command == 1)
                    {
                        isSaved = true;
                        break;
                    }
                    else if (command == 2) break;
                    else
                    {
                        Utility.ClearMenu();
                        Utility.PrintMenu("잘못된 입력입니다.");
                        Utility.PrintMenu("아무 키나 입력\n>>");
                        Console.ReadLine();
                    }
                }
                if (isSaved) break;
                else continue;
            }

            // 직업 선택
            while (true)
            {
                Utility.ClearAll();
                Utility.PrintScene("원하시는 직업을 선택해주세요.");
                Utility.PrintScene("1. Warrior\n2. Mage\n3. Rogue");
                Utility.PrintMenu("직업을 선택하세요.\n>>");
                int jobInput = Utility.UserInput(1, 3) - 1;
                if (jobInput < 0)
                {
                    Utility.ClearMenu();
                    Utility.PrintMenu("잘못된 입력입니다.");
                    Utility.PrintMenu("아무 키나 입력\n>>");
                    Console.ReadLine();
                    continue;
                }
                jobType = (EJobType)jobInput;
                Utility.PrintScene($"선택하신 직업은 '{jobType.ToString()}'입니다. 저장하시겠습니까?");

                bool isSaved = false;
                while (true)
                {
                    Utility.ClearMenu();
                    Utility.PrintMenu("1. 저장\n2. 다시 입력");
                    Utility.PrintMenu("원하시는 행동을 입력해주세요.\n>>");
                    int command = Utility.UserInput(1, 2);
                    if (command == 1)
                    {
                        App.Program.SetPlayer(jobInput);
                        isSaved = true;
                        break;
                    }
                    else if (command == 2) break;
                    else
                    {
                        Utility.ClearMenu();
                        Utility.PrintMenu("잘못된 입력입니다.");
                        Utility.PrintMenu("아무 키나 입력\n>>");
                        Console.ReadLine();
                    }
                }
                if (isSaved) break;
                else continue;
            }

            GameManager.Instance.Player.Name = nameInput;
            GameManager.Instance.Player.Job = jobType;

            Utility.ClearScene();
            Utility.ClearMenu();
        }
    }
}
