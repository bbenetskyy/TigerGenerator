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
        public List<SimpleCluster> Clusters { get;  } = new List<SimpleCluster>();

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

            for (var i = 0; i < Players.Count; i++)
            {
                UpdateIndex(ref sCluster, ref index);
                sCluster[index] = SelectPlayer(ref lastPlayer)?.ToString();
            }

            stopwatch.Stop();
            Logger.Info("Finish Work.");
            Logger.Info($"Result Clusters = {Clusters.Count}");
            Logger.Info($"Work time = {stopwatch.Elapsed}");

            return new Response();
        }

        [CanBeNull]
        protected virtual  Player SelectPlayer([CanBeNull]ref Player lastPlayer)
        {
            Player resPlayer = null;
            var random = new Random();
            var notUsedPlayers = Players?.Where(p => !p.Used).ToList();

            if (notUsedPlayers == null || notUsedPlayers.Count == 0)
                return null;

            if (lastPlayer == null)
            {
                resPlayer = notUsedPlayers[random.Next(0, notUsedPlayers.Count - 1)];
            }
            else
            {
                var mLastPlayer = lastPlayer;
                var otherTeamPlayers = notUsedPlayers.Where(p => p.Team != mLastPlayer.Team).ToList();
                if (otherTeamPlayers.Count != 0)
                {
                    resPlayer = otherTeamPlayers[random.Next(0, otherTeamPlayers.Count - 1)];
                }
                else
                {
                    var sameTeamPlayers = notUsedPlayers.Where(p => p.Team == mLastPlayer.Team).ToList();
                    if (sameTeamPlayers.Count != 0)
                    {
                        var otheMentorPlayers = sameTeamPlayers.Where(p => p.Mentor != mLastPlayer.Mentor).ToList();
                        resPlayer = otheMentorPlayers.Count != 0
                            ? otheMentorPlayers[random.Next(0, otheMentorPlayers.Count - 1)]
                            : sameTeamPlayers[random.Next(0, sameTeamPlayers.Count - 1)];
                    }
                }
            }
            if (resPlayer != null)
                resPlayer.Used = true;
            lastPlayer = resPlayer;
            return resPlayer;
        }

        protected virtual void UpdateIndex(ref SimpleCluster sCluster, ref int index)
        {
            if (sCluster == null)
            {
                sCluster = new SimpleCluster();
                Clusters.Add(sCluster);
            }   

            index++;
            if (index <= 3) return;

            index = 0;
            sCluster = new SimpleCluster();
            Clusters.Add(sCluster);
        }
    }
}
