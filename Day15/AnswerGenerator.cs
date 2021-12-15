using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Extensions;
using AdventOfCode.Day13;

namespace AdventOfCode.Day15
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
            long result = -1;

            var board = new Board(_input.ToList());
            
            result = board.FindPath();

            return result;
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
        private int _maxColumns;
        private int _maxRows;
        private long _minimalTotalRisk;

        public Board(List<string> lines)
        {
            _maxRows = lines.Count();
            _maxColumns = lines[0].Length;
            _rows = new int[_maxRows, _maxColumns];

            for (int row = 0; row < lines.Count; row++)
            {
                var line = lines[row];
                for (int column = 0; column < line.Length; column++)
                {
                    _rows[row, column] = int.Parse(line[column].ToString());
                }
            }

            _minimalTotalRisk = long.MaxValue;
        }

        public void NextMinStep(int row, int column, long totalRisk)
        {
            totalRisk += _rows[row, column];
            
            if (row == _maxColumns -1 && column == _maxRows - 1)
            {
                if (totalRisk < _minimalTotalRisk)
                {
                    _minimalTotalRisk = totalRisk;
                    Console.WriteLine(_minimalTotalRisk);
                }
                
                return;
            }

            var next = GetMinimalAdjacentPoint(row, column);
            NextMinStep(next.Item1, next.Item2, totalRisk);
            
            return;
        }

        private void FindMin()
        {
            var next = GetMinimalAdjacentPoint(0, 0);
            NextMinStep(next.Item1, next.Item2, 0);
        }

        public long FindPath()
        {
            FindMin();

            foreach (var adjacentPoint in GetAdjacentPoints(0, 0))
            {
                var visited = new HashSet<int>();
                NextStep(adjacentPoint.Item1, adjacentPoint.Item2, visited, 0);
            }

            return _minimalTotalRisk;
        }

        public void NextStep(int row, int column, HashSet<int> visited, long totalRisk)
        {
           // if (visited.Contains(GetPosition(row, column))) return;
            
            totalRisk += _rows[row, column];

            if (totalRisk > _minimalTotalRisk) return;

            if (row == _maxColumns -1 && column == _maxRows - 1)
            {
                if (totalRisk < _minimalTotalRisk)
                {
                    _minimalTotalRisk = totalRisk;
                    Console.WriteLine(_minimalTotalRisk);
                }
                
                return;
            }

            //visited.Add(GetPosition(row, column));

            foreach (var adjacentPoint in GetAdjacentPoints(row, column))
            {
                NextStep(adjacentPoint.Item1, adjacentPoint.Item2, visited, totalRisk);
            }
            
            //visited.Remove(GetPosition(row, column));

            return;
        }

        private int GetPosition(int row, int column)
        {
            return (row + 1) * _maxColumns + (column + 1);
        }

        private IEnumerable<(int, int)> GetAdjacentPoints(int row, int column)
        {
            var result = new List<(int, int)>
            {
                //(row - 1, column),
               // (row,     column - 1),
                (row,     column + 1),
                (row + 1, column)
            };

            return result.Where(point => point.Item1 >= 0
                                         && point.Item1 < _maxRows
                                         && point.Item2 >= 0
                                         && point.Item2 < _maxColumns)
                //.OrderBy(tuple => _rows[tuple.Item1, tuple.Item2])
                
                ;
        }
        
        private (int, int) GetMinimalAdjacentPoint(int row, int column)
        {
            var result = new List<(int, int)>
            {
                //(row - 1, column),
                // (row,     column - 1),
                (row,     column + 1),
                (row + 1, column)
            };

            return result.Where(point => point.Item1 >= 0
                                         && point.Item1 < _maxRows
                                         && point.Item2 >= 0
                                         && point.Item2 < _maxColumns)
                .OrderBy(tuple => _rows[tuple.Item1, tuple.Item2])
                .First()
                ;
        }

        public int Count()
        {
            var result = 0;
            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(1); j++)
                {
                    result += _rows[i, j] ;
                }
            }

            return result;
        }
        
        public bool[,] Clone(bool[,] value)
        {
            var result = new bool[value.GetLength(0), value.GetLength(1)];

            for (var i = 0; i < value.GetLength(0); i++)
            {
                for (var j = 0; j < value.GetLength(1); j++)
                {
                    result[i, j] = value[i, j];
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
                    Console.Write(_rows[i, j] );
                }

                Console.WriteLine();
            }
        }
    }
}