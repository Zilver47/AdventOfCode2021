using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day01
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

            var measurements = _input.Select(int.Parse).ToList();

            for (int i = 1; i < measurements.Count; i++)
            {
                if (measurements[i - 1] < measurements[i]) result++;
            }

            return result;
        }

        public long Part2()
        {
            long result = 0;

            var measurements = _input.Select(int.Parse).ToList();

            for (int i = 3; i < measurements.Count; i++)
            {
                var sumA = measurements[i - 3] + measurements[i - 2] + measurements[i - 1];
                var sumB = measurements[i - 2] + measurements[i - 1] + measurements[i];

                if (sumA < sumB) result++;
            }

            return result;
        }
    }
}