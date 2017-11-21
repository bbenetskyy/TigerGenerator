using NLog;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TigerGenerator.Logic.Cluster;
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
            //var tigerDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            //    "TigerGenerator");
            var stopwatch = new Stopwatch();
            var response = new Response();
            stopwatch.Start();

            GenerateTargetDirectory();

            var tigerFile = Path.Combine(TargetDirectory, $"{FileName}.txt");
            var builder = new StringBuilder();
            var maxPlayerLenght = GetMaxPlayerLenght(clusters);

            SetClusteredPlayers(builder, clusters);

            AddAdditionalFight(builder, maxPlayerLenght);

            WriteDataToFile(tigerFile, builder.ToString());

            stopwatch.Stop();
            Logger.Info($"Finish ReadData. Success = {response.Success}; Errors = {response.Errors.Count}");
            Logger.Info($"ReadData reading time {stopwatch.Elapsed}");
            return response;
        }

        private void AddAdditionalFight(StringBuilder builder, int maxPlayerLenght)
        {
            throw new System.NotImplementedException();
        }

        public int GetMaxPlayerLenght(List<SimpleCluster> clusters) => clusters.Max(c => c.Max(p => p.Length));

        private void SetClusteredPlayers(StringBuilder builder, List<SimpleCluster> clusters)
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
