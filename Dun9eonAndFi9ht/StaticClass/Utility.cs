using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.StaticClass
{
    public static class Utility
    {
        const int startMenuLine = 15;
        static int currentMenuLine = 16;
        const int startSceneLine = 0;
        static int currentSceneLine = 0;

        /// <summary>
        /// 입력값의 범위를 받고 입력을 실행. 유효 입력 값인지 검사 후 결과 반환
        /// </summary>
        /// <param name="min">입력 최소값</param>
        /// <param name="max">입력 최대값</param>
        /// <returns>에러코드(음수), 정상 입력값</returns>
        public static int UserInput(int min, int max)
        {
            string? firstInput = Console.ReadLine();
            if (String.IsNullOrEmpty(firstInput)) return -1;       //빈 입력
            if (!int.TryParse(firstInput, out int input)) return -2;      //숫자가 아님
            if (input > max || input < min) return -3;      //범위 내 숫자가 아님
            return input;
        }

        /// <summary>
        /// string 출력 시 (0,15) 위치에서 계속해서 출력
        /// </summary>
        /// <param name="message">출력 할 메시지</param>
        public static void PrintMenu(string message)
        {
            EnsurePrintLine();
            Console.SetCursorPosition(0, currentMenuLine);
            Console.WriteLine(message);
            UpdateMenuLine();
        }

        /// <summary>
        /// 여러 string 출력 시 (0,15) 위치에서 계속해서 출력
        /// </summary>
        /// <param name="messages">출력할 메시지 string[]</param>
        public static void PrintMenu(string[] messages)
        {
            EnsurePrintLine();
            Console.SetCursorPosition(0, currentMenuLine);
            foreach (string message in messages)
            {
                Console.WriteLine(message);
            }
            UpdateMenuLine();
        }
        /// <summary>
        /// string 출력시 (0,15) 위치 아래에서 줄바꿈 없이 출력
        /// </summary>
        /// <param name="message">출력 메시지</param>
        public static void PrintMenuW(string message)
        {
            EnsurePrintLine();
            Console.SetCursorPosition(0, currentMenuLine);
            Console.Write(message);
            UpdateMenuLine();
        }

        /// <summary>
        /// string 출력 시 위쪽에서 출력하게 만드는 코드
        /// </summary>
        /// <param name="message"></param>
        public static void PrintScene(string message)
        {
            Console.SetCursorPosition(0, currentSceneLine);
            Console.WriteLine(message);
            UpdateSceneLine();
            EnsurePrintLine();
        }

        /// <summary>
        /// 여러 string 출력 시 위쪽에서 출력하게 만드는 코드
        /// </summary>
        /// <param name="messages"></param>
        public static void PrintScene(string[] messages)
        {
            Console.SetCursorPosition(0, currentSceneLine); // 화면 최상단(0,0)에서 시작
            foreach (string message in messages)
            {
                Console.WriteLine(message);
            }
            UpdateSceneLine();
            EnsurePrintLine();
        }
        /// <summary>
        /// string 출력 시 위쪽에서 줄바꿈 없이 출력
        /// </summary>
        /// <param name="message">출력할 메시지</param>
        public static void PrintSceneW(string message)
        {
            Console.SetCursorPosition(0, currentSceneLine);
            Console.Write(message);
            UpdateSceneLine();
            EnsurePrintLine();
        }



        /// <summary>
        /// string을 자유 위치에서 적을 수 있도록 만드는 코드, 단 중심줄은 안된다.
        /// </summary>
        /// <param name="message">적을 메시지</param>
        /// <param name="line">적을 줄</param>
        public static void PrintFree(string message, int line)
        {
            if(line != startMenuLine)
            {
                Console.SetCursorPosition(0, line);
                Console.WriteLine(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, line);
                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// 화면 분할 아래쪽을 지워버리는 역할
        /// </summary>
        public static void ClearMenu()
        {
            for (int i = startMenuLine + 1; i <= currentMenuLine; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new String(' ', Console.WindowWidth));
            }
            currentMenuLine = startMenuLine + 1;
        }

        /// <summary>
        /// 화면 분할 위쪽 지워버리는 역할
        /// </summary>
        public static void ClearScene()
        {
            for (int i = startSceneLine; i < startMenuLine; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new String(' ', Console.WindowWidth));
            }
            currentSceneLine = startSceneLine;
        }

        /// <summary>
        /// 모든 화면 지우는 역할(구분선은 남습니다.)
        /// </summary>
        public static void ClearAll()
        {
            ClearScene();
            ClearMenu();
        }

        /// <summary>
        /// 구분선이 있는지 확인하고 없으면 그리는 역할
        /// </summary>
        public static void EnsurePrintLine()
        {
            Console.SetCursorPosition(0, startMenuLine);
            if (Console.CursorLeft == 0)
            {
                PrintLine();
            }
        }

        /// <summary>
        /// 화면 분할("=====") 출력
        /// </summary>
        private static void PrintLine()
        {
            int width = Console.WindowWidth;
            string line = new string('=', width);
            Console.SetCursorPosition(0, startMenuLine);
            Console.WriteLine(line);
        }

        private static void UpdateMenuLine()
        {
            currentMenuLine = Console.CursorTop;
        }

        private static void UpdateSceneLine()
        {
            currentSceneLine = Console.CursorTop;
        }
    }
}