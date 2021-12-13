using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Extensions;

namespace AdventOfCode.Day12
{
    public class AnswerGenerator : IAnswerGenerator
    {
        private readonly string[] _input;
        private long _numberOfDistinctPaths;
        private readonly Dictionary<string, int> _distinctPaths;

        public AnswerGenerator(string[] input)
        {
            _input = input;
            _numberOfDistinctPaths = 0;
            _distinctPaths = new Dictionary<string, int>();
        }

        public long Part1()
        {
            var caves = Parse();

            foreach (var connectedCave in caves["start"])
            {
                Traverse(connectedCave, caves[connectedCave], caves, new List<string>());
            }

            return _numberOfDistinctPaths;
        }

        private void Traverse(string cave, List<string> connectedCaves, Dictionary<string, List<string>> caves, IList<string> distinctPath)
        {
            if (IsSmallCave(cave) && distinctPath.Contains(cave) && cave != "end") return;
            
            distinctPath.Add(cave);

            if (cave == "end")
            {
                _numberOfDistinctPaths++;

                //Console.WriteLine(string.Join(',', distinctPath));

                return;
            }
            
            foreach (var connectedCave in connectedCaves.Where(cc => cc != "start"))
            {
                Traverse(connectedCave, caves[connectedCave], caves, distinctPath.Clone());
            }
        }

        private static bool IsSmallCave(string cave)
        {
            return IsLower(cave);
        }

        public static bool IsLower(string value)
        {
            return value.All(character => !char.IsUpper(character));
        }

        public long Part2()
        {
            var caves = Parse();

            foreach (var connectedCave in caves["start"])
            {
                Traverse2(connectedCave, caves[connectedCave], caves, new List<string>(), string.Empty);
            }

            return _distinctPaths.Count;
        }

        private void Traverse2(string cave, List<string> connectedCaves, Dictionary<string, List<string>> caves, IList<string> distinctPath, string twice)
        {
            if (IsSmallCave(cave) && distinctPath.Contains(cave) && cave != "end")
            {
                if (string.IsNullOrEmpty(twice))
                {
                    twice = cave;
                }
                else
                {
                    return;
                }
            }
            
            distinctPath.Add(cave);
            
            if (cave == "end")
            {
                var pathAsString = string.Join(',', distinctPath);
                if (!_distinctPaths.ContainsKey(pathAsString)) _distinctPaths.Add(pathAsString, 0);
                
                //Console.WriteLine(pathAsString);

                return;
            }
            
            foreach (var connectedCave in connectedCaves.Where(cc => cc != "start"))
            {
                Traverse2(connectedCave, caves[connectedCave], caves, distinctPath.Clone(), twice);
            }
        }

        private Dictionary<string, List<string>> Parse()
        {
            var caves = new Dictionary<string, List<string>>();
            var parts = _input.Select(l => new Path(l)).ToList();
            foreach (var path in parts)
            {
                if (caves.ContainsKey(path.From))
                {
                    if (!caves[path.From].Contains(path.To))
                    {
                        caves[path.From].Add(path.To);
                    }
                }
                else
                {
                    caves.Add(path.From, new List<string> { path.To });
                }

                if (caves.ContainsKey(path.To))
                {
                    if (!caves[path.To].Contains(path.From))
                    {
                        caves[path.To].Add(path.From);
                    }
                }
                else
                {
                    caves.Add(path.To, new List<string> { path.From });
                }
            }

            return caves;
        }
    }

    public class Path
    {
        public string From { get; set; }
        public string To { get; set; }

        public Path(string line)
        {
            var parts = line.Split('-');
            From = parts[0];
            To = parts[1];
        }
    }

    public class Cave
    {
        public string Name { get; set; }

        public List<string> ConnectedCaves { get; set; }

        public Cave(string name, List<string> connectedCaves)
        {
            Name = name;
            ConnectedCaves = connectedCaves;
        }
    }
}