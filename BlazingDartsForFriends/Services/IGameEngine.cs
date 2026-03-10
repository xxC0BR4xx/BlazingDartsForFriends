using BlazingDartsForFriends.Models;
using Classes;

namespace BlazingDartsForFriends.Services
{
    public interface IGameEngine
    {
        GameState InitializeGame(GameSettings settings, List<Player> players);
        (bool IsValid, string? ErrorMessage) ValidateThrow(GameState gameState, Throw dartThrow);
        GameState ProcessTurn(GameState gameState, Turn turn);
        bool IsGameFinished(GameState gameState);
        int? GetWinner(GameState gameState);
        List<int> GetPossibleCheckouts(int score);
    }

    public abstract class BaseGameEngine : IGameEngine
    {
        public virtual GameState InitializeGame(GameSettings settings, List<Player> players)
        {
            var gameState = new GameState
            {
                Settings = settings,
                CurrentPlayerIndex = 0
            };

            foreach (var player in players)
            {
                gameState.PlayerStates.Add(new PlayerGameState
                {
                    PlayerId = player.Id,
                    PlayerName = player.Name,
                    CurrentScore = GetInitialScore(settings)
                });
            }

            return gameState;
        }

        protected abstract int GetInitialScore(GameSettings settings);

        public abstract (bool IsValid, string? ErrorMessage) ValidateThrow(GameState gameState, Throw dartThrow);

        public abstract GameState ProcessTurn(GameState gameState, Turn turn);

        public abstract bool IsGameFinished(GameState gameState);

        public abstract int? GetWinner(GameState gameState);

        public virtual List<int> GetPossibleCheckouts(int score)
        {
            return new List<int>();
        }

        protected bool IsValidCheckout(int score, Throw dartThrow, CheckoutMode checkoutMode)
        {
            return checkoutMode switch
            {
                CheckoutMode.SingleOut => true,
                CheckoutMode.DoubleOut => dartThrow.IsDouble,
                CheckoutMode.MasterOut => dartThrow.IsDouble || dartThrow.IsTriple,
                _ => true
            };
        }
    }
}
