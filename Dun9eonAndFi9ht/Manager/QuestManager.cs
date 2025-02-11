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

        public QuestManager()
        {
            AllQuests = new List<Quest>();
        }
        


        public void AddQuest(Quest quest)
        {
            AllQuests.Add(quest);
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
            return AllQuests.FindAll(q => !q.HasAccepted);
        }

        /// <summary>
        /// 모든 퀘스트 목록 가져오기
        /// </summary>
        public List<Quest> GetAllQuests()
        {
            return new List<Quest>(AllQuests);
        }

        public List<Quest> GetCompletedQuests()
        {
            return AllQuests.FindAll(q => q.IsCleared);
        }

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

        public void ShowAllQuests()
        {
            foreach (var quest in AllQuests)
            {
                quest.ShowQuestInfo();
            }
        }


    }
}
