using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day08
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
            var digits = _input.Select(line => 
                line.Split('|')[1].Trim().Split(' ')).ToList();

            return digits.Sum(digit => digit.LongCount(value => value.Length is 2 or 7 or 3 or 4));
        }

        public long Part2()
        {
            var signalLines = _input.Select(line => 
                line.Split('|')[0].Trim().Split(' ')).ToList();
            var digitLines = _input.Select(line => 
                line.Split('|')[1].Trim().Split(' ')).ToList();
         
            var allDigitsByNumeric = new Dictionary<int, string>();   
            long result = 0;
            for (var i = 0; i < _input.Length; i++)
            {
                var signals = signalLines[i];
                allDigitsByNumeric[1] = signals.Single(n => n.Length == 2);
                allDigitsByNumeric[7] = signals.Single(n => n.Length == 3);
                allDigitsByNumeric[4] = signals.Single(n => n.Length == 4);
                allDigitsByNumeric[8] = signals.Single(n => n.Length == 7);

                var bd = string.Join(string.Empty, allDigitsByNumeric[4].Where(c => !allDigitsByNumeric[1].Contains(c)));

                var signalsWith5Segments = signals.Where(n => n.Length == 5).ToList();
                allDigitsByNumeric[5] = GetContainsAll(bd, signalsWith5Segments);
                allDigitsByNumeric[3] = GetContainsAll(allDigitsByNumeric[1], signalsWith5Segments);
                allDigitsByNumeric[2] = signalsWith5Segments.Single(n => n != allDigitsByNumeric[3] && n != allDigitsByNumeric[5]);

                var signalsWith6Segments = signals.Where(n => n.Length == 6).ToList();
                allDigitsByNumeric[9] = GetContainsAll(allDigitsByNumeric[4], signalsWith6Segments);
                allDigitsByNumeric[6] = GetContainsAll(allDigitsByNumeric[5], signalsWith6Segments.Where(n => n != allDigitsByNumeric[9]));
                allDigitsByNumeric[0] = signalsWith6Segments.Single(n => n != allDigitsByNumeric[9] && n != allDigitsByNumeric[6]);

                var allDigitsByEncodedKey = allDigitsByNumeric.ToDictionary(x => x.Value, x => x.Key);
                
                var outputValue = string.Empty;
                var digits = digitLines[i];
                foreach (var encodedDigit in digits)
                {
                    var key = allDigitsByEncodedKey.Keys
                        .Single(k => k.Length == encodedDigit.Length && k.All(c => encodedDigit.Contains(c)));
                    outputValue += allDigitsByEncodedKey[key];
                    
                    //Console.Write($"{allDigitsByEncodedKey[key]}");
                }

                //Console.WriteLine();
                
                result += int.Parse(outputValue);
            }
            
            return result;
        }

        private string GetContainsAll(string four, IEnumerable<string> values)
        {
            return values.Single(value => four.All(value.Contains));
        }
    }


}