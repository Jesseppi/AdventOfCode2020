﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace day11
{
    class Program
    {
        static void Main(string[] args)
        {
            Seater.OrganiseChaos(SeatLayout.Seats);
        }

        public static class Seater
        {

            public static void OrganiseChaos(List<string> seats)
            {

                var seatingPlan = new SeatingPlan
                {
                    SeatLayout = new List<List<char>>(),
                    layoutHasChanged = true
                };

                foreach (var str in seats)
                {
                    var charList = new List<char>();
                    charList.AddRange(str);
                    seatingPlan.SeatLayout.Add(charList);
                }

                var rowNumber = seatingPlan.SeatLayout.Count;
                var columnNumber = seatingPlan.SeatLayout[0].Count;

                var loopCount = 0;

                while (seatingPlan.layoutHasChanged == true)
                {
                    loopCount++;
                    var newSeatingLayout = new List<List<char>>();
                    seatingPlan.layoutHasChanged = false;
                    for (var row = 0; row < rowNumber; row++)
                    {
                        newSeatingLayout.Add(new List<char>());
                        for (var column = 0; column < seatingPlan.SeatLayout[row].Count; column++)
                        {
                            var item = seatingPlan.SeatLayout[row][column];
                            newSeatingLayout[row].Add(item);
                            var newValue = GetSeatValue(seatingPlan.SeatLayout, row, column, 5);
                            if (item != newValue.Item1) seatingPlan.layoutHasChanged = true;
                            newSeatingLayout[row][column] = newValue.Item1;
                        }
                        //Console.WriteLine();
                    }
                    Console.WriteLine();
                    seatingPlan.SeatLayout = newSeatingLayout;
                    //seatingPlan.layoutHasChanged = false;

                    var emptySeats = 0;
                    var occupiedSeats = 0;
                    foreach (var row in seatingPlan.SeatLayout)
                    {
                        foreach (var column in row)
                        {
                            if (column == '#') occupiedSeats++;
                            if (column == 'L') emptySeats++;
                            Console.Write(column);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine($"Number of Empty Seats: {emptySeats}");
                    Console.WriteLine($"Number of Occupied Seats: {occupiedSeats}");
                    Console.WriteLine();
                }
            }

            public class SeatingPlan
            {
                public List<List<char>> SeatLayout { get; set; }

                public bool layoutHasChanged { get; set; } = false;
            }

            public static (char, int) GetSeatValue(List<List<char>> seats, int currentRow, int currentColumn, int rule)
            {
                var emptySeatCount = 0;
                var occupiedSeatCounter = 0;

                var currentSeatType = seats[currentRow][currentColumn];

                //Console.WriteLine("Checking rows");
                var rowsToCheck = GetCellsToCheck(currentRow, seats.Count);
                //Console.WriteLine("Checking columns");
                var ColumnsToCheck = GetCellsToCheck(currentColumn, seats[0].Count);

                var checkCells = 0;
                foreach (var row in rowsToCheck)
                {
                    foreach (var column in ColumnsToCheck)
                    {
                        var direction = GetDirection(currentRow, currentColumn, row, column);
                        if (row == currentRow && column == currentColumn) continue;
                        checkCells++;
                        //Console.WriteLine($"(row:{row} column:{column})");
                        var seatType = seats[row][column];
                        if (seatType == '.') {
                            var deeperseatType = GoDeeper(seats,direction,row,column);
                            seatType = deeperseatType;
                        }
                        if (seatType == '#') occupiedSeatCounter++;
                        if (seatType == 'L') emptySeatCount++;
                    }
                }

                //Console.Write(checkCells);
                if (currentSeatType == 'L' && occupiedSeatCounter == 0) return ('#', emptySeatCount);
                if (currentSeatType == '#' && occupiedSeatCounter >= rule) return ('L', emptySeatCount);
                return (currentSeatType, emptySeatCount);
            }

            public static List<int> GetCellsToCheck(int currentCell, int cellCount)
            {
                var rowsToCheck = Enumerable.Range(currentCell - 1, 3).Where(x => x >= 0 && x < cellCount).ToList();
                return rowsToCheck;
            }

            public enum Direction {
                UP,
                DOWN,
                LEFT,
                LEFTUP,
                LEFTDOWN,
                RIGHT,
                RIGHTUP,
                RIGHTDOWN,
                NEUTRAL
            }

            public static Direction GetDirection(int currentRow, int currentColumn, int row, int column){
                if(row < currentRow ){
                    if(column <  currentColumn) return Direction.LEFTUP;
                    if (column > currentColumn) return Direction.RIGHTUP;
                    return Direction.UP;
                }

                if(row == currentRow){
                    if (column < currentColumn) return Direction.LEFT;
                    if (column > currentColumn) return Direction.RIGHT;
                }

                if (row > currentRow)
                {
                    if (column < currentColumn) return Direction.LEFTDOWN;
                    if (column > currentColumn) return Direction.RIGHTDOWN;
                    return Direction.DOWN;
                }
                return Direction.NEUTRAL;
            }

            public static char GoDeeper(List<List<char>> seats, Direction direction,int row, int colum){
                var newRow = row;
                var newColumn = colum;
                if(direction == Direction.UP ){
                    newRow--;
                }
                if (direction == Direction.DOWN)
                {
                    newRow++;
                }
                if (direction == Direction.LEFT)
                {
                    newColumn--;
                }
                if (direction == Direction.LEFTUP)
                {
                    newColumn--;
                    newRow--;
                }
                if (direction == Direction.LEFTDOWN)
                {
                    newColumn--;
                    newRow++;
                }
                if (direction == Direction.RIGHT)
                {
                    newColumn++;
                }
                if (direction == Direction.RIGHTUP)
                {
                    newColumn++;
                    newRow--;
                }
                if (direction == Direction.RIGHTDOWN)
                {
                    newColumn++;
                    newRow++;
                }

                if( newRow < 0 || newRow >= seats.Count  || newColumn < 0 || newColumn >= seats[0].Count ){
                    return '.';
                }
                var seatType = seats[newRow][newColumn];

                if (seatType == '.') seatType = GoDeeper(seats, direction, newRow, newColumn);
                return seatType;
            }
        }
        public static class SeatLayout
        {
            public static List<string> testSeats = new List<string>{
                "L.LL.LL.LL",
                "LLLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLLL",
                "L.LLLLLL.L",
                "L.LLLLL.LL"
            };

            public static List<string> testSeats2 = new List<string>{
                "..........",
                ".....L....",
                "..........",
            };
            public static List<string> Seats = new List<string>
        {
            "LLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLL.LLLL.L.LLLLLLL.LLLLLLL.LLLLLL.LL.LLLL.LLLLLLLLLLLLLLL",
            "LLL.LLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLL.LLLLLLL.LLLL.LLL....LLL.LLL.LLLLLLL.LLL.LLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLL.LLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "LLLL.LLLL.LLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLL.LLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLL.LLLL.LLLLLLL.L.LLL.LLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLL.LLL..LL.LLL",
            "LLLLLLLLL.LLLLLLLLLLL.LLLLL.LLL.LLLLLL.LLLL.LL.L.LLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLL.L.LLLLLLLLLL.LLL.LLLL.LLLLLLLLLLLLL.LLL.LLLLLLLLLLLLLLLLLL.LLLLLLLLLLL",
            "....L..L....L...L.......L.LL.....L..L...LLLL.LLL..L.L...LL..L...LL.....L..LL.....L....L.L..L",
            "LLLLLLLLLLLLLL.LLLLLL.LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLL.LLL.LLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLL.LL.LLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLL..LLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLL.LLLLLLLLL.LLLLLLLLLLLLLL.LLLLLL.LL..LLL.LLLLLLLLLLLLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLL",
            "LLL..LLLL.LLLLL.LLLLLLLL.LLLLLLLLLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLL.LLLLLLLLLLL.LLLL.LLLLLLLLL.L.LLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLL.LLLL.LLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLL.LLL.LLLLL.LLLLLLLLLLLLLLLLLLLLL..LLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            ".L..L......L.L....L...L..........L.L....L...LL....LL....LL...LLL....LL..L...L...L.........L.",
            "LLLLLLLLL.LLLLLLLLL.LLLLLLLLLLL.LLL.LL.LLLLLLLLLLLLLLLLLLLLL..LLLLLLLLLLLLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLL.LLLLLLL.LLLLLL.LLLLLLLLLLL.LL.LLLLLLL.LLLLLLL.L.LLL.LLLL.LLLLLLLLLLL",
            "LLLLLL.LL.LLLL.LLLLLL.LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLL.LL.LLLLLLLLLLLLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLL..LLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLL.L.LLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLLLLLL.LLLL.LL.L.LLLLLLLLLLLLLL.L.LLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLLLLLLLLLLLL.LL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "..L.L.L..L.L.....LL..L........L..L....L...LL.........L.L.L.L..L......L.........L..L....L....",
            "LLLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLL.L.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLL.L.LLLLLLLLLLLLLLL.LLLL",
            "LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLL",
            "LLLL.LLLL.LLLLLLLLLL..LLLLLLLLL.LLLLLLLLLLL.LLL.LLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLLLLL",
            "LLLL.LLLL.LLL..LLLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLL",
            "LLLLL.LLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLL.LLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLLLLL.LLLL.LLLLLLL.L.LL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLL.L.LLLLLLLL.LL.LLLLLLLL",
            "LLLLLLL.LLLLLL.LLLLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLLLLL",
            "L...L.......LL.LL.L....L...L.....L.......L.........L.L.L..LL.L.L.L.L..L....L.L...L.L.LL.....",
            "LL.LLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLL..LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "LLLLL.LLL.LLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLL.LLL.LLLLLL.",
            "LLLLLLLLL.LLLL.LLLLLL.LLLLLLLLL.LLLLLL..LLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.L.LLLLLLLL.",
            "LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLL..LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLL.LL",
            "..LLL.L..L..L..L..L..L.L...LL..L.....LLL.L..LL.....LLL.......L.L...L........LL........L....L",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLL.LLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLL",
            "L.LLLLLLL.LLLL.LLLLLL.LLLLLLLLL.LL.LLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LL.LL...L......L.L.....L.........L....L...L..L..LLL.L...L......L.L.....LL..LL..LLL....L..L.L",
            "LLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLL",
            "LL.LLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLL.L.LLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL.L.LLLLLL.L.LLLLLLLLL",
            "LLLLLLL.L.LLLL.LLLL.L.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLL.LL",
            "L..L.L....L......LLLL...L....LL...L..LLL......LLL.LL.LLL...LL..L...LLL........L.....LL.L.LLL",
            "LLLLLLLLL.LLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLL.LL.LLLLLLLL.LLLLLLLLLLL",
            "LLLL.LLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLL.L.LLLLLLL.LLLLLLL.LLLLLLL.L.LL.LLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLL..LLLLLL.LLLL.LLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "LL.......L......L...LL.......LL.LL.LL.L.LL.L..L.......L.L.LLLL.LL...LL......LLL..L....LLL...",
            "LLLLLLLLL.LLL..LLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLL.LLLLL.LLLLLLLL.LL.LLLLLLLLL.L.LLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL.LLL",
            "LLLLLLLLLLLLLLLLLLLLL.L.LLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLL.LLLLLLLLL.L.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLL.LLLLLL.LLLLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLLLLLL.LLLLLLL..LLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLL.LLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLL.L.LLLLLLLLLLLLLLLLLLLL",
            "...L..L..L..L...L.........L.L.L..LLL.....LL......L..LLL..L.L.......L.L.L..LLL...........L...",
            "LLLLLLLLL.LLLL..LLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLL.LLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLL.LLLLLLLLLLLLLLLLL.LLLLLL.LLLLLL.LLLL.L.LLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLL.L.LLLLLLLLL",
            "LLLLLLLLL.LLLL.LLL..L.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.L.LLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLLL.LLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLL.LLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLL.LLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLL.LLLLLLLLLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLL.LLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLLL.LLLLLLL.LL.LLLLLLLLL.LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL..LLLLLLLLLLLLLLLLLLL",
            "..L.LLL.....L....L.L.L..L.L..L.L..LL.......L.L.L......L..L..L..L..L.......L...LL..LL..L...LL",
            "LLLLLLLLL.LL.LLLL.LLL.LLLLLLLLLL.LLLLL.LLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL..LLLLLL.L.LLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLL.LL.LLLLLL.LLLLLL.L.LL.LLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLLL.LL.LLLLLL.LLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLL.L.LLLLLLL.LLLLLL.LLLL.LLLLLLLLLLLLLLLLL.LLL.LLLL..LLLLLLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLL..LLLLLLLLLLLLLLLL.LLLL.L.LLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL..LL.LLLLL.LLLLLLLLLLL",
            "LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLLLL.L",
            "LLLL.LLLL.LLLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLL.LLLL",
            "LLLLLLLLLLLLLLLLLLLLL.LLLLLLLL..LLLLLLLLLLL.LLLLLLLLL.L.LLLLLLLLLLLLLLL.LLLLLLLL..LLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLL.LLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLL.LLL.LLLLLLLLL.LLLLLL.LLLL.LLLLLLLLL.LLLLLL..LLLLLL.LLLLLLLLLLLLLLLLLLLLLLL",
            "L.LLLLLLLL.LLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLL.LLLLL.LLL.LLLLLL..LLLLLLLLLLLLLLLLLL.LLL.LLLLLLL",
        };

        }
    }
}
