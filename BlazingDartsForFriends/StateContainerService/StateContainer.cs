using BlazingDartsForFriends.Pages;

namespace BlazingDartsForFriends.StateContainerService
{


    public class StateContainer
    {
        public bool isAddPlayerComponentVisible { get; set; } = false;

        public bool homeNeedsReRender { get; set; } = false;

        public int playerID { get; set; } = 0;

        public Home home;


    }
}
