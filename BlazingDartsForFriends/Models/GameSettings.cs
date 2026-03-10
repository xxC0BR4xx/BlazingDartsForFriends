namespace BlazingDartsForFriends.Models
{
    public class GameSettings
    {
        public GameMode GameMode { get; set; } = GameMode.X01;
        public int StartingScore { get; set; } = 301;
        public CheckoutMode CheckoutMode { get; set; } = CheckoutMode.SingleOut;
        public List<int> PlayerIds { get; set; } = new();
    }

    public enum GameMode
    {
        X01,
        Cricket,
        AroundTheClock
    }

    public enum CheckoutMode
    {
        SingleOut,
        DoubleOut,
        MasterOut
    }
}
