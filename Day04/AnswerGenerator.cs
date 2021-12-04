using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day04
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

            var (draw, boards) = new BoardParser(_input).Parse();

            foreach (var number in draw)
            {
                foreach (var board in boards)
                {
                    board.Play(number);
                    if (board.HasBingo()) return DetermineScore(board, number);
                }
            }

            return result;
        }

        private long DetermineScore(Board board, int number)
        {
            return board.SumUnmarked() * number;
        }

        public long Part2()
        {
            long result = -1;
            
            var (draw, boards) = new BoardParser(_input).Parse();
            var boardsList = boards.ToList();
            
            foreach (var number in draw)
            {
                var boardsToKeep = new List<Board>();

                foreach (var board in boardsList)
                {
                    board.Play(number);
                    switch (board.HasBingo())
                    {
                        case false:
                            boardsToKeep.Add(board);
                            break;
                        case true when boardsList.Count() == 1:
                            return DetermineScore(boardsList.First(), number);
                    }
                }
                
                boardsList = boardsToKeep;
            }

            return result;
        }
    }

    public class BoardParser
    {
        private readonly string[] _input;

        public BoardParser(string[] input)
        {
            _input = input;
        }

        public Tuple<IEnumerable<int>, IEnumerable<Board>> Parse()
        {
            var draw = _input[0].Split(',').Select(int.Parse);
            var boards = new List<Board>();
            var rows = new List<string>();
            for (var i = 2; i < _input.Length; i++)
            {
                var line = _input[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    boards.Add(new Board(rows));
                    rows = new List<string>();
                }
                else
                {
                    rows.Add(line);
                }
            }
            
            boards.Add(new Board(rows));

            return new Tuple<IEnumerable<int>, IEnumerable<Board>>(draw, boards);
        }
    }

    public class Board
    {
        private readonly int[,] _rows;
        private readonly bool[,] _marked;

        public Board(IReadOnlyList<string> rows)
        {
            _rows = new int[rows.Count, rows.Count];

            for (var i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var column = row.Split(' ')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(int.Parse).ToArray();
                for (var j = 0; j < column.Length; j++)
                {
                    _rows[i, j] = column[j];
                }
            }

            _marked = new bool[rows.Count, rows.Count];
        }

        public void Play(int number)
        {
            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(0); j++)
                {
                    if (_rows[i,j] == number)
                    {
                        _marked[i,j] = true;
                    }
                }
            }
        }

        public bool HasBingo()
        {
            // check rows
            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                var allMarked = true;
                for (var j = 0; j < _rows.GetLength(0); j++)
                {
                    if (!_marked[i, j]) allMarked = false;
                }
                
                if (allMarked) return true;
            }

            // check columns
            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                var allMarked = true;
                for (var j = 0; j < _rows.GetLength(0); j++)
                {
                    if (!_marked[j, i]) allMarked = false;
                }

                if (allMarked) return true;
            }

            return false;
        }

        public long SumUnmarked()
        {
            long result = 0;
            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(0); j++)
                {
                    if (!_marked[i, j])
                    {
                        result += _rows[i, j];
                    }
                }
            }

            return result;
        }
    }
}