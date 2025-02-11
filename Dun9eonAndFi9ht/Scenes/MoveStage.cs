using DataDefinition;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Scenes
{
    internal class MoveStage: Scene
    {
        public override ESceneType Start()
        {
            int stage = Dungeon.stage;
            int maxStageCleared = Dungeon.maxStageCleared;
            int nextinput = -1;
            while (nextinput < 0)
            {
                Utility.ClearAll();
                Utility.PrintScene($"현재 위치한 층을 이동합니다. (현재 {stage}층)");
                Utility.PrintScene($"최고층 : {maxStageCleared + 1}층");
                Utility.PrintMenu("층을 입력해 주세요 (0. 나가기)");
                Utility.PrintMenu(">> ");
                nextinput = Utility.UserInput(0, maxStageCleared + 1);
                if (nextinput == 0) // 유저가 취소를 선택한 경우
                {
                    Utility.PrintMenu("이전 메뉴로 돌아갑니다.");
                    return ESceneType.StartScene;
                }
                else if (nextinput < 1 || nextinput > maxStageCleared + 1) // 범위를 벗어난 경우
                {
                    Utility.ClearMenu();
                    Utility.PrintMenu("잘못된 입력입니다.");
                    Utility.PrintMenu("아무 키나 입력\n>>");
                    Console.ReadLine();
                    nextinput = -1; // 루프를 계속 돌도록 초기화
                }
            }
            Utility.PrintScene($"{nextinput}층으로 이동합니다.");
            stage = nextinput;
            Utility.ClearMenu();
            Utility.PrintMenu("아무 키나 입력하세요");
            Console.Write(">> ");
            Console.ReadLine();
            return ESceneType.StartScene;
        }
    }
}
