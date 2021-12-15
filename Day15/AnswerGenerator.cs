using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day15
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
            var board = new Board(_input.ToList());

            //board.Print();
            var result = board.FindPath();

            return result;
        }

        public long Part2()
        {
            var board = new Board(_input.ToList());

            board.Enlarge();
            //board.Print();
            var result = board.FindPath();

            return result;
        }
    }
    
    public class Board
    {
        private int[,] _rows;
        private int _maxColumns;
        private int _maxRows;
        private bool[,] _visited;

        public Board(List<string> lines)
        {
            _maxRows = lines.Count();
            _maxColumns = lines[0].Length;
            _rows = new int[_maxRows, _maxColumns];

            for (var row = 0; row < lines.Count; row++)
            {
                var line = lines[row];
                for (var column = 0; column < line.Length; column++)
                {
                    _rows[row, column] = int.Parse(line[column].ToString());
                }
            }
            
            _visited = new bool[_maxRows, _maxColumns];
        }

        public long FindPath()
        {
            _rows[0, 0] = 0;

            var start = new Tile();
            start.Row = 0;
            start.Column = 0;

            var finish = new Tile();
            finish.Row = _maxRows - 1;
            finish.Column = _maxColumns - 1;
	
            var activeTiles = new List<Tile>();
            activeTiles.Add(start);

            while (activeTiles.Any())
            {
                var lowestTotalRisk = activeTiles.Min(x => x.PreviousRisk);
                var checkTiles = activeTiles.Where(x => x.PreviousRisk == lowestTotalRisk).ToList();

                foreach (var checkTile in checkTiles)
                {
                    if (checkTile.Row == finish.Row && checkTile.Column == finish.Column)
                    {
                        //We can actually loop through the parents of each tile to find our exact path which we will show shortly. 

                        //var tile = checkTile;
                        ////Console.WriteLine("Retracing steps backwards...");
                        //while (true)
                        //{
                        //    Console.WriteLine(
                        //        $"{tile.Row} : {tile.Column} (Value: {tile.Risk} PreviousRisk: {tile.PreviousRisk} Risk: {tile.Risk}");
                        //    tile = tile.Parent;
                        //    if (tile == null)
                        //    {
                        //        return checkTile.PreviousRisk;
                        //    }
                        //}

                        return checkTile.PreviousRisk;
                    }

                    _visited[checkTile.Row, checkTile.Column] = true;
                    activeTiles.Remove(checkTile);

                    var walkableTiles = GetWalkableTiles(checkTile);
                    foreach (var walkableTile in walkableTiles)
                    {
                        //We have already visited this tile so we don't need to do so again!
                        if (_visited[walkableTile.Row, walkableTile.Column])
                        {
                            continue;
                        }

                        //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                        if (activeTiles.Any(x => x.Row == walkableTile.Row && x.Column == walkableTile.Column))
                        {
                            var existingTile = activeTiles.Single(x =>
                                x.Row == walkableTile.Row && x.Column == walkableTile.Column);
                            if (existingTile.PreviousRisk > walkableTile.PreviousRisk)
                            {
                                activeTiles.Remove(existingTile);
                                activeTiles.Add(walkableTile);
                            }
                        }
                        else
                        {
                            //We've never seen this tile before so add it to the list. 
                            activeTiles.Add(walkableTile);
                        }
                    }
                }
            }

            return -1;
        }

        private List<Tile> GetWalkableTiles(Tile currentTile)
        {
            var possibleTiles = new List<Tile>()
            {
                new() { Row = currentTile.Row, Column = currentTile.Column - 1, PreviousRisk = currentTile.PreviousRisk },
                new() { Row = currentTile.Row, Column = currentTile.Column + 1, PreviousRisk = currentTile.PreviousRisk},
                new() { Row = currentTile.Row - 1, Column = currentTile.Column, PreviousRisk = currentTile.PreviousRisk },
                new() { Row = currentTile.Row + 1, Column = currentTile.Column, PreviousRisk = currentTile.PreviousRisk },
            };
            
            possibleTiles = possibleTiles
                .Where(tile => tile.Row >= 0 && tile.Row < _maxRows)
                .Where(tile => tile.Column >= 0 && tile.Column < _maxColumns).ToList();
            
            foreach (var tile in possibleTiles)
            {
                tile.PreviousRisk += _rows[tile.Row, tile.Column];
            }

            return possibleTiles;
        }

        public void Enlarge()
        {
            var newRows = new int[_maxRows * 5, _maxColumns * 5];

            for (var row = 0; row < _maxRows; row++)
            {
                for (var co = 0; co < _maxColumns; co++)
                {
                    newRows[row, co] = _rows[row, co];
                }
            }
            
            for (var row = 0; row < _maxRows; row++)
            {
                for (var column = 0; column < _maxColumns; column++)
                {
                    var value = newRows[row, column];

                    for (var times = 1; times < 5; times++)
                    {
                        value++;
                        value = value > 9 ? 1 : value;

                        var newRow = row + times * _maxRows;
                        newRows[newRow, column] = value;
                    }
                }
            }
            
            var newMaxRows = _maxRows * 5;
            for (var row = 0; row < newMaxRows; row++)
            {
                for (var column = 0; column < _maxColumns; column++)
                {
                    var value = newRows[row, column];

                    for (var times = 1; times < 5; times++)
                    {
                        value++;
                        value = value > 9 ? 1 : value;

                        var newColumn = column + times * _maxColumns;
                        newRows[row, newColumn] = value;
                    }
                }
            }
            
            _maxRows *= 5;
            _maxColumns *= 5;
            _rows = newRows;
            _totalRisks = new int[_maxRows, _maxColumns];
            _visited = new bool[_maxRows, _maxColumns];
        }

        public void Print()
        {
            Console.WriteLine();

            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(1); j++)
                {
                    Console.Write(_rows[i, j]);
                }

                Console.WriteLine();
            }
        }
    }
    record Tile
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int PreviousRisk { get; set; }
        
        public override string ToString()
        {
            return $"{Row}-{Column} (Risk: {PreviousRisk};";
        }
    }
}