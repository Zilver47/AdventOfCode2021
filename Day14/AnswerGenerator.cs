using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Extensions;

namespace AdventOfCode.Day14
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
            var (template, rules) = Parser.Parse(_input);
            
            var polymer = string.Empty;
            for (var i = 0; i < 10; i++)
            {
                polymer = template[0].ToString();
                for (var characterIndex = 0; characterIndex < template.Length - 1; characterIndex++)
                {
                    var pair = string.Concat(template[characterIndex], template[characterIndex + 1]);
                    polymer += rules[pair] + template[characterIndex + 1];
                }

                template = polymer;
                
                //Console.WriteLine($"{i + 1}: {polymer}");
            }

            var characterOccurences = new Dictionary<string, long>();
            foreach (var character in polymer)
            {
                characterOccurences.AddOrIncrease(character.ToString());
            }
            
            long max = characterOccurences.Max(p => p.Value);
            long min = characterOccurences.Min(p => p.Value);

            return max - min;
        }

        public long Part2()
        {
            var (template, rules) = Parser.Parse(_input);

            var pairs = new Dictionary<string, long>();
            for (var characterIndex = 0; characterIndex < template.Length - 1; characterIndex++)
            {
                var pair = string.Concat(template[characterIndex], template[characterIndex + 1]);
                pairs.AddOrIncrease(pair);
            }
            
            for (var i = 0; i < 40; i++)
            {
                var newPairs = new Dictionary<string, long>();
                foreach (var key in pairs.Keys)
                {
                    var pair = key;
                    newPairs.AddOrIncrease(pair[0] + rules[key], pairs[key]);
                    newPairs.AddOrIncrease(rules[key] + pair[1], pairs[key]);
                }

                pairs = newPairs;
            }

            var characterOccurences = new Dictionary<string, long>();
            foreach (var key in pairs.Keys)
            {
                characterOccurences.AddOrIncrease(key[0].ToString(), pairs[key]);
            }

            characterOccurences.AddOrIncrease(template[^1].ToString()); // last
            
            long max = characterOccurences.Max(p => p.Value);
            long min = characterOccurences.Min(p => p.Value);

            return max - min;
        }
    }

    public class Parser
    {
        public static (string, Dictionary<string, string>) Parse(string[] input)
        {
            var template = input[0];

            var pairs = new Dictionary<string, string>();
            for (var i = 2; i < input.Length; i++)
            {
                var parts = input[i].Split(" -> ");
                pairs.Add(parts[0], parts[1]);
            }

            return (template, pairs);
        }
    }
}