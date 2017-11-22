using NLog;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TigerGenerator.Logic.Cluster;
using TigerGenerator.Logic.Helpers;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Logic.DocumentWriters.Text
{
    public class TextDataWriter : IDataWriter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private object _writerDetails;

        public string TargetDirectory { get; set; }

        public object WriterDetails
        {
            get { return _writerDetails; }
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
            var maxPlayerLenght = GetMaxPlayerLenght(clusters);
            var groupSize = 2;
            var playersCount = builder.Count;
            var lastId = builder.Count;

            SetClusteredPlayers(builder, clusters);

            if (playersCount % 2 != 0)
            {
                AddAdditionalFight(builder, maxPlayerLenght, ref lastId);
                playersCount -= 1;
            }

            do
            {
                AddNextFights(builder, maxPlayerLenght, groupSize, ref lastId);
                groupSize *= 2;
            } while (groupSize <= playersCount);

            WriteDataToFile(tigerFile, builder.ToString());

            stopwatch.Stop();
            Logger.Info($"Finish ReadData. Success = {response.Success}; Errors = {response.Errors.Count}");
            Logger.Info($"ReadData reading time {stopwatch.Elapsed}");
            return response;
        }

        private void AddAdditionalFight(MultiLineStringBuilder builder, int maxPlayerLenght, ref int id)
        {
            var lastItem = builder.Count - 1;

            for (int index = 0; index < builder.Count; index++)
            {
                builder[index].Append(new string(' ', maxPlayerLenght));
            }

            builder[lastItem--].Append($"#{++id}.{new string('_', maxPlayerLenght)}");
            builder[lastItem].Append($"#{++id}.{new string('_', maxPlayerLenght)}");
        }

        private void AddNextFights(MultiLineStringBuilder builder, int maxPlayerLenght, int groupSize, ref int id)
        {
            var playersCount = builder.Count;
            var startIndex = groupSize - 1;

            //todo repeated part !!!!
            for (int index = 0; index < builder.Count; index++)
            {
                builder[index].Append(new string(' ', maxPlayerLenght));
            }

            do
            {
                var index = groupSize / 2 + 1 + startIndex;
                if (index < playersCount)
                    builder[index].Append($"#{++id}.{new string('_', maxPlayerLenght)}");
                startIndex += groupSize;
            } while (startIndex < playersCount);
        }

        public int GetMaxPlayerLenght(List<SimpleCluster> clusters) => clusters.Max(c => c.Max(p => p?.Length ?? 1)) * 2;

        private void SetClusteredPlayers(MultiLineStringBuilder builder, List<SimpleCluster> clusters)
        {
            var id = 0;
            foreach (var cluster in clusters)
            {
                foreach (var player in cluster)
                {
                    builder.AppendLine($"#{++id}. {player}");
                }
            }
        }

        public void WriteDataToFile(string tigerFile, string text)
        {
            new StreamWriter(tigerFile).WriteLine(text);
        }

        private void GenerateTargetDirectory()
        {
            if (!Directory.Exists(TargetDirectory))
                Directory.CreateDirectory(TargetDirectory);
        }
    }
}
