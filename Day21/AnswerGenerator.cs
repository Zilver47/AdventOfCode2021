using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Extensions;

namespace AdventOfCode.Day21
{
    public class AnswerGenerator : IAnswerGenerator
    {
        private readonly Dictionary<int, long> _score = new Dictionary<int, long>();

        public AnswerGenerator(string[] input)
        {
        }

        public long Part1()
        {
            var game = new Game(new Player(1, 0), new Player(10, 0));

            var dice = new Dice();
            while (true)
            {
                var roll = dice.Roll() + dice.Roll() + dice.Roll();
                
                var result = game.Turn(roll);
                if (result == 1)
                {
                    return dice.Rolls * game.PlayerTwo.Score;
                }

                if (result == 2)
                {
                    return dice.Rolls * game.PlayerOne.Score;
                }
            }

        }

        public long Part2()
        {
            var game = new Game(new Player(1, 0), new Player(10, 0));

            Play(game);

            return _score.Max(kv => kv.Value);
        }
        
        private readonly Dictionary<int, int> _diceOptions = new()
        { 
            { 3, 1 },
            { 4, 3 },
            { 5, 6 },
            { 6, 7 },
            { 7, 6 },
            { 8, 3 },
            { 9, 1 }
        };
        private void Play(Game game, long universes = 1L)
        {
            foreach (var roll in _diceOptions.Keys)
            {
                var clone = game.Clone();
                var result = clone.Turn(roll);
                if (result == 0)
                {
                    Play(clone, _diceOptions[roll] * universes);
                }
                else
                {
                    _score.AddOrIncrease(result, _diceOptions[roll] * universes);
                }
            }
        }
    }

    public class Game
    {
        public Player PlayerOne;
        public Player PlayerTwo;
        private bool _playerOneTurn;

        public Game(Player playerOne, Player playerTwo, bool playerOneTurn = true)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            _playerOneTurn = playerOneTurn;
        }

        public int Turn(int roll)
        {
            if (_playerOneTurn)
            {
                Move(PlayerOne, roll);

                //Console.WriteLine($"Player 1 rolls {roll} and moves to space {Position} for a total score of {Score}.");

                if (PlayerOne.Score >= 21)
                {
                    return 1;
                }
            }
            else
            {
                Move(PlayerTwo, roll);

                //Console.WriteLine($"Player 2 rolls {roll} and moves to space {Position} for a total score of {Score}.");

                if (PlayerTwo.Score >= 21)
                {
                    return 2;
                }
            }

            _playerOneTurn = !_playerOneTurn;

            return 0;
        }

        private static void Move(Player player, int roll)
        {
            player.Position = (player.Position + roll - 1) % 10 + 1;

            player.Score += player.Position;
        }

        public Game Clone()
        {
            return new Game(new Player(PlayerOne.Position, PlayerOne.Score), 
                new Player(PlayerTwo.Position, PlayerTwo.Score), _playerOneTurn);
        }
    }

    public record Player(int Position, int Score)
    {
        public int Score { get; set; } = Score;
        public int Position { get; set; } = Position;
    }

    public class Dice
    {
        private int _value = 0;

        public int Rolls { get; set; }

        public int Roll()
        {
            Rolls++;
            
            return ++_value;
        }
    }
}