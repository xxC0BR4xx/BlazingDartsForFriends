using BlazingDartsForFriends.Models;

namespace BlazingDartsForFriends.Services
{
    public class X01GameEngine : BaseGameEngine
    {
        protected override int GetInitialScore(GameSettings settings)
        {
            return settings.StartingScore;
        }

        public override (bool IsValid, string? ErrorMessage) ValidateThrow(GameState gameState, Throw dartThrow)
        {
            if (dartThrow.Score < 0 || dartThrow.Score > 20 && dartThrow.Score != 25)
            {
                return (false, "Invalid score. Must be 0-20 or 25 (bull)");
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
            var scoreBeforeTurn = currentPlayer.CurrentScore;
            var totalThrows = turn.Throws.Count;

            currentPlayer.DartsThrown += totalThrows;
            currentPlayer.TurnsPlayed++;

            var turnScore = 0;
            var isBust = false;

            foreach (var dartThrow in turn.Throws)
            {
                var throwScore = dartThrow.TotalScore;
                turnScore += throwScore;

                var newScore = currentPlayer.CurrentScore - throwScore;

                if (newScore < 0 || newScore == 1)
                {
                    isBust = true;
                    turn.IsBust = true;
                    break;
                }

                if (newScore == 0)
                {
                    if (!IsValidCheckout(currentPlayer.CurrentScore, dartThrow, gameState.Settings.CheckoutMode))
                    {
                        isBust = true;
                        turn.IsBust = true;
                        break;
                    }

                    currentPlayer.CurrentScore = 0;
                    gameState.IsFinished = true;
                    gameState.WinnerId = currentPlayer.PlayerId;
                    gameState.FinishedAt = DateTime.UtcNow;
                    break;
                }

                currentPlayer.CurrentScore = newScore;
            }

            if (isBust)
            {
                currentPlayer.CurrentScore = scoreBeforeTurn;
                currentPlayer.ScoreHistory.Add(0);
            }
            else
            {
                currentPlayer.ScoreHistory.Add(turnScore);
            }

            gameState.History.Add(turn);

            if (!gameState.IsFinished)
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

        public override List<int> GetPossibleCheckouts(int score)
        {
            var checkouts = new List<int>();

            if (score <= 170 && score > 1)
            {
                for (int i = 1; i <= 20; i++)
                {
                    if (i * 3 == score) checkouts.Add(i);
                    if (i * 2 == score) checkouts.Add(i);
                }

                if (score == 50 || score == 25) checkouts.Add(25);
            }

            return checkouts;
        }
    }
}
