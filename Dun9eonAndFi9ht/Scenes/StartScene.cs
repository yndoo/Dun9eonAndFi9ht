﻿using Dun9eonAndFi9ht.Manager;
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
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("1");
                Console.ResetColor();
                Utility.PrintMenu(". 상태보기");
                Console.ForegroundColor = ConsoleColor.Magenta;

                Utility.PrintMenuW("2");
                Console.ResetColor();
                Utility.PrintMenuW(". 전투 시작 ");
                Utility.PrintMenuW("(현재 ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW($"{Dungeon.stage}");
                Console.ResetColor();
                Utility.PrintMenu("층)");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("3");
                Console.ResetColor();
                Utility.PrintMenu(". 층 이동하기");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("4");
                Console.ResetColor();
                Utility.PrintMenu(". 인벤토리");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("5");
                Console.ResetColor();
                Utility.PrintMenu(". 퀘스트 수주");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("6");
                Console.ResetColor();
                Utility.PrintMenu(". 게임 종료");
                Utility.PrintMenu("");
                Utility.PrintMenu("");
                Utility.PrintMenu("원하시는 행동을 입력해주세요.\n>>");

                

            int userInput = Utility.UserInput(1, 6);
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
                    return ESceneType.MoveStage;
                }
                else if (userInput == 4)
                {
                    return ESceneType.InventoryScene;
                }
                else if (userInput == 5)
                {
                    return ESceneType.QuestScene;
                }
                else if(userInput == 6)
                {
                    return ESceneType.Exit;
                }
                else
                {
                    Utility.ClearMenu();
                    Utility.PrintMenu("잘못된 입력입니다.\n");
                    Utility.PrintMenu("아무 키나 입력해주세요.\n>>");
                    Console.ReadKey();
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
                if(nameInput.Count() == 0)
                {
                    nameInput = "None";
                }
                Utility.PrintScene($"입력하신 이름은 '{nameInput}'입니다. 저장하시겠습니까?");


                bool isSaved = false;
                while(true)
                {
                    Utility.ClearMenu();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintMenuW("1");
                    Console.ResetColor();
                    Utility.PrintMenu(". 저장");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintMenuW("2");
                    Console.ResetColor();
                    Utility.PrintMenu(". 다시 입력");
                    Utility.PrintMenu("");
                    Utility.PrintMenu("");
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
                        Utility.PrintMenu("잘못된 입력입니다.\n");
                        Utility.PrintMenu("아무 키나 입력해주세요.\n>>");
                        Console.ReadKey();
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
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW("1");
                Console.ResetColor();
                Utility.PrintScene(". Warrior");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW("2");
                Console.ResetColor();
                Utility.PrintScene(". Mage");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW("3");
                Console.ResetColor();
                Utility.PrintScene(". Rogue");
                Utility.PrintMenu("직업을 선택하세요.\n>>");
                int jobInput = Utility.UserInput(1, 3) - 1;
                if (jobInput < 0)
                {
                    Utility.ClearMenu();
                    Utility.PrintMenu("잘못된 입력입니다.\n");
                    Utility.PrintMenu("아무 키나 입력해주세요.\n>>");
                    Console.ReadKey();
                    continue;
                }
                jobType = (EJobType)jobInput;
                Utility.PrintScene($"선택하신 직업은 '{jobType.ToString()}'입니다. 저장하시겠습니까?");

                bool isSaved = false;
                while (true)
                {
                    Utility.ClearMenu();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintMenuW("1");
                    Console.ResetColor();
                    Utility.PrintMenu(". 저장");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintMenuW("2");
                    Console.ResetColor();
                    Utility.PrintMenu(". 다시 입력");
                    Utility.PrintMenu("");
                    Utility.PrintMenu("");
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
                        Utility.PrintMenu("잘못된 입력입니다.\n");
                        Utility.PrintMenu("아무 키나 입력해주세요.\n>>");
                        Console.ReadKey();
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
