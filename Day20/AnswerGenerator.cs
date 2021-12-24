using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day20
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
            var algorithm = _input[0];
            var board = new Board(_input.Skip(2).ToList(), algorithm);

            for (var steps = 0; steps < 2; steps++)
            {
                board.Enlarge(steps + 1);
                board.Enhance();
            }

            return board.Count();
        }

        public long Part2()
        {
            var algorithm = _input[0];
            var board = new Board(_input.Skip(2).ToList(), algorithm);

            for (var steps = 0; steps < 50; steps++)
            {
                board.Enlarge(steps + 1);
                board.Enhance();
            }

            return board.Count();
        }
    }

    public class Board
    {
        private readonly string _algorithm;
        private  bool[,] _rows;
        
        public int MaxColumns { get; private set; }
        public int MaxRows { get; private set; }

        public Board(List<string> lines, string algorithm)
        {
            _algorithm = algorithm;
            MaxRows = lines.Count;
            MaxColumns = lines[0].Length;
            _rows = new bool[MaxRows, MaxColumns];

            for (var row = 0; row < MaxRows; row++)
            {
                for (var column = 0; column < MaxColumns; column++)
                {
                    _rows[row, column] = lines[row].ToCharArray()[column] == '#';
                }
            }
        }

        public void Enlarge(int times)
        {
            var result = new bool[MaxRows + 2, MaxColumns + 2];
            if (times % 2 == 1)
            {
                for (var i = 0; i < _rows.GetLength(0); i++)
                {
                    for (var j = 0; j < _rows.GetLength(1); j++)
                    {
                        result[i + 1, j + 1] = _rows[i, j];
                    }
                }
            }
            else
            {
                for (var i = 0; i < result.GetLength(0); i++)
                {
                    for (var j = 0; j < result.GetLength(1); j++)
                    {
                        result[i, j] = _algorithm[0] == '#';
                    }
                }
                
                for (var i = 0; i < _rows.GetLength(0); i++)
                {
                    for (var j = 0; j < _rows.GetLength(1); j++)
                    {
                        result[i + 1, j + 1] = _rows[i, j];
                    }
                }
            }

            _rows = result;
            MaxRows += 2;
            MaxColumns += 2;
        }

        public int Count()
        {
            var result = 0;
            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(1); j++)
                {
                    result += _rows[i, j] ? 1 : 0;
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
                    Console.Write(_rows[i, j] ? '#': '.');
                }

                Console.WriteLine();
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
                (row,     column),
                (row,     column + 1),
                (row + 1, column - 1),
                (row + 1, column),
                (row + 1, column + 1)
            };

            return result;
        }

        private bool IsVisible((int row, int column) point)
        {
            return point.row >= 0
                   && point.row < MaxRows
                   && point.column >= 0
                   && point.column < MaxColumns;
        }

        public void Enhance()
        {
            var result = new bool[MaxRows, MaxColumns];
            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(1); j++)
                {
                    result[i, j] = EnhancePixel((i, j));
                }
            }

            _rows = result;
        }

        private bool EnhancePixel((int row, int column) point)
        {
            var numberOfBinaryString = string.Empty;
            var points = GetAdjacentPoints(point.row, point.column);
            foreach (var p in points)
            {
                if (IsVisible(p))
                {
                    numberOfBinaryString += _rows[p.Item1, p.Item2] ? 1 : 0;
                }
                else
                {
                    numberOfBinaryString += _rows[point.row, point.column] ? 1 : 0;
                }
            }

            var number = Convert.ToInt32(numberOfBinaryString, 2);
            return _algorithm[number] == '#';
        }
    }
}