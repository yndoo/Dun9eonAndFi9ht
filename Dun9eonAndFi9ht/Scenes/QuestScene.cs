using DataDefinition;
using Dun9eonAndFi9ht.Manager;
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
            Utility.ClearScene();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Utility.PrintScene("Quest!!\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW("1");
            Console.ResetColor();
            Utility.PrintScene(". 마을을 위협하는 미니언 처치");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW("2");
            Console.ResetColor();
            Utility.PrintScene(". 장비를 장착해보자");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Utility.PrintSceneW("3");
            Console.ResetColor();
            Utility.PrintScene(". 더욱 더 강해지기!");

            Utility.PrintMenu("원하시는 퀘스트를 선택해주세요.\n>>");

            int selectCase = 0;
            Utility.UserInput(1, 3);

            switch(selectCase)
            {
                case 0:
                    return ESceneType.QuestScene;
                default:
                    return ESceneType.QuestScene;
                case 1:
                    Dictionary<string, object> data= DataTableManager.Instance.GetDBData("questTable", 0);
                    
                    string[] message = Utility.ConvertObjToStrArr((List<object>)data["questdescription"]);

                    Quest quest = new Quest(
                        data["questtitle"].ToString(),
                        message, 
                        data["targetMonster"].ToString(),
                        Convert.ToInt32(data["needKills"]), 
                        Convert.ToInt32(data["rewardItem"]), 
                        Convert.ToInt32(data["rewardMoney"])
                        );
                    quest.ShowQuestInfo();
                    Console.ReadKey();
                    break;
            }
            return ESceneType.QuestScene;

        }
    }
}
