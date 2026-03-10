using BlazingDartsForFriends.Pages;
using BlazingDartsForFriends.Services;
using BlazingDartsForFriends.Models;
using Classes;

namespace BlazingDartsForFriends.StateContainerService
{
    public class StateContainer
    {
        public bool isAddPlayerComponentVisible { get; set; } = false;

        public bool isGameOver { get; set; } = true;

        public bool homeNeedsReRender { get; set; } = false;

        public int playerID { get; set; } = 0;

        public Home? home;
        public Game? game;

        public GameService GameService { get; set; } = new GameService();

        public GameSettings CurrentGameSettings { get; set; } = new GameSettings();

        public List<Player> Players { get; set; } = new List<Player>();

        public List<Player> activePlayers { get; set; } = new List<Player>();

        public event Action? OnStateChanged;

        public void NotifyStateChanged()
        {
            OnStateChanged?.Invoke();
        }
    }
}
