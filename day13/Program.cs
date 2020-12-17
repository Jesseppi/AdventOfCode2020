using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace day13
{
    class Program
    {
        static void Main(string[] args)
        {

            foreach (var time in JourneyPlanner.Journey)
            {
                Console.WriteLine( GetClosestBusTime(time.Key, time.Value));
            }
        }

        public static int GetClosestBusTime(long arrivalTime, string buses)
        {
            var arrivalTimeDouble = Convert.ToDouble(arrivalTime);
            var busItems = buses.Split(",").ToList().Where(x => Regex.Match(x, @"\d").Success).Select(y => Int32.Parse(y)).ToList();

            var closestDifference = (0,1000);

            foreach (var bus in busItems)
            {
                var BusTime = arrivalTime / Convert.ToDouble(bus);
                var floatNumber = (BusTime - Math.Truncate(BusTime));
                var difference = bus - (bus * floatNumber);
                if(difference < closestDifference.Item2) closestDifference = (bus,Convert.ToInt32(difference));
            }

            return closestDifference.Item1 * closestDifference.Item2;


        }

        public static class JourneyPlanner
        {
            public static Dictionary<long, string> Test = new Dictionary<long, string>
            {
                {939,"7,13,x,x,59,x,31,19"}
            };

            public static Dictionary<long, string> Journey = new Dictionary<long, string>
            {
                {1002561,"17,x,x,x,x,x,x,x,x,x,x,37,x,x,x,x,x,409,x,29,x,x,x,x,x,x,x,x,x,x,13,x,x,x,x,x,x,x,x,x,23,x,x,x,x,x,x,x,373,x,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,19"}
            };
        }
    }
}


