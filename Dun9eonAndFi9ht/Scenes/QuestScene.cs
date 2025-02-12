using DataDefinition;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.Quests;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Scenes
{
    internal class QuestScene : Scene
    {
        public override ESceneType Start()
        {
            List<Quest> quests = new List<Quest>();
            int selectCase;
            bool isVaild = false;
            while (!isVaild)
            {
                Utility.ClearAll();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Utility.PrintScene("Quest!!");
                Console.ResetColor();
                Utility.PrintScene(" ");
                Utility.PrintScene("확인할 퀘스트 목록을 선택하세요");
                Utility.PrintScene(" ");


                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW("1");
                Console.ResetColor();
                Utility.PrintScene(". 수행 중인 퀘스트");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW("2");
                Console.ResetColor();
                Utility.PrintScene(". 받지 않은 퀘스트");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintSceneW("3");
                Console.ResetColor();
                Utility.PrintScene(". 완료한 퀘스트");


                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("0.");
                Console.ResetColor();
                Utility.PrintMenu(" 나가기");
                Utility.PrintMenu("");
                Utility.PrintMenu("");
                Utility.PrintMenu("원하시는 행동을 입력해주세요.");
                Utility.PrintMenu(">>> ");

                selectCase = Utility.UserInput(0, 4);
                if (selectCase == 0) return ESceneType.StartScene;

                switch (selectCase)
                {
                    case 1:
                        isVaild = true;
                        quests = QuestManager.Instance.GetAcceptedQuests();
                        break;
                    case 2:
                        isVaild = true;
                        quests = QuestManager.Instance.GetAvailableQuests();
                        break;
                    case 3:
                        isVaild = true;
                        quests = QuestManager.Instance.GetCompletedQuests();
                        break;
                    case 4:
                        isVaild = true;
                        quests = QuestManager.Instance.GetAllQuests();
                        break;
                    default:
                        Utility.ClearMenu();
                        Utility.PrintMenu("잘못된 입력입니다.");
                        Utility.PrintMenu("");
                        Utility.PrintMenu("아무 키나 눌러주세요.");
                        Utility.PrintMenu(">>");
                        Console.ReadKey();
                        break;
                }
            }
            isVaild = false;
            while (!isVaild)
            {
                if (quests.Count == 0)
                {
                    isVaild = true;
                    Utility.ClearAll();
                    Utility.PrintScene("해당 목록에 퀘스트가 없습니다.");
                    Utility.PrintMenu("아무 키나 입력해주세요.\n>>");
                    Console.SetCursorPosition(0, 18);
                    Console.ReadKey();
                    return ESceneType.QuestScene;
                }

                Utility.ClearAll();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Utility.PrintScene("Quest!!");
                Console.ResetColor();
                Utility.PrintScene(" ");
                Utility.PrintScene("확인할 퀘스트 목록을 선택하세요");
                Utility.PrintScene(" ");
                for (int i = 0; i < quests.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintSceneW($"{i + 1}");
                    Console.ResetColor();
                    Utility.PrintScene($". {quests[i].QuestTitle}");
                }

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("0.");
                Console.ResetColor();
                Utility.PrintMenu(" 나가기");
                Utility.PrintMenu("");
                Utility.PrintMenu("");
                Utility.PrintMenu("원하시는 행동을 입력해주세요.");
                Utility.PrintMenu(">>> ");

                int questSelect = Utility.UserInput(0, quests.Count);

                if (questSelect == 0) return ESceneType.QuestScene;
                else if (questSelect > 0 && questSelect <= quests.Count)
                {
                    isVaild = true;
                    Quest selectedQuest = quests[questSelect - 1];
                    selectedQuest.ShowQuestInfo();
                    Console.ReadKey();
                }
                else
                {
                    Utility.ClearMenu();
                    Utility.PrintMenu("잘못된 입력입니다.");
                    Utility.PrintMenu("");
                    Utility.PrintMenu("아무 키나 눌러주세요.");
                    Utility.PrintMenu(">>");
                    Console.ReadKey();
                }
            }
            return ESceneType.QuestScene;
        }
    }
}
