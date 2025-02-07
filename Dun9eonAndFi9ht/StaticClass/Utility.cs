using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.StaticClass
{
    public static class Utility
    {
        const int startLine = 15;
        static int currentLine = 15;

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
        /// 출력 시 일정 위치 아래에서 출력하게 만드는 코드 (0,15)
        /// </summary>
        /// <param name="message">출력 할 메시지</param>
        public static void PrintMenu(string message)
        {
            if (currentLine == startLine)
            {
                PrintLine();
            }
            Console.WriteLine(message);
            UpdateCurrentLine();
        }

        /// <summary>
        /// 여러 string 출력 시 사용할 코드 (0,15) 위치에서 계속해서 출력
        /// </summary>
        /// <param name="messages">출력할 메시지 string[]</param>
        public static void PrintMenu(string[] messages)
        {
            if (currentLine == startLine)
            {
                PrintLine();
            }
            foreach (string message in messages)
            {
                Console.WriteLine(message);
            }
            UpdateCurrentLine();
        }

        /// <summary>
        /// 화면 분할("=====") 출력
        /// </summary>
        public static void PrintLine()
        {
            int width = Console.WindowWidth;
            string line = new string('=', width);
            Console.SetCursorPosition(0, startLine);
            Console.WriteLine(line);
            UpdateCurrentLine();
        }

        /// <summary>
        /// 커서 위치 업데이트
        /// </summary>
        private static void UpdateCurrentLine()
        {
            currentLine = Console.CursorTop;
        }

        /// <summary>
        /// 화면 분할 아래쪽을 지워버리는 역할
        /// </summary>
        public static void ClearMenu()
        {
            for (int i = startLine + 1; i < currentLine; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new String(' ', Console.WindowWidth));
            }
            currentLine = 15;
        }
    }
}