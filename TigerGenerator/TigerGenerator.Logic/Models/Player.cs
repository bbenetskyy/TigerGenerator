using System.Diagnostics;

namespace TigerGenerator.Logic.Models
{
    [DebuggerDisplay("{Team} => {Mentor} => {Initials} => {Used}")]
    public class Player
    {
        public string Team { get; set; }
        public string Mentor { get; set; }
        public string Initials { get; set; }
        public bool Used { get; set; }

        public override string ToString()
        {
            return $"{Initials} {Team}";
        }
    }
}
