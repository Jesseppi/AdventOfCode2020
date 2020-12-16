using System;
using System.Collections.Generic;
using System.Linq;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Joltage.GetJoltDifferences(Adapters.BigBag));
        }

        public static class Joltage
        {

            public static int GetJoltDifferences(List<int> bag)
            {
                var maxValue = bag.Max();
                var maxIndex = maxValue + 1;
                var sortedArray = new int[maxIndex];
                bag.ForEach(item => sortedArray[item] = item);
                var singleJolts = 1;
                var doubleJolts = 0;
                var tripleJolts = 1;

                var chain = new List<int>();

                var nextAdapter = bag.Min();
                while(nextAdapter < maxValue)
                {
                    chain.Add(nextAdapter);
                    var currentAdapter = sortedArray[nextAdapter];
                    var maxAdapter = currentAdapter+3;

                    if (sortedArray[currentAdapter + 1] != 0) {
                        singleJolts++;
                        nextAdapter ++;
                        continue;
                    }
                    if ( maxAdapter <= maxValue && sortedArray[maxAdapter] != 0){
                        tripleJolts++;
                        nextAdapter += 3;
                    };
                    continue;
                }

                chain.ForEach(x => Console.Write(x + " "));
                Console.WriteLine();
                Console.WriteLine(singleJolts);
                Console.WriteLine(tripleJolts);
                return singleJolts * tripleJolts;

            }

            public static int GetPathNumbers(List<int> bag){
                var maxValue = bag.Max();
                var maxIndex = maxValue + 1;
                var sortedArray = new int[maxIndex];
                bag.ForEach(item => sortedArray[item] = item);
                var singleJolts = 1;
                var doubleJolts = 0;
                var tripleJolts = 1;

                var chain = new List<int>();

                foreach (var adapter in bag)
                {
                    var currentAdapter = sortedArray[nextAdapter];
                    var maxAdapter = currentAdapter + 3;

                    if (sortedArray[currentAdapter + 1] != 0)
                    {
                        singleJolts++;
                        continue;
                    }
                    if (maxAdapter <= maxValue && sortedArray[maxAdapter] != 0)
                    {
                        tripleJolts++;
                        nextAdapter += 3;
                    };
                    continue;
                }

                chain.ForEach(x => Console.Write(x + " "));
                Console.WriteLine();
                Console.WriteLine(singleJolts);
                Console.WriteLine(tripleJolts);
                return singleJolts * tripleJolts;
            }
        }



        public static class Adapters
        {
            public static List<int> Bag = new List<int>
            {
                16,
                10,
                15,
                5,
                1,
                11,
                7,
                19,
                6,
                12,
                4,
            };

            public static List<int> MediumBag = new List<int>{
                28,
                33,
                18,
                42,
                31,
                14,
                46,
                20,
                48,
                47,
                24,
                23,
                49,
                45,
                19,
                38,
                39,
                11,
                1,
                32,
                25,
                35,
                8,
                17,
                7,
                9,
                4,
                2,
                34,
                10,
                3
            };

            public static List<int> BigBag = new List<int>
            {
                59,
                134,
                159,
                125,
                95,
                92,
                169,
                43,
                154,
                46,
                110,
                79,
                117,
                151,
                141,
                56,
                87,
                10,
                65,
                170,
                89,
                32,
                40,
                118,
                36,
                94,
                124,
                173,
                164,
                166,
                113,
                67,
                76,
                102,
                107,
                52,
                144,
                119,
                2,
                72,
                86,
                73,
                66,
                13,
                15,
                38,
                47,
                109,
                103,
                128,
                165,
                148,
                116,
                146,
                18,
                135,
                68,
                83,
                133,
                171,
                145,
                48,
                31,
                106,
                161,
                6,
                21,
                22,
                77,
                172,
                28,
                78,
                96,
                55,
                132,
                39,
                100,
                108,
                33,
                23,
                54,
                157,
                80,
                153,
                9,
                62,
                26,
                147,
                1,
                27,
                131,
                88,
                138,
                93,
                14,
                123,
                122,
                158,
                152,
                71,
                49,
                101,
                37,
                99,
                160,
                53,
                3
            };
        }
    }
}
