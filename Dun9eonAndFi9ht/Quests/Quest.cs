using DataDefinition;
using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Quests
{
    public abstract class Quest
    {
        public string QuestTitle { get; set; } // 퀘스트 제목
        public string[] QuestDescription { get; set; } // 퀘스트 내용 설명
        public int RewardItem { get; set; } // 보상 아이템
        public int RewardMoney { get; set; } // 보상 금액
        public bool HasAccepted { get; set; } //퀘스트를 수주한 상태인지 판단
        public bool IsCleared { get; set; } //퀘스트 클리어 여부 판단
        public EQuestType Type { get; set; }



        //퀘스트 생성자
        public Quest(string title, string[] description, int rewardItem, int rewardMoney, EQuestType type)
        {
            QuestTitle = title;
            QuestDescription = description;
            RewardItem = rewardItem;
            RewardMoney = rewardMoney;
            Type = type;
            HasAccepted = false;
            IsCleared = false;
        }
        public abstract bool CheckCompletion(); // 퀘스트 조건 만족 여부 확인
        public abstract void ShowQuestInfo();  // 퀘스트 정보 출력

        public void ShowSelect()
        {
            Utility.ClearMenu();

            if (!HasAccepted&&!IsCleared) // 퀘스트를 아직 수락하지 않은 경우
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("1");
                Console.ResetColor();
                Utility.PrintMenu(". 수락");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenuW("2");
                Console.ResetColor();
                Utility.PrintMenu(". 거절");
            }
            else if (!IsCleared) // 퀘스트 진행 중
            {
                if (CheckCompletion()) // 완료 가능 상태
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintMenuW("1");
                    Console.ResetColor();
                    Utility.PrintMenu(". 퀘스트 완료");

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintMenuW("2");
                    Console.ResetColor();
                    Utility.PrintMenu(". 돌아가기");
                }
                else //완료 불가능 상태
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Utility.PrintMenu("클리어 조건이 충족되지 않았습니다.");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Utility.PrintMenuW("아무 키를 눌러 돌아가기");
                    Console.SetCursorPosition(0, 18);
                    Console.ReadKey();
                    return;
                }

            }
            else // 퀘스트 완료 상태
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Utility.PrintMenu("해당 퀘스트는 이미 완료되었습니다.");
                Console.ResetColor();
                Utility.PrintMenuW("아무 키를 눌러 돌아가기");
                Console.SetCursorPosition(0, 18);
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Utility.PrintMenuW("\n원하시는 행동을 입력해주세요.\n>> ");
                int choice = Utility.UserInput(1, 2);

                if (!HasAccepted) // 퀘스트 수락/거절
                {
                    switch (choice)
                    {
                        case 1:
                            HasAccepted = true;
                            Utility.PrintScene("\n퀘스트를 수락했습니다!");
                            Utility.PrintFree("아무키나 눌러 돌아가기",22);
                            Console.SetCursorPosition(0, 23);
                            return;
                        case 2:
                            Utility.PrintScene("\n퀘스트를 거절했습니다.");
                            Utility.PrintFree("아무키나 눌러 돌아가기", 22);
                            Console.SetCursorPosition(0, 23);
                            return;
                            
                    }
                }
                else if (CheckCompletion() && !IsCleared) // 퀘스트 완료 가능 상태
                {
                    switch (choice)
                    {
                        case 1:
                            IsCleared = true;
                            ReceiveReward();
                            return;
                        case 2:
                            return;
                    }
                }
            }
        }

        /// <summary>
        /// 퀘스트 완료 후 보상을 지급하는 메서드
        /// </summary>
        public void ReceiveReward()
        {

            Utility.PrintSceneW("보상을 받았습니다!: ");
            InventoryManager.Instance.GrantItem(RewardItem);
            GameManager.Instance.Player.Gold += RewardMoney;
            Utility.PrintSceneW($"{InventoryManager.Instance.GetItemNameById(RewardItem)} x 1,{RewardMoney}G 획득");
            Console.SetCursorPosition(0, 21);

            // 완료된 퀘스트 목록으로 이동
            HasAccepted = false;
            IsCleared = true;
        }
    }
}
