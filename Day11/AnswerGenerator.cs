using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day11
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
            long result = 0;

            var board = new Board(_input.ToList());

            for (var i = 0; i < 100; i++)
            {
                result += board.Step();
                //board.Print();
            }

            return result;
        }

        public long Part2()
        {
            var board = new Board(_input.ToList());

            var all = board.MaxColumns * board.MaxRows;
            for (var i = 0; i < 300; i++)
            {
                var numberOfFlashes = board.Step();
                
                //Console.WriteLine(i);
                //board.Print();

                if (numberOfFlashes == all)
                {
                    return i + 1;
                }
            }

            return -1;
        }
    }

    public class Board
    {
        private readonly int[,] _rows;
        private bool[,] _flashed;
        
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
                    _rows[row, column] = int.Parse(line[row].ToCharArray()[column].ToString());
                }
            }

        }

        public int Count()
        {
            var result = 0;
            for (var i = 0; i < _flashed.GetLength(0); i++)
            {
                for (var j = 0; j < _flashed.GetLength(1); j++)
                {
                    result += _flashed[i, j] ? 1 : 0;
                }
            }

            return result;
        }

        public void Print()
        {
            Console.WriteLine();

            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(1); j++)
                {
                    Console.Write(_rows[i, j]);
                }

                Console.WriteLine();
            }
        }

        public long Step()
        {
            _flashed = new bool[MaxRows, MaxColumns];

            for (var row = 0; row < MaxRows; row++)
            {
                for (var column = 0; column < MaxColumns; column++)
                {
                    _rows[row, column]++;
                }
            }
            
            Flash();
            
            for (var row = 0; row < MaxRows; row++)
            {
                for (var column = 0; column < MaxColumns; column++)
                {
                    if (_rows[row, column] > 9)
                    {
                        _rows[row, column] = 0;
                    }
                }
            }

            return Count();
        }

        private void Flash()
        {
            for (var row = 0; row < MaxRows; row++)
            {
                for (var column = 0; column < MaxColumns; column++)
                {
                    if (_rows[row, column] > 9)
                    {
                        Flash(row, column);
                    }
                }
            }
        }

        private void Flash(int row, int column)
        {
            if (_flashed[row, column]) return;

            _flashed[row, column] = true;

            foreach (var adjacentPoint in GetAdjacentPoints(row, column))
            {
                _rows[adjacentPoint.Item1, adjacentPoint.Item2]++;
                if (_rows[adjacentPoint.Item1, adjacentPoint.Item2] > 9)
                {
                    Flash(adjacentPoint.Item1, adjacentPoint.Item2);
                }
            }
        }
        
        private IEnumerable<(int, int)> GetAdjacentPoints(int row, int column)
        {
            var result = new List<(int, int)>
            {
                (row - 1, column - 1),
                (row - 1, column),
                (row - 1, column + 1),
                (row,     column - 1),
                (row,     column + 1),
                (row + 1, column - 1),
                (row + 1, column),
                (row + 1, column + 1)
            };

            return result.Where(point => point.Item1 >= 0
                                         && point.Item1 < MaxRows
                                         && point.Item2 >= 0
                                         && point.Item2 < MaxColumns);
        }
    }
}