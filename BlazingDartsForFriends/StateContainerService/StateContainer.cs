using BlazingDartsForFriends.Pages;
using Classes;

namespace BlazingDartsForFriends.StateContainerService
{


    public class StateContainer
    {
        public bool isAddPlayerComponentVisible { get; set; } = false;

        public bool isGameOver { get; set; } = true;

        public bool homeNeedsReRender { get; set; } = false;

        public int playerID { get; set; } = 0;

        public Home home;
        public Game game;



        public List<Player> Players { get; set; } = new List<Player>();


        public List<Player> activePlayers { get; set; } = new List<Player>();

    }
}
