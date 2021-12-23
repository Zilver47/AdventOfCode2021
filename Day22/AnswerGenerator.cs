using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AdventOfCode.Day22
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

            var steps = Parser.Parse(_input).ToList();

            var board = new Board();
            var i = 1;
            //foreach (var step in steps)
            //{
            //    board.Apply(step);
                
            //    Console.WriteLine($"Step {i}");
            //    i++;
            //}

            result = board.Count();
            
            return result;
        }

        public long Part2()
        {
            long result = -1;
            
            var steps = Parser.Parse(_input).ToList();
            
            var board = new Board();
            var i = 1;
            foreach (var step in steps)
            {
                board.Apply(step);
                
                //Console.WriteLine($"Step {i}");
                i++;
            }

            result = board.Count();

            return result;
        }
    }

    public class Board
    {
        private List<Grid> _grids;

        public Board()
        {
            _grids = new List<Grid>();
        }
        
        public void Apply(RebootSteps step)
        {
            foreach (var grid in _grids)
            {
                grid.Apply(step);
            }

            if (step.On) _grids.Add(new Grid(step));
        }

        public long Count()
        {
            long result = 0;

            foreach (var grid in _grids)
            {
                result += grid.Count();
            }

            return result;
        }
    }

    public class Parser
    {
        public static IEnumerable<RebootSteps> Parse(string[] _input)
        {
            foreach (var line in _input)
            {
                yield return new RebootSteps(line);
            }
        }
    }

    public class Grid
    {
        private bool[,,] _grid;
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int RangeX => MaxX - MinX + 1;
        public int MinY { get; set; }
        public int MaxY { get; set; }
        public int RangeY => MaxY-MinY + 1;
        public int MinZ { get; set; }
        public int MaxZ { get; set; }
        public int RangeZ => MaxZ - MinZ + 1;

        public Grid(RebootSteps step)
        {
            MinX = step.MinX;
            MaxX = step.MaxX;
            MinY = step.MinY;
            MaxY = step.MaxY;
            MinZ = step.MinZ;
            MaxZ = step.MaxZ;
            
            _grid = new bool[RangeX, RangeY, RangeZ];
        }

        public void Apply(RebootSteps step)
        {
            for (int x = step.MinX; x <= step.MaxX; x++)
            {
                if (x < MinX || x > MaxX) continue;

                for (int y = step.MinY; y <= step.MaxY; y++)
                {
                    if (y < MinY || y > MaxY) continue;

                    for (int z = step.MinZ; z <= step.MaxZ; z++)
                    {
                        if (z < MinZ || z > MaxZ) continue;
                        
                        _grid[x - MinX, y - MinY, z - MinZ] = true;
                    }
                }
            }
        }

        public long Count()
        {
            var result = 0;
            for (int x = 0; x < _grid.GetLength(0); x++)
            {
                for (int y = 0; y <_grid.GetLength(1); y++)
                {
                    for (int z = 0; z < _grid.GetLength(2); z++)
                    {
                        result += _grid[x, y, z] ? 0 : 1;
                    }
                }
            }

            return result;
        }
    }

    public class RebootSteps
    {
        public bool On { get; set; }
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }
        public int MinZ { get; set; }
        public int MaxZ { get; set; }

        public RebootSteps(string line)
        {
            On = line.Split(' ')[0] == "on";

            var parts = line.Split(' ')[1].Split(',');
            MinX = int.Parse(parts[0].Substring(2, parts[0].IndexOf(".") - 2));
            MaxX = int.Parse(parts[0].Substring(parts[0].IndexOf("..") + 2));
            
            MinY = int.Parse(parts[1].Substring(2, parts[1].IndexOf(".") - 2));
            MaxY = int.Parse(parts[1].Substring(parts[1].IndexOf("..") + 2));
            
            MinZ = int.Parse(parts[2].Substring(2, parts[2].IndexOf(".") - 2));
            MaxZ = int.Parse(parts[2].Substring(parts[2].IndexOf("..") + 2));
        }
    }
}