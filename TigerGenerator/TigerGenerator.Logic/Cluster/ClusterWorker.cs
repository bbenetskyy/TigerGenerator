using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using TigerGenerator.Logic.Cluster.Interfaces;
using TigerGenerator.Logic.Models;
using JetBrains.Annotations;
using ToolsPortable;

namespace TigerGenerator.Logic.Cluster
{
   public class ClusterWorker: IClusterWorker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public List<Player> Players { get; set; }
        public List<SimpleCluster> Clusters { get; set; }

        [NotNull]
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
                sCluster[index] = SelectPlayer(lastPlayer)?.ToString();
            }

            stopwatch.Stop();
            Logger.Info("Finish Work.");
            Logger.Info($"Result Clusters = {Clusters.Count}");
            Logger.Info($"Work time = {stopwatch.Elapsed}");

            return new Response();
        }

        [CanBeNull]
        protected virtual  Player SelectPlayer([CanBeNull] Player lastPlayer)
        {
            if (Players == null || Players.Count == 0)
                return null;

            Player resPlayer = null;
            var random = new Random();


            if (lastPlayer == null)
            {
                resPlayer = Players[random.Next(0, Players.Count - 1)];
            }
            else
            {
                var otherTeamPlayers = Players.Where(p => p.Team != lastPlayer.Team).ToList();
                if (otherTeamPlayers.Count != 0)
                {
                    
                }
                else
                {
                    var sameTeamPlayers = Players.Where(p => p.Team == lastPlayer.Team).ToList();
                    if (sameTeamPlayers.Count != 0)
                    {
                        var otheMentorPlayers = sameTeamPlayers.Where(p => p.Mentor != lastPlayer.Mentor).ToList();
                        resPlayer = otheMentorPlayers.Count != 0
                            ? otheMentorPlayers[random.Next(0, otheMentorPlayers.Count - 1)]
                            : sameTeamPlayers[random.Next(0, sameTeamPlayers.Count - 1)];
                    }
                }
            }

            lastPlayer = resPlayer;
            return resPlayer;
        }

        protected virtual void UpdateIndex(SimpleCluster sCluster, ref int index)
        {
            if (sCluster == null)
                sCluster = new SimpleCluster();

            index++;
            if (index <= 3) return;

            index = 0;
            sCluster = new SimpleCluster();
            Clusters.Add(sCluster);
        }
    }
}
