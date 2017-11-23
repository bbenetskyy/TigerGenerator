
using NLog;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TigerGenerator.Logic.Cluster;
using TigerGenerator.Logic.Cluster.Interfaces;
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
            get => _writerDetails;
            set
            {
                _writerDetails = value;
                if (_writerDetails is string fileName)
                    FileName = fileName;
            }
        }

        public string FileName { get; set; }

        public Response WriteData(string text)
        {
            Logger.Info($"Starting WriteDate from FileName =  {FileName}; ReaderDetails = {WriterDetails}");
            var stopwatch = new Stopwatch();
            var response = new Response();
            stopwatch.Start();

            GenerateTargetDirectory();

            var tigerFile = Path.Combine(TargetDirectory, $"{FileName}.txt");
            WriteDataToFile(tigerFile, text);

            stopwatch.Stop();
            Logger.Info($"Finish WriteDate. Success = {response.Success}; Errors = {response.Errors.Count}");
            Logger.Info($"WriteDate reading time {stopwatch.Elapsed}");
            return response;
        }

        public Response WriteData(List<SimpleCluster> clusters)
        {
            //todo addunity container
            IClusterConverter converter = new ClusterConverter();
            return WriteData(converter.ConvertToString(clusters));
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
