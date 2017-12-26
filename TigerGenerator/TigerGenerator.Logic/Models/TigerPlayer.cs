using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigerGenerator.Logic.Models
{
    public class TigerPlayer
    {
        public Player Player { get; set; }
        public int FullScore { get; set; }
        public List<Match> Matches { get; set; }
    }
}
