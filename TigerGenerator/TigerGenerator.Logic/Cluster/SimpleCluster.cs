using System.Runtime.CompilerServices;
using NLog;

namespace TigerGenerator.Logic.Cluster
{
    public class SimpleCluster
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string LeftTop { get; set; }
        public string LeftBottom { get; set; }
        public string RightTop { get; set; }
        public string RigthBottom { get; set; }

        [IndexerName("ClusterItem")]
        public string this[int index]   // Indexer declaration  
        {
            get
            {
                Logger.Info($"SimpleCluster get index {index}");
                while (index > 3)
                {
                    index -= 4;
                }
                switch (index)
                {
                    case 0:
                        return LeftTop;
                    case 1:
                        return LeftBottom;
                    case 2:
                        return RightTop;
                    case 3:
                        return RigthBottom;
                    default:
                        Logger.Warn($"Index ({index}) our of range!");
                        return string.Empty;
                }
            }

            set
            {
                Logger.Info($"SimpleCluster set index {index}");
                while (index > 3)
                {
                    index -= 4;
                }
                switch (index)
                {
                    case 0:
                        LeftTop = value;
                        break;
                    case 1:
                        LeftBottom = value;
                        break;
                    case 2:
                        RightTop = value;
                        break;
                    case 3:
                        RigthBottom = value;
                        break;
                    default:
                        Logger.Warn($"Index ({index}) our of range!");
                        break;
                }
            }
        }
    }
}
