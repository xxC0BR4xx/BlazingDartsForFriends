namespace Classes
{
    public class Player
    {
        public Player(int id, string Name) {
            this.Id = id;
            this.Name = Name;
            this.currentScoreStack = new Stack<int>();
            this.scoreStack = new Stack<int>();

        }

        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public Stack<int> scoreStack;
        public Stack<int> currentScoreStack;

        public bool IsActive { get; set; }

        public int goalScore { get; set; }

        public double getCurrentAverage()
        {
            int average = 0;
            foreach(int i in currentScoreStack) {
                average += i;
            }
            int j = currentScoreStack.Count;
            if (j > 0)
            {
                average = average / j;

            }
            else average = 0;
            return average;
        }
        
    }
}
