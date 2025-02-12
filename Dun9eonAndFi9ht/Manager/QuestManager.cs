using Dun9eonAndFi9ht.Quests;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Manager
{
    public class QuestManager
    {
        private static QuestManager? instance;
        public static QuestManager Instance => instance ??= new QuestManager();

        public List<Quest> AllQuests { get; set; }

        public event Action<Quest>? OnQuestCleared;

        public QuestManager()
        {
            AllQuests = new List<Quest>();
            Initialize();
        }
        /// <summary>
        /// 퀘스트 DB를 가져와 저장하는 함수
        /// </summary>
        public void Initialize()
        {
            int id = 0;
            while(true)
            {
                Dictionary<string, object> questData = DataTableManager.Instance.GetDBData("questTable", id);
                if (questData == null) break;

                string questTitle = questData["questtitle"].ToString();
                string[] message;
                message = Utility.ConvertObjToStrArr((List<object>)questData["questdescription"]);
                int rewardItem = Convert.ToInt32(questData["rewardItem"]);
                int rewardMoney = Convert.ToInt32(questData["rewardMoney"]);
                string type = questData["type"].ToString();

                Quest quest = null;

                switch(type)
                {
                    case "KillMonsterQuest":
                        quest = new KillMonsterQuest(
                            questTitle, message, rewardItem, rewardMoney,
                            questData["targetMonsterName"].ToString(),
                            Convert.ToInt32(questData["needKills"])
                        );
                        break;
                    case "ReachLevelQuest":
                        quest = new ReachLevelQuest(
                            questTitle, message, rewardItem, rewardMoney,
                            Convert.ToInt32(questData["targetLevel"])
                        );
                        break;
                    case "EquipItemQuest":
                        quest = new EquipItemQuest(
                            questTitle, message,
                            questData["requiredItemName"].ToString(),
                            rewardItem, rewardMoney
                        );
                        break;
                    default:
                        Console.WriteLine($"알 수 없는 퀘스트 타입: {type}");
                        continue;
                }
                if (quest != null) AllQuests.Add(quest);
                id++;
            }
        }


        /// <summary>
        /// 받은(수락한) 퀘스트 목록 가져오기
        /// </summary>
        public List<Quest> GetAcceptedQuests()
        {
            return AllQuests.FindAll(q => q.HasAccepted && !q.IsCleared);
        }

        /// <summary>
        /// 아직 받지 않은(수락하지 않은) 퀘스트 목록 가져오기
        /// </summary>
        public List<Quest> GetAvailableQuests()
        {
            return AllQuests.FindAll(q => !q.HasAccepted&&!q.IsCleared);
        }

        /// <summary>
        /// 모든 퀘스트 목록 가져오기
        /// </summary>
        public List<Quest> GetAllQuests()
        {
            return new List<Quest>(AllQuests);
        }
        /// <summary>
        /// 클리어한 퀘스트 목록 가져오기
        /// </summary>
        /// <returns></returns>
        public List<Quest> GetCompletedQuests()
        {
            return AllQuests.FindAll(q => q.IsCleared);
        }

        /// <summary>
        /// 어떤 퀘스트 클래스든 퀘스트 클리어 확인 메소드
        /// </summary>
        /// <returns></returns>
        public void CheckQuestCompletion()
        {
            foreach (var quest in AllQuests)
            {
                if (quest.CheckCompletion())
                {
                    Utility.PrintScene($"{quest.QuestTitle} 퀘스트 완료!");
                    quest.IsCleared = true;
                }
            }
        }
        /// <summary>
        /// 클리어한 퀘스트 목록 가져오기
        /// </summary>
        /// <returns></returns>
        public void ShowAllQuests()
        {
            foreach (var quest in AllQuests)
            {
                quest.ShowQuestInfo();
            }
        }

        /// <summary>
        /// 퀘스트 진행도 확인 메소드
        /// </summary>
        /// <returns></returns>
        public void CheckQuests()
        {
            foreach (var quest in GetAcceptedQuests())
            {
                if (!quest.IsCleared && quest.CheckCompletion())
                {
                    Utility.PrintScene($"[퀘스트 진행 완료] {quest.QuestTitle}");
                }
            }
        }

        public void InitializeQuest()
        {
            foreach(var quest in AllQuests)
            {
                if (quest.HasAccepted && !quest.IsCleared)
                {
                    quest.ResetProgress();
                    quest.HasAccepted = false;
                    quest.IsCleared = false;
                }
            }
        }
    }
}
