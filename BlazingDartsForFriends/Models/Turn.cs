namespace BlazingDartsForFriends.Models
{
    public class Throw
    {
        public int Score { get; set; }
        public int Multiplier { get; set; } = 1;
        public bool IsBust { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public int TotalScore => Score * Multiplier;

        public bool IsDouble => Multiplier == 2;
        public bool IsTriple => Multiplier == 3;
    }

    public class Turn
    {
        public int PlayerId { get; set; }
        public List<Throw> Throws { get; set; } = new();
        public int TurnNumber { get; set; }
        public bool IsBust { get; set; }
        public int TotalScore => IsBust ? 0 : Throws.Sum(t => t.TotalScore);

        public void AddThrow(Throw dartThrow)
        {
            if (Throws.Count < 3)
            {
                Throws.Add(dartThrow);
            }
        }

        public bool IsComplete => Throws.Count >= 3 || IsBust;
    }
}
