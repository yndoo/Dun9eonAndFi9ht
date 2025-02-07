using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.StaticClass
{
    public static class Utility
    {
        /// <summary>
        /// 입력값의 범위를 받고 입력을 실행. 유효 입력 값인지 검사 후 결과 반환
        /// </summary>
        /// <param name="min">입력 최소값</param>
        /// <param name="max">입력 최대값</param>
        /// <returns>에러코드(음수), 정상 입력값</returns>
        public static int UserInput(int min, int max)
        {
            string firstInput = Console.ReadLine();
            if (String.IsNullOrEmpty(firstInput))
            {
                return -1;       //빈 입력
            }
            int input;
            if (!int.TryParse(firstInput, out input))
            {
                return -2;      //숫자가 아님
            }
            if (input > max || input < min)
            {
                return -3;      //범위 내 숫자가 아님
            }
            return input;
        }

        /// <summary>
        /// 출력 시 일정 위치 아래에서 출력하게 만드는 코드 (0,15)
        /// </summary>
        /// <param name="message">출력 할 메시지</param>
        public static void PrintMenu(string message)
        {
            Console.SetCursorPosition(0, 15);
            Console.WriteLine(message);
        }

        /// <summary>
        /// 여러 string 출력 시 사용할 코드 (0,15) 위치에서 계속해서 출력
        /// </summary>
        /// <param name="messages">출력할 메시지 string[]</param>
        public static void PrintMenu(string[] messages)
        {
            Console.SetCursorPosition(0, 15);
            foreach (string message in messages)
            {
                Console.WriteLine(message);
            }
        }
    }
}