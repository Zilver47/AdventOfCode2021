using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day09
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

            var lowPoints = board.GetLowPointValues();
            var result = lowPoints.Select(p => p + 1).Sum();

            return result;
        }

        public long Part2()
        {
            var board = new Board(_input.ToList());

            var basins = board.GetBasins();

            var result = basins.OrderByDescending(x => x)
                .Take(3)
                .Aggregate((a, b) => a * b);

            return result;
        }
    }

    public class Board
    {
        private readonly int[,] _rows;
        private readonly bool[,] _visited;
        private readonly int _numberOfRows;
        private readonly int _numberOfColumns;

        public Board(IReadOnlyList<string> rows)
        {
            _numberOfRows = rows.Count;
            _numberOfColumns = rows[0].Length;
            _rows = new int[_numberOfRows, _numberOfColumns];
            _visited = new bool[_numberOfRows, _numberOfColumns];

            for (var i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                for (var j = 0; j < row.Length; j++)
                {
                    _rows[i, j] = int.Parse(row[j].ToString());
                }
            }
        }

        public IEnumerable<int> GetLowPointValues()
        {
            return GetLowPoints()
                .Select(point => _rows[point.Item1, point.Item2]);
        }

        public List<(int, int)> GetLowPoints()
        {
            var result = new List<(int, int)>();
            for (var i = 0; i < _numberOfRows; i++)
            {
                for (var j = 0; j < _numberOfColumns; j++)
                {
                    if (GetAdjacentValues(i,j)
                        .All(point => point > _rows[i,j]))
                    {
                        result.Add((i,j));
                    }
                }
            }

            return result;
        }

        public IEnumerable<long> GetBasins()
        {
            var result = new List<long>();
            var lowPoints = GetLowPoints();
            foreach (var (row, column) in lowPoints)
            {
                var basin = GetBasin(row, column);
                result.Add(basin.Count);
            }

            return result;
        }

        public List<int> GetBasin(int row, int column)
        {
            var result = new List<int>();
            if (_visited[row, column]) return result;
            _visited[row, column] = true;

            var value = _rows[row, column];
            if (value == 9) return result;

            result.Add(value);

            var adjacentPoints = GetAdjacentPoints(row, column);
            foreach (var (adjacentRow, adjacentColumn) in adjacentPoints)
            {
                result.AddRange(GetBasin(adjacentRow, adjacentColumn));
            }

            return result;
        }

        private IEnumerable<int> GetAdjacentValues(int row, int column)
        {            
            return GetAdjacentPoints(row, column)
                .Select(point => _rows[point.Item1, point.Item2]);
        }

        private IEnumerable<(int, int)> GetAdjacentPoints(int row, int column)
        {
            var result = new List<(int, int)>
            {
                (row - 1, column),
                (row,     column - 1),
                (row,     column + 1),
                (row + 1, column)
            };

            return result.Where(point => point.Item1 >= 0
                                         && point.Item1 < _numberOfRows
                                         && point.Item2 >= 0
                                         && point.Item2 < _numberOfColumns);
        }
    }
}