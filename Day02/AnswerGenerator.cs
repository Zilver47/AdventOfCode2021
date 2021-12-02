using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day02
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
            long depth = 0;
            long horizontal = 0;

            var instructions = ParseInput();

            foreach (var instruction in instructions)
            {
                switch (instruction.Direction)
                {
                    case "down":
                        depth += instruction.Ammount;
                        break;
                    case "up":
                        depth -= instruction.Ammount;
                        break;
                    case "forward":
                        horizontal += instruction.Ammount;
                        break;
                }
            }

            return depth * horizontal;
        }

        private IEnumerable<Instruction> ParseInput()
        {
            return _input.Select(line => 
                new Instruction
                {
                    Direction = line.Substring(0, line.IndexOf(' ')),
                    Ammount = int.Parse(line.Substring(line.IndexOf(' ') + 1))
                });
        }

        public long Part2()
        {
            long depth = 0;
            long horizontal = 0;
            long aim = 0;

            var instructions = ParseInput();

            foreach (var instruction in instructions)
            {
                switch (instruction.Direction)
                {
                    case "down":
                        aim += instruction.Ammount;
                        break;
                    case "up":
                        aim -= instruction.Ammount;
                        break;
                    case "forward":
                        horizontal += instruction.Ammount;
                        depth += instruction.Ammount * aim;
                        break;

                }
            }

            return depth * horizontal;
        }
    }
}