using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Logic.Cluster.Interfaces
{
    public interface IClusterWorker
    {
        List<Player> Players { get; set; }

        List<SimpleCluster> Clusters { get; }

        Response Work();
    }
}
