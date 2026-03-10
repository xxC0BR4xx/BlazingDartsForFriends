using BlazingDartsForFriends.Models;

namespace BlazingDartsForFriends.Services
{
    public class AroundTheClockGameEngine : BaseGameEngine
    {
        protected override int GetInitialScore(GameSettings settings)
        {
            return 1;
        }

        public override (bool IsValid, string? ErrorMessage) ValidateThrow(GameState gameState, Throw dartThrow)
        {
            if (dartThrow.Score < 1 || dartThrow.Score > 20)
            {
                return (false, "Score must be between 1 and 20");
            }

            return (true, null);
        }

        public override GameState ProcessTurn(GameState gameState, Turn turn)
        {
            var currentPlayer = gameState.CurrentPlayer;

            currentPlayer.DartsThrown += turn.Throws.Count;
            currentPlayer.TurnsPlayed++;

            foreach (var dartThrow in turn.Throws)
            {
                if (dartThrow.Score == currentPlayer.CurrentScore)
                {
                    currentPlayer.CurrentScore++;
                    currentPlayer.ScoreHistory.Add(dartThrow.Score);
                }
            }

            gameState.History.Add(turn);

            if (currentPlayer.CurrentScore > 20)
            {
                gameState.IsFinished = true;
                gameState.WinnerId = currentPlayer.PlayerId;
                gameState.FinishedAt = DateTime.UtcNow;
            }
            else
            {
                gameState.NextPlayer();
            }

            return gameState;
        }

        public override bool IsGameFinished(GameState gameState)
        {
            return gameState.IsFinished;
        }

        public override int? GetWinner(GameState gameState)
        {
            return gameState.WinnerId;
        }
    }
}
