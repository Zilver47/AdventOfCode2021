using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AdventOfCode.Day05
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

            var lines = Parser.Parse(_input).ToList();
            var maxX = Math.Max(lines.Max(l => l.Start.Item1), lines.Max(l => l.End.Item1));
            var maxY = Math.Max(lines.Max(l => l.Start.Item2), lines.Max(l => l.End.Item2));

            var board = new Board(maxX, maxY);
            foreach (var line in lines.Where(l => l.HorizontalOrVertical))
            {
                board.AddLine(line);

                //board.Print();
            }

            result = board.Count();

            return result;
        }

        public long Part2()
        {
            long result = -1;
            
            var lines = Parser.Parse(_input).ToList();
            var maxX = Math.Max(lines.Max(l => l.Start.Item1), lines.Max(l => l.End.Item1));
            var maxY = Math.Max(lines.Max(l => l.Start.Item2), lines.Max(l => l.End.Item2));

            var board = new Board(maxX, maxY);
            foreach (var line in lines)
            {
                board.AddLine(line);

                //board.Print();
            }

            result = board.Count();

            return result;
        }
    }

    public class Parser
    {
        public static IEnumerable<Line> Parse(string[] input)
        {
            return input.Select(line => new Line(line));
        }
    }

    public class Line
    {
        public (int, int) Start { get; set; }
        public (int, int) End { get; set; }

        public bool HorizontalOrVertical => 
            Start.Item1 == End.Item1 || Start.Item2 == End.Item2;

        public Line(string line)
        {
            var startParts = line[..line.IndexOf(' ')].Split(',').Select(int.Parse);
            Start = new ValueTuple<int, int>(startParts.First(), startParts.Last());
            
            var endParts = line[(line.IndexOf('>') + 2)..].Split(',').Select(int.Parse);
            End = new ValueTuple<int, int>(endParts.First(), endParts.Last());
        }
        
        public IEnumerable<(int, int)> GetPoints()
        {
            var diffX = Start.Item1 == End.Item1 ? 0 : Start.Item1 < End.Item1 ? 1 : -1;
            var diffY = Start.Item2 == End.Item2 ? 0 : Start.Item2 < End.Item2 ? 1 : -1;

            var x = Start.Item1;
            var y = Start.Item2;

            while (x != End.Item1 || y != End.Item2)
            {
                yield return (x, y);

                x += diffX;
                y += diffY;
            }
            
            yield return End;
        }
    }

    public class Board
    {
        private readonly int[,] _field;

        public Board(int height, int width)
        {
            _field = new int[height + 1, width + 1];
        }

        public void AddLine(Line line)
        {
            foreach (var point in line.GetPoints())
            {
                _field[point.Item1, point.Item2]++;
            }
        }

        public long Count()
        {
            var result = 0;
            for (var i = 0; i < _field.GetLength(0); i++)
            {
                for (var j = 0; j < _field.GetLength(1); j++)
                {
                    if (_field[i, j] > 1) result++;
                }
            }

            return result;
        }

        public void Print()
        {
            Console.WriteLine();

            for (var i = 0; i < _field.GetLength(0); i++)
            {
                for (var j = 0; j < _field.GetLength(1); j++)
                {
                    Console.Write(_field[j, i] == 0 ? "." : _field[j, i]);
                }

                Console.WriteLine();
            }
        }
    }
}