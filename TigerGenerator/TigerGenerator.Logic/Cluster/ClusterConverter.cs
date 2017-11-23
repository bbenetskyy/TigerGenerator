//#define GetMaxPlayerLenght

using NLog;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TigerGenerator.Logic.Cluster.Interfaces;
using TigerGenerator.Logic.Helpers;
using ToolsPortable;

namespace TigerGenerator.Logic.Cluster
{
    public class ClusterConverter : IClusterConverter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string ConvertToString(List<SimpleCluster> clusters)
        {
            Logger.Info($"Starting convert data from clusters. Clusters.Count = {clusters.Count};");
            var stopwatch = new Stopwatch();
            var builder = new MultiLineStringBuilder();
#if GetMaxPlayerLenght
            var placeholderLenght = GetMaxPlayerLenght(clusters);
#else
            var placeholderLenght = 10;
#endif
            var groupSize = 2;
            var groupMmultiplier = 2;

            SetClusteredPlayers(builder, clusters);

            var playersCount = builder.Count;
            var lastId = builder.Count;
            var itemsForAdd = playersCount / groupSize;

            if (playersCount % 2 != 0)
            {
                AddAdditionalFight(builder, placeholderLenght, ref lastId);
                playersCount -= 1;
            }

            while (!IsPowerOfTwo(playersCount))
            {
                playersCount += 2;
            }

            do
            {
                AddNextFights(builder, placeholderLenght, groupSize, itemsForAdd, ref lastId);

                groupSize *= groupMmultiplier;
                itemsForAdd = playersCount / groupSize;
            } while (groupSize <= playersCount);

            stopwatch.Stop();
            Logger.Info("Finish Convert.");
            Logger.Info($"Converting time {stopwatch.Elapsed}");
            return builder.ToString();
        }


        private void AddAdditionalFight(MultiLineStringBuilder builder, int placeholderLenght, ref int id)
        {
            var lastItem = builder.Count - 1;


            TabToNextLevel(builder);

            builder[lastItem--].Append($"#{++id}.{new string('_', placeholderLenght)}");
            builder[lastItem].Append($"#{++id}.{new string('_', placeholderLenght)}");
        }

        private bool IsPowerOfTwo(int x)
        {
            return x > 0 && (x & (x - 1)) == 0;
        }

        private void AddNextFights(MultiLineStringBuilder builder, int placeholderLenght, int groupSize, int itemsForAdd, ref int id)
        {
            var playersCount = builder.Count;
            var startIndex = groupSize - 1;
            var index = groupSize / 2;
            var itemsAdded = 0;

            TabToNextLevel(builder);

            do
            {
                if (index >= playersCount)
                {
                    startIndex -= groupSize + 1 - (playersCount - startIndex);
                    index = groupSize / 2 + 1 + startIndex;
                }

                if (index < playersCount)
                    builder[index].Append($"#{++id}.{new string('_', placeholderLenght)}");

                index = groupSize / 2 + 1 + startIndex;
                startIndex += groupSize;

                itemsAdded++;
            } while (itemsAdded < itemsForAdd);
        }

        private static void TabToNextLevel(MultiLineStringBuilder builder, int tabsCount = 4)
        {
            for (int index = 0; index < builder.Count; index++)
            {
                builder[index].Append(new string('\t', tabsCount));
            }
        }

        public int GetMaxPlayerLenght(List<SimpleCluster> clusters) => clusters.Max(GetMaxPlayerLenght);

        private int GetMaxPlayerLenght(SimpleCluster cluster) => cluster.Max(p => p?.Length ?? 1);

        private void SetClusteredPlayers(MultiLineStringBuilder builder, List<SimpleCluster> clusters)
        {
            var maxPlayerLenght = GetMaxPlayerLenght(clusters);
            var id = 0;
            foreach (var cluster in clusters)
            {
                foreach (var player in cluster)
                {
                    if (player.IsNotBlank())
                        builder.AppendLine($"#{++id}. {player} {new string(' ', maxPlayerLenght - player.Length)}");
                }
            }
        }
    }
}
