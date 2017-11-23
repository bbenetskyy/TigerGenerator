using System.Collections.Generic;

namespace TigerGenerator.Logic.Cluster.Interfaces
{
    public interface IClusterConverter
    {
        string ConvertToString(List<SimpleCluster> clusters);
    }
}
