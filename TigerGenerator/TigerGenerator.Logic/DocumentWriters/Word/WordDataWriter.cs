using Microsoft.Office.Interop.Word;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using TigerGenerator.Logic.Cluster;
using TigerGenerator.Logic.Cluster.Interfaces;
using TigerGenerator.Logic.Models;
using Application = Microsoft.Office.Interop.Word.Application;

namespace TigerGenerator.Logic.DocumentWriters.Word
{
    public class WordDataWriter : IDataWriter, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private object _writerDetails;
        private Application _application;
        private Document _document;

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

            var tigerFile = Path.Combine(TargetDirectory, $"{FileName}.docx");
            WriteDataToFile(tigerFile, text);

            stopwatch.Stop();
            Logger.Info($"Finish WriteDate. Success = {response.Success}; Errors = {response.Errors.Count}");
            Logger.Info($"WriteDate reading time {stopwatch.Elapsed}");
            return response;
        }

        public Response WriteData(List<SimpleCluster> clusters)
        {
            //todo add unity container
            IClusterConverter converter = new ClusterConverter();
            return WriteData(converter.ConvertToString(clusters));
        }

        public void WriteDataToFile(string tigerFile, string text)
        {
            var fileTemplate = Path.Combine(Directory.GetCurrentDirectory(), @"Templates\Template.docx");
            File.Copy(fileTemplate, tigerFile, true);

            _application = new Application();
            _document = _application.Documents.Open(tigerFile);

            _document.Shapes["TextTitle"].TextFrame.TextRange.Text = FileName;

            var range = _document.Range(0, 0);
            range.Text = new string('\n', 5);
            range.Text += text;
            range.Font.Size = 8;

            _document.Save();
            ReleaseMemory();
        }

        private void GenerateTargetDirectory()
        {
            if (!Directory.Exists(TargetDirectory))
                Directory.CreateDirectory(TargetDirectory);
        }

        public void ReleaseMemory()
        {
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            if (_document != null)
            {
                //close and release
                _document.Close();
                Marshal.ReleaseComObject(_document);
                _document = null;
            }

            if (_application != null)
            {
                //quit and release
                _application.Quit();
                Marshal.ReleaseComObject(_application);
                _application = null;
            }
        }

        public void Dispose()
        {
            ReleaseMemory();
        }
    }
}

