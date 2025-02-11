using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht
{
    public class Quest
    {
        public string QuestTitle { get; set; } // 퀘스트 제목
        public string QuestDescription { get; set; } // 퀘스트 내용 설명
        public string TargetMonster { get; set; } // 목표 몬스터 이름
        public int NeedKills { get; set; } // 잡아야 할 몬스터 수
        public int CurrentKills { get; set; } // 현재 잡은 몬스터 수
        public string RewardItem { get; set; } // 보상 아이템
        public int RewardMoney { get; set; } // 보상 금액
        public bool HasAccepted; //퀘스트를 수주한 상태인지 판단
        public bool IsCleared; //퀘스트 클리어 여부 판단
        public bool IsGoal; //잡아야 할 몬스터 수를 모두 채웠는지 여부 판단


        //퀘스트 생성자
        public Quest(string questtitle, string questdescription, string targetMonster, int needKills, string rewardItem, int rewardMoney)
        {
            QuestTitle = questtitle;
            QuestDescription = questdescription;
            TargetMonster = targetMonster;
            NeedKills = needKills;
            CurrentKills = 0; // 퀘스트 시작 시 잡은 몬스터 수 0으로 초기화
            RewardItem = rewardItem;
            RewardMoney = rewardMoney;
        }

        /// <summary>
        /// 퀘스트 진행 상황 출력 메서드
        /// </summary>
        public virtual void ShowQuestInfo()
        {
            Console.WriteLine($"퀘스트 제목: {QuestTitle}");
            Console.WriteLine();
            Console.WriteLine($"내용: {QuestDescription}");
            Console.WriteLine();
            Console.WriteLine($"{TargetMonster} {NeedKills}마리 처치 ({CurrentKills} / {NeedKills})");
            Console.WriteLine();
            Console.WriteLine("-보상-");
            Console.WriteLine($"{RewardItem}");
            Console.WriteLine($"{RewardMoney}G");
            Console.WriteLine();
            ShowSelect();
        }

        /// <summary>
        /// 퀘스트 진행 상황에 따라 다른 선택지를 보여주는 메서드
        /// </summary>
        public void ShowSelect()
        {
            if (HasAccepted == false)
            {
                Console.WriteLine("1. 수락");
                Console.WriteLine("2. 거절");
            }
            else if (CurrentKills < NeedKills)
            {
                Console.WriteLine("클리어 조건이 충족되지 않았습니다.");
                Console.WriteLine("0. 돌아가기");
            }
            else
            {
                Console.WriteLine("1. 보상 받기");
                Console.WriteLine("2. 돌아가기");
            }
        }

        /// <summary>
        /// 몬스터 처치 시 현재 잡은 몬스터 수 누적시키는 메서드
        /// </summary>
        public void AddKillCount()
        {
            if (CurrentKills < NeedKills)
            {
                CurrentKills++;
            }
        }
    }
}
