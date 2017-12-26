using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigerGenerator.Logic.ViewModels
{
    public class PlayerViewModel
    {
        public readonly Guid Id;
        public string Name { get; set; }

        public int LocalScore { get; set; }

        public int FullScore { get; set; }

        public PlayerViewModel()
            : this(Guid.NewGuid()) { }

        public PlayerViewModel(Guid id)
        {
            Id = id;
        }

        public void SetMatchResults()
        {
            FullScore += LocalScore;
            LocalScore = 0;
        }
    }
}
