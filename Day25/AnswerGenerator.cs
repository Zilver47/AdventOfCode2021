using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day25
{
    public class AnswerGenerator : IAnswerGenerator
    {
        private readonly string[] _input;

        public AnswerGenerator(string[] input)
        {
            _input = input;
        }

        public long Part1()
        {
            var board = new Board(_input.ToList());
            for (var i = 0; i < 600; i++)
            {
                //board.Print();
                var result = board.Step();
                if (result)
                {
                    return i + 1;
                }
            }

            return -1;
        }

        public long Part2()
        {
            long result = -1;


            return result;
        }
    }
    public class Board
    {
        private int[,] _rows;

        public int MaxColumns { get; }
        public int MaxRows { get; }

        public Board(List<string> line)
        {
            MaxRows = line.Count;
            MaxColumns = line[0].Length;
            _rows = new int[MaxRows, MaxColumns];

            for (var row = 0; row < MaxRows; row++)
            {
                for (var column = 0; column < MaxColumns; column++)
                {
                    var c = line[row].ToCharArray()[column];
                    _rows[row, column] = c switch
                    {
                        '.' => 0,
                        '>' => 1,
                        _ => 2
                    };
                }
            }
        }

        public void Print()
        {
            Console.WriteLine();

            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(1); j++)
                {
                    var x = _rows[i, j];
                    Console.Write(x == 0 ? "." : x == 1 ? ">" : "v");
                }

                Console.WriteLine();
            }
        }

        public int[,] Copy()
        {
            var copy = new int[MaxRows, MaxColumns];

            Array.Copy(_rows, 0, copy, 0, _rows.Length);

            return copy;
        }

        public bool Step()
        {
            var oldMap = Copy();

            var newMap = new int[MaxRows, MaxColumns];
            for (var row = 0; row < _rows.GetLength(0); row++)
            {
                for (var column = _rows.GetLength(1) - 1; column >= 0; column--)
                {
                    switch (_rows[row, column])
                    {
                        case 0:
                            break;
                        case 1:
                            var next = GetNextEastFacingPoint(row, column);
                            if (_rows[next.Item1, next.Item2] != 0)
                            {
                                newMap[row, column] = _rows[row, column];
                                continue;
                            }

                            newMap[row, column] = 0;
                            newMap[next.Item1, next.Item2] = 1;
                            break;
                        case 2:
                            newMap[row, column] = _rows[row, column];
                            break;
                    }
                }
            }

            var newerMap = new int[MaxRows, MaxColumns];
            for (var row = newMap.GetLength(0) - 1; row >= 0; row--)
            {
                for (var column = 0; column < newMap.GetLength(1); column++)
                {
                    switch (newMap[row, column])
                    {
                        case 0:
                            break;
                        case 1:
                            newerMap[row, column] = newMap[row, column];
                            break;
                        case 2:
                            var next = GetNextSouthFacingPoint(row, column);
                            if (newMap[next.Item1, next.Item2] != 0)
                            {
                                newerMap[row, column] = newMap[row, column];
                                continue;
                            }

                            newerMap[row, column] = 0;
                            newerMap[next.Item1, next.Item2] = 2;
                            break;
                    }

                }
            }

            _rows = newerMap;
            return Compare(oldMap, _rows);
        }

        private bool Compare(int[,] oldMap, int[,] newMap)
        {
            for (var i = 0; i < newMap.GetLength(0); i++)
            {
                for (var j = 0; j < newMap.GetLength(1); j++)
                {
                    if (oldMap[i, j] != newMap[i, j]) return false;
                }
            }

            return true;
        }

        private (int, int) GetNextEastFacingPoint(int row, int column)
        {
            var newColumn = column + 1;
            if (newColumn >= MaxColumns) newColumn = 0;
            return (row, newColumn);
        }

        private (int, int) GetNextSouthFacingPoint(int row, int column)
        {
            var newRow = row + 1;
            if (newRow >= MaxRows) newRow = 0;
            return (newRow, column);
        }
    }
}