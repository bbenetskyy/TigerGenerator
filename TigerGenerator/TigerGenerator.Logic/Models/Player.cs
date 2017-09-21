namespace TigerGenerator.Logic.Models
{
    public class Player
    {
        public string Team { get; set; }
        public string Mentor { get; set; }
        public string Initials { get; set; }

        public override string ToString()
        {
            return $"{Initials} {Team}";
        }
    }
}
