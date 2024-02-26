namespace Classes
{
    public class Player
    {
        public Player(int id, string Name) {
            this.Id = id;
            this.Name = Name;
        }

        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
            public string Name { get; set; }

        
        
    }
}
