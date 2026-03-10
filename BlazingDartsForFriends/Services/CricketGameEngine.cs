using BlazingDartsForFriends.Models;

namespace BlazingDartsForFriends.Services
{
    public class CricketGameEngine : BaseGameEngine
    {
        private readonly int[] _cricketNumbers = { 15, 16, 17, 18, 19, 20, 25 };

        protected override int GetInitialScore(GameSettings settings)
        {
            return 0;
        }

        public override (bool IsValid, string? ErrorMessage) ValidateThrow(GameState gameState, Throw dartThrow)
        {
            if (!_cricketNumbers.Contains(dartThrow.Score))
            {
                return (false, "In Cricket, you can only hit 15-20 and Bull (25)");
            }

            if (dartThrow.Multiplier < 1 || dartThrow.Multiplier > 3)
            {
                return (false, "Invalid multiplier. Must be 1, 2, or 3");
            }

            if (dartThrow.Score == 25 && dartThrow.Multiplier > 2)
            {
                return (false, "Bull can only be single (25) or double (50)");
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
                var hits = dartThrow.Multiplier;
                var score = dartThrow.Score;

                currentPlayer.ScoreHistory.Add(dartThrow.TotalScore);
            }

            gameState.History.Add(turn);

            if (IsGameFinished(gameState))
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
