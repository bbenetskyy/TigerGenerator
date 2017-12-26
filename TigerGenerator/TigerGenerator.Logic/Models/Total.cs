using System.Collections.Generic;

namespace TigerGenerator.Logic.Models
{
    public class Total
    {
        public TigerPlayer FirstPlace { get; set; }
        public TigerPlayer SecondPlace { get; set; }
        public TigerPlayer ThirdPlace { get; set; }
        public List<WonPlace> UnOfficialPlaces { get; set; }
    }
}
