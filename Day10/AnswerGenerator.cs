using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day10;

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

        var points = new Dictionary<char, int>
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 }
        };
        
        var syntaxErrors = new Dictionary<char, int>
        {
            { ')', 0 },
            { ']', 0 },
            { '}', 0 },
            { '>', 0 }
        };

        var startingCharacters = new Stack<char>();
        foreach (var line in _input)
        {
            foreach (var character in line)
            {
                if (IsStartingCharacter(character))
                {
                    startingCharacters.Push(character);
                }
                else if (IsMatch(startingCharacters.Peek(), character))
                {
                    startingCharacters.Pop();
                }
                else
                {
                    syntaxErrors[character]++;
                    break;
                }
            }
        }

        foreach (var (character, value) in syntaxErrors)
        {
            result += points[character] * value;
        }

        return result;
    }

    private bool IsStartingCharacter(char value)
    {
        return new[] { '(', '[', '{', '<' }.Contains(value);
    }

    private bool IsMatch(char last, char next)
    {
        switch (last)
        {
            case '(' when next == ')':
            case '[' when next == ']':
            case '{' when next == '}':
            case '<' when next == '>':
                return true;
            default:
                return false;
        }
    }

    public long Part2()
    {
        var points = new Dictionary<char, int>
        {
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 }
        };
        
        var incompleteLines = new List<Stack<char>>();
        foreach (var line in _input)
        {
            var allCharacters = new Stack<char>();
            var startingCharacters = new Stack<char>();
            var isCorrupt = false;
            foreach (var character in line)
            {
                allCharacters.Push(character);

                if (IsStartingCharacter(character))
                {
                    startingCharacters.Push(character);
                }
                else if (IsMatch(startingCharacters.Peek(), character))
                {
                    startingCharacters.Pop();
                }
                else
                {
                    isCorrupt = true;
                }
            }

            if (!isCorrupt) incompleteLines.Add(allCharacters);
        }

        var scores = new List<long>();
        foreach (var incompleteLine in incompleteLines)
        {
            var endingCharacters = new Stack<char>();

            long result = 0;
            while (incompleteLine.Count > 0)
            {
                var character = incompleteLine.Pop();
                if (IsStartingCharacter(character))
                {
                    if (endingCharacters.Count > 0 && IsMatch(character, endingCharacters.Peek()))
                    {
                        endingCharacters.Pop();
                    }
                    else
                    {
                        result = result * 5 + points[GetMatch(character)];
                    }
                }
                else
                {
                    endingCharacters.Push(character);
                }
            }

            scores.Add(result);
        }
        
        scores.Sort();
        var index = Convert.ToInt32(Math.Ceiling((decimal)scores.Count / 2));
        
        return scores[index];
    }

    private char GetMatch(char last)
    {
        return last switch
        {
            '(' => ')',
            '[' => ']',
            '{' => '}',
            '<' => '>',
            _ => ' '
        };
    }
}
