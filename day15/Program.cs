using System;
using System.Collections.Generic;

namespace day15
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(GetTurnResult(Numbers.Test2,30000000));
            Console.WriteLine(GetTurnResult(Numbers.Test3, 30000000));
            Console.WriteLine(GetTurnResult(Numbers.Test4, 30000000));
            Console.WriteLine(GetTurnResult(Numbers.Test5, 30000000));
            Console.WriteLine(GetTurnResult(Numbers.Test6, 30000000));
            Console.WriteLine(GetTurnResult(Numbers.Test7, 30000000));
            Console.WriteLine(GetTurnResult(Numbers.Input1, 30000000));
        }

        public static int GetTurnResult(List<int> numbers, int turnToCheck)
        {

            var mentionedNumbers = new Dictionary<int, List<int>>();
            var turn = 1;
            var lastMentionedNumber = 0;


            for (var i = 0; i < turnToCheck; i++)
            {
                var mentionedNumber = 0;
                if (i < numbers.Count)
                {
                    mentionedNumber = numbers[i];
                    if (i == numbers.Count - 1) lastMentionedNumber = mentionedNumber;
                }
                else
                {
                    var timesMentionedList = mentionedNumbers[lastMentionedNumber];
                    mentionedNumber = timesMentionedList.Count == 1 ? 0 : timesMentionedList[timesMentionedList.Count-1] - timesMentionedList[timesMentionedList.Count - 2];
                }

                //Console.WriteLine(mentionedNumber);
                lastMentionedNumber = mentionedNumber;


                if (mentionedNumbers.ContainsKey(mentionedNumber))
                {
                    mentionedNumbers[mentionedNumber].Add(turn);
                }
                else
                {
                    mentionedNumbers.Add(mentionedNumber, new List<int> { turn });
                }
                turn++;
            }
            return lastMentionedNumber;
        }

        public static class Numbers
        {
            public static List<int> Test = new List<int>
            {
                0,3,6
            };

            public static List<int> Test2 = new List<int>
            {
                1,3,2
            };

            public static List<int> Test3 = new List<int>
            {
                2,1,3
            };

            public static List<int> Test4 = new List<int>
            {
                1,2,3
            };

            public static List<int> Test5 = new List<int>
            {
                2,3,1
            };

            public static List<int> Test6 = new List<int>
            {
                3,2,1
            };

            public static List<int> Test7 = new List<int>
            {
                3,1,2
            };

            public static List<int> Input1 = new List<int>
            {
                9,3,1,0,8,4
            };
        }
    }
}


