using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using TigerGenerator.Logic.Cluster.Interfaces;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Logic.Cluster
{
   public class ClusterWorker: IClusterWorker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public List<Player> Players { get; set; }
        public List<SimpleCluster> Clusters { get; set; }

        public virtual Response Work()
        {
            Logger.Info("Start Work.");
            Logger.Info($"Initial Players = {Players.Count}");

            var stopwatch = new Stopwatch();
            SimpleCluster sCluster = null;
            Player lastPlayer = null;
            var index = -1;

            stopwatch.Start();
            Clusters.Clear();

            foreach (var player in Players)
            {
                UpdateIndex(sCluster, ref index);
                sCluster[index] = SelectPlayer(lastPlayer);
            }

            stopwatch.Stop();
            Logger.Info("Finish Work.");
            Logger.Info($"Result Clusters = {Clusters.Count}");
            Logger.Info($"Work time = {stopwatch.Elapsed}");
        }

        private Player SelectPlayer(Player lastPlayer)
        {
            throw new NotImplementedException();
        }

        protected virtual void UpdateIndex(SimpleCluster sCluster, ref int index)
        {
            index++;
            if (index <= 3) return;

            index = 0;
            sCluster = new SimpleCluster();
            Clusters.Add(sCluster);
        }
    }
}
