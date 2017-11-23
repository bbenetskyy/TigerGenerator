using DevExpress.XtraSplashScreen;
using Microsoft.Office.Interop.Word;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using TigerGenerator.Controls.Properties;
using TigerGenerator.Logic.Cluster;
using TigerGenerator.Logic.Cluster.Interfaces;
using TigerGenerator.Logic.DocumentReaders.Excel;
using TigerGenerator.Logic.DocumentWriters;
using TigerGenerator.Logic.DocumentWriters.Word;
using TigerGenerator.Logic.Helpers;
using TigerGenerator.Logic.Models;
using Application = Microsoft.Office.Interop.Word.Application;
using IDataReader = TigerGenerator.Logic.DocumentReaders.IDataReader;

namespace TigerGenerator.Controls.Forms
{
    public partial class SplashForm : SplashScreen
    {
        //todo here refactoring is needed
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Application _application;
        private Document _document;

        public SplashForm()
        {
            InitializeComponent();
        }


        private void MainForm_Shown(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.Multiselect = false;
                    openFileDialog.Filter = @"Excel Files|*.xls;*.xlsx";
                    openFileDialog.Title = "Select Excel File With Data";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var fileName = openFileDialog.FileName;
                        //todo add Unity Container
                        using (IDataReader excelDataReader = new ExcelDataReader())
                        {
                            if (excelDataReader is INotifyChanges)
                                ((INotifyChanges)excelDataReader).SendNotification += SendNotification;
                            excelDataReader.ReaderDetails = fileName;
                            var response = excelDataReader.ReadData();

                            if (response.Success)
                            {
                                WritePlayers(response.ReturnValue as IEnumerable<PlayersGroup>);
                                SendNotification(null, "Work Completed.");
                            }
                            else
                            {
                                foreach (var exception in response.Errors)
                                {
                                    Logger.Error(exception);
                                }
                                SendNotification(null,
                                    string.Format(Resources.SplashForm_Errors_Log, response.Errors.Count));
                            }
                        }
                    }
                    SystemSounds.Beep.Play();
                    Thread.Sleep(500);
                    MainForm.Finished = true;
                }
            });
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void SendNotification(object sender, string e)
        {
            Invoke(new Action(() =>
            {
                lInfo.Text = e;
            }));
        }

        private void WritePlayers(IEnumerable<PlayersGroup> playersGroups)
        {
            Logger.Info("Starting WritePLayers");

            if (playersGroups == null)
            {
                Logger.Warn("End WritePLayers, because playersGroups == null");
                return;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //todo add Unity Container
            IDataWriter dataWriter = new WordDataWriter();
            //IDataWriter dataWriter = new TextDataWriter();
            dataWriter.TargetDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "TigerGenerator");

            IClusterWorker worker = new ClusterWorker();
            foreach (var group in playersGroups)
            {
                SendNotification(null, $"Work with group {group.Type} {group.Weight}");

                worker.Players = group.Players;
                worker.Work();

                dataWriter.WriterDetails = $"{group.Type}_{group.Weight}";
                dataWriter.WriteData(worker.Clusters);
            }

            stopwatch.Stop();
            Logger.Info($"End WritePLayers. Time = {stopwatch.Elapsed}");
        }


    }
}