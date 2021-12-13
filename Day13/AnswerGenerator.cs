using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day13
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
            var input = new Parser(_input).Parse();
            var board = input.Item1;
            var folds = input.Item2;

            //board.Print();

            board.Fold(folds.First().Ax, folds.First().Value);

            //board.Print();

            var result = board.Count();
            return result;
        }

        public long Part2()
        {
            long result = -1;
            
            var input = new Parser(_input).Parse();
            var board = input.Item1;
            var folds = input.Item2;
            
            foreach (var fold in folds)
            {
                board.Fold(fold.Ax, fold.Value);
            }

            board.Print();

            return result;
        }
    }
    
    public class Parser
    {
        private readonly string[] _input;

        public Parser(string[] input)
        {
            _input = input;
        }

        public Tuple<Board, IEnumerable<Fold>> Parse()
        {
            var folds = new List<Fold>();
            var dotRows = new List<(int, int)>();
            var isFolds = false;
            for (var i = 0; i < _input.Length; i++)
            {
                var line = _input[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    isFolds = true;
                }
                else if (isFolds)
                {
                    folds.Add(new Fold(line));
                }
                else
                {
                    var parts = line.Split(',');
                    dotRows.Add((int.Parse(parts[0]), int.Parse(parts[1])));
                }
            }
            
            return new Tuple<Board, IEnumerable<Fold>>(new Board(dotRows), folds);
        }
    }

    public class Fold
    {
        public string Ax { get; set; }
        public int Value { get; set; }

        public Fold(string line)
        {
            var parts = line.Replace("fold along ", string.Empty).Split('=');
            Ax = parts[0];
            Value = int.Parse(parts[1]);
        }
    }

    public class Board
    {
        private bool[,] _rows;
        private int _maxColumns;
        private int _maxRows;

        public Board(IEnumerable<(int, int)> dots)
        {
            _maxRows = dots.Max(item => item.Item2) + 1;
            _maxColumns = dots.Max(item => item.Item1) + 1;
            _rows = new bool[_maxRows, _maxColumns];

            foreach (var dot in dots)
            {
                _rows[dot.Item2, dot.Item1] = true;
            }
        }

        public void Fold(string ax, int value)
        {
            if (ax == "y")
            {
                var delta = value % 2 == 0 ? 1 : 2;
                for (var row = value + delta; row < _maxRows; row++)
                {
                    for (var column = 0; column < _maxColumns; column++)
                    {
                        if (_rows[row, column])
                        {
                            _rows[_maxRows - row - 1, column] = true;
                        }
                    }
                }
                
                _maxRows = value;
                var newRows = new bool[_maxRows, _maxColumns];
                for (var row = 0; row < _maxRows; row++)
                {
                    for (var column = 0; column < _maxColumns; column++)
                    {
                        newRows[row, column] = _rows[row, column];
                    }
                }

                _rows = newRows;
            }
            else
            {
                for (var row = 0; row < _maxRows; row++)
                {
                    for (var column = value + 1; column < _maxColumns; column++)
                    {
                        if (_rows[row, column])
                        {
                            _rows[row, _maxColumns - column - 1] = true;
                        }
                    }
                }

                _maxColumns = value;
                var newRows = new bool[_maxRows, _maxColumns];
                for (var row = 0; row < _maxRows; row++)
                {
                    for (var column = 0; column < _maxColumns; column++)
                    {
                        newRows[row, column] = _rows[row, column];
                    }
                }

                _rows = newRows;
            }
        }

        public int Count()
        {
            var result = 0;
            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(1); j++)
                {
                    result += _rows[i, j] ? 1 : 0;
                }
            }

            return result;
        }

        public void Print()
        {
            Console.WriteLine();

            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(1); j++)
                {
                    Console.Write(_rows[i, j] ? "#" : ".");
                }

                Console.WriteLine();
            }
        }
    }
}