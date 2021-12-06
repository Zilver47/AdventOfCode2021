using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Extensions;

namespace AdventOfCode.Day06
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
            var intervals = _input[0].Split(',')
                .Select(int.Parse).ToList();

            for (var day = 1; day <= 80; day++)
            {
                for (var i = 0; i < intervals.Count; i++)
                {
                    var interval = intervals[i];
                    if (interval == 0)
                    {
                        intervals.Add(9);
                        intervals[i] = 6;
                    }
                    else
                    {
                        intervals[i] = interval - 1;
                    }
                }

                //Print(day, intervals);
            }

            return intervals.Count;
        }

        private void Print(int day, IEnumerable<int> intervals)
        {
            Console.WriteLine();
            Console.Write($"After {day} days: ");
            foreach (var interval in intervals)
            {
                Console.Write($"{interval},");
            }
        }

        public long Part2()
        {
            var intervals = _input[0].Split(',')
                .Select(int.Parse).ToArray();

            var fish = new Dictionary<int, long>
            {
                { 0, 0 },
                { 1, 0 },
                { 2, 0 },
                { 3, 0 },
                { 4, 0 },
                { 5, 0 },
                { 6, 0 },
                { 7, 0 },
                { 8, 0 },
                { 9, 0 }
            };

            foreach (var interval in intervals)
            {
                fish.AddOrIncrease(interval);
            }

            for (var day = 1; day <= 256; day++)
            {
                var newFish = new Dictionary<int, long>
                {
                    { 0, 0 },
                    { 1, 0 },
                    { 2, 0 },
                    { 3, 0 },
                    { 4, 0 },
                    { 5, 0 },
                    { 6, 0 },
                    { 7, 0 },
                    { 8, 0 },
                    { 9, 0 }
                };
                foreach (var (key, value) in fish)
                {
                    if (key == 0)
                    {
                        newFish[6] = value;
                        newFish[8] = value;
                    }
                    else
                    {
                        newFish[key - 1] += value;
                    }
                }

                fish = newFish;
            }

            return fish.Values.Sum();
        }
    }
}