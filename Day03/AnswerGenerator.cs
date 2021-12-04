using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Extensions;

namespace AdventOfCode.Day03
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
            var gamma = string.Empty;
            var epsilon = string.Empty;

            var lines = _input.ToArray();
            for (var j = 0; j < lines[0].Length; j++)
            {
                var ones = 0;
                var zeros = 0;

                foreach (var line in lines)
                {
                    if (line[j] == '1') ones++;
                    else zeros++;
                }

                gamma += ones > zeros ? "1" : "0";
                epsilon += ones > zeros ? "0" : "1";
            }
            
            return Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2);
        }

        public static string ToBitString(BitArray bits)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < bits.Count; i++)
            {
                var c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }

        public long Part2()
        {
            var lines = _input.ToList();
            
            return GetOnes(lines) * GetZeros(lines);
        }

        private long GetOnes(List<string> numbers)
        {
            var numbersToKeep = numbers.Clone();
            for (var j = 0; j < numbers[0].Length; j++)
            {
                var compare = GetOnesString(numbers[0].Length, numbersToKeep);
                foreach (var number in numbers)
                {
                    if (compare[j] != number[j]) numbersToKeep.Remove(number);

                    if (numbersToKeep.Count == 1)
                    {
                        return Convert.ToInt32(numbersToKeep.First(), 2);
                    }
                }
            }

            return 0;
        }

        private long GetZeros(List<string> numbers)
        {
            var numbersToKeep = numbers.Clone();
            for (var j = 0; j < numbers[0].Length; j++)
            {
                var compare = GetZerosString(numbers[0].Length, numbersToKeep);
                for (var i = 0; i < numbers.Count; i++)
                {
                    if (compare[j] != numbers[i][j]) numbersToKeep.Remove(numbers[i]);

                    if (numbersToKeep.Count == 1)
                    {
                        return Convert.ToInt32(numbersToKeep.First(), 2);
                    }
                }

            }

            return 0;
        }

        private string GetOnesString(int length, IList<string> lines)
        {
            var result = string.Empty;
            for (var j = 0; j < length; j++)
            {
                var ones = 0;
                var zeros = 0;

                foreach (var line in lines)
                {
                    if (line[j] == '1') ones++;
                    else zeros++;
                }

                result += ones > zeros ? "1" : "0";
            }

            return result;
        }

        private string GetZerosString(int length, IList<string> lines)
        {
            var result = string.Empty;
            for (var j = 0; j < length; j++)
            {
                var ones = 0;
                var zeros = 0;

                foreach (var line in lines)
                {
                    if (line[j] == '1') ones++;
                    else zeros++;
                }

                result += ones > zeros ? "0" : "1";
            }

            return result;
        }
    }
}