namespace BlazingDartsForFriends.Models
{
    public class GameState
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public GameSettings Settings { get; set; } = new();
        public List<PlayerGameState> PlayerStates { get; set; } = new();
        public int CurrentPlayerIndex { get; set; }
        public List<Turn> History { get; set; } = new();
        public bool IsFinished { get; set; }
        public int? WinnerId { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAt { get; set; }

        public PlayerGameState CurrentPlayer => PlayerStates[CurrentPlayerIndex];

        public void NextPlayer()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % PlayerStates.Count;
        }
    }

    public class PlayerGameState
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int CurrentScore { get; set; }
        public List<int> ScoreHistory { get; set; } = new();
        public int DartsThrown { get; set; }
        public int TurnsPlayed { get; set; }

        public double Average
        {
            get
            {
                if (DartsThrown == 0) return 0;
                var totalScored = ScoreHistory.Sum();
                return Math.Round((double)totalScored / DartsThrown * 3, 2);
            }
        }

        public double CheckoutPercentage { get; set; }
    }
}
