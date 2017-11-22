//#define GetMaxPlayerLenght

using NLog;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TigerGenerator.Logic.Cluster;
using TigerGenerator.Logic.Helpers;
using TigerGenerator.Logic.Models;
using ToolsPortable;

namespace TigerGenerator.Logic.DocumentWriters.Text
{
    public class TextDataWriter : IDataWriter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private object _writerDetails;

        public string TargetDirectory { get; set; }

        public object WriterDetails
        {
            get => _writerDetails;
            set
            {
                _writerDetails = value;
                if (_writerDetails is string fileName)
                    FileName = fileName;
            }
        }

        public string FileName { get; set; }

        public Response WriteData(List<SimpleCluster> clusters)
        {
            Logger.Info($"Starting ReadData from FileName =  {FileName}; ReaderDetails = {WriterDetails}");
            var stopwatch = new Stopwatch();
            var response = new Response();
            stopwatch.Start();

            GenerateTargetDirectory();

            var tigerFile = Path.Combine(TargetDirectory, $"{FileName}.txt");
            var builder = new MultiLineStringBuilder();
#if GetMaxPlayerLenght
            var placeholderLenght = GetMaxPlayerLenght(clusters);
#else
            var placeholderLenght = 10;
#endif
            var groupSize = 2;

            SetClusteredPlayers(builder, clusters);

            var playersCount = builder.Count;
            var lastId = builder.Count;
            var groupMmultiplier = 2;


            if (playersCount % 2 != 0)
            {
                AddAdditionalFight(builder, placeholderLenght, ref lastId);
                playersCount -= 1;
            }

            do
            {
                AddNextFights(builder, placeholderLenght, groupSize, ref lastId);
                groupSize *= groupMmultiplier;
            } while (groupSize <= playersCount);

            WriteDataToFile(tigerFile, builder.ToString());

            stopwatch.Stop();
            Logger.Info($"Finish ReadData. Success = {response.Success}; Errors = {response.Errors.Count}");
            Logger.Info($"ReadData reading time {stopwatch.Elapsed}");
            return response;
        }

        private void AddAdditionalFight(MultiLineStringBuilder builder, int placeholderLenght, ref int id)
        {
            var lastItem = builder.Count - 1;


            TabToNextLevel(builder);

            builder[lastItem--].Append($"#{++id}.{new string('_', placeholderLenght)}");
            builder[lastItem].Append($"#{++id}.{new string('_', placeholderLenght)}");
        }

        private void AddNextFights(MultiLineStringBuilder builder, int placeholderLenght, int groupSize, ref int id)
        {
            var playersCount = builder.Count;
            var startIndex = groupSize - 1;
            var index = groupSize / 2;

            TabToNextLevel(builder);

            do
            {
                builder[index].Append($"#{++id}.{new string('_', placeholderLenght)}");
                index = groupSize / 2 + 1 + startIndex;
                startIndex += groupSize;
            } while (index < playersCount);
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


        public void WriteDataToFile(string tigerFile, string text)
        {
            using (var writer = new StreamWriter(tigerFile))
            {
                writer.WriteLine(text);
            }
        }

        private void GenerateTargetDirectory()
        {
            if (!Directory.Exists(TargetDirectory))
                Directory.CreateDirectory(TargetDirectory);
        }
    }
}
