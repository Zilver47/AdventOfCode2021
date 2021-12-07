using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day07
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
            var positions = _input[0].Split(',').Select(int.Parse).ToList();
            var min = positions.Min();
            var max = positions.Max();

            var minNumberOfMoves = long.MaxValue;
            for (var i = min; i <= max; i++)
            {
                var moves = DetermineNumberOfMoves(positions, i);
                if (moves < minNumberOfMoves)
                {
                    minNumberOfMoves = moves;
                }
            }
            
            return minNumberOfMoves;
        }

        private long DetermineNumberOfMoves(IEnumerable<int> positions, int i)
        {
            return positions.Sum(position => Math.Abs(position - i));
        }

        public long Part2()
        {
            var positions = _input[0].Split(',').Select(int.Parse).ToList();
            var min = positions.Min();
            var max = positions.Max();

            var minNumberOfMoves = long.MaxValue;
            for (var i = min; i <= max; i++)
            {
                var moves = DetermineNumberOfMoves2(positions, i);
                if (moves < minNumberOfMoves)
                {
                    minNumberOfMoves = moves;
                }
            }
            
            return minNumberOfMoves;
        }

        private long DetermineNumberOfMoves2(IReadOnlyList<int> positions, int i)
        {
            return positions.Sum(position => GetMovesForPosition(position, i));
        }

        private static int GetMovesForPosition(int position, int i)
        {
            var moves = 0;
            var delta = Math.Abs(position - i);
            for (var k = 1; k <= delta; k++)
            {
                moves += k;
            }

            return moves;
        }
    }
}