using BlazingDartsForFriends.Models;
using Classes;

namespace BlazingDartsForFriends.Services
{
    public class GameService
    {
        private IGameEngine _currentEngine;
        private GameState? _currentGame;

        public GameState? CurrentGame => _currentGame;

        public event Action? OnGameStateChanged;

        public GameState StartNewGame(GameSettings settings, List<Player> players)
        {
            _currentEngine = CreateEngine(settings.GameMode);
            _currentGame = _currentEngine.InitializeGame(settings, players);
            OnGameStateChanged?.Invoke();
            return _currentGame;
        }

        public Turn CreateNewTurn()
        {
            if (_currentGame == null)
                throw new InvalidOperationException("No active game");

            return new Turn
            {
                PlayerId = _currentGame.CurrentPlayer.PlayerId,
                TurnNumber = _currentGame.CurrentPlayer.TurnsPlayed + 1
            };
        }

        public (bool IsValid, string? ErrorMessage) ValidateThrow(Throw dartThrow)
        {
            if (_currentGame == null)
                return (false, "No active game");

            return _currentEngine.ValidateThrow(_currentGame, dartThrow);
        }

        public void ProcessTurn(Turn turn)
        {
            if (_currentGame == null)
                throw new InvalidOperationException("No active game");

            _currentGame = _currentEngine.ProcessTurn(_currentGame, turn);
            OnGameStateChanged?.Invoke();
        }

        public bool IsGameFinished()
        {
            if (_currentGame == null)
                return false;

            return _currentEngine.IsGameFinished(_currentGame);
        }

        public int? GetWinner()
        {
            if (_currentGame == null)
                return null;

            return _currentEngine.GetWinner(_currentGame);
        }

        public List<int> GetPossibleCheckouts(int score)
        {
            return _currentEngine?.GetPossibleCheckouts(score) ?? new List<int>();
        }

        public void EndGame()
        {
            _currentGame = null;
            OnGameStateChanged?.Invoke();
        }

        private IGameEngine CreateEngine(GameMode gameMode)
        {
            return gameMode switch
            {
                GameMode.X01 => new X01GameEngine(),
                GameMode.Cricket => new CricketGameEngine(),
                GameMode.AroundTheClock => new AroundTheClockGameEngine(),
                _ => throw new ArgumentException($"Unknown game mode: {gameMode}")
            };
        }
    }
}
