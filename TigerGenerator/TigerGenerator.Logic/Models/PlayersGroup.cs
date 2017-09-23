using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigerGenerator.Logic.Models
{
    public class PlayersGroup
    {
        public string Type { get; set; }

        public string Weight { get; set; }

        public List<Player> Players { get; } = new List<Player>();
    }
}
