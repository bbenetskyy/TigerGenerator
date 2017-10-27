using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using NLog;
using TigerGenerator.Logic.DocumentReaders.Excel;
using TigerGenerator.Logic.Models;
using Microsoft.Office.Interop.Word;
using TigerGenerator.Controls.Properties;
using TigerGenerator.Logic.Cluster;
using TigerGenerator.Logic.Cluster.Interfaces;
using Application = Microsoft.Office.Interop.Word.Application;
using System = Microsoft.Office.Interop.Word.System;
using Task = System.Threading.Tasks.Task;

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
                        using (var excelDataReader = new ExcelDataReader())
                        {
                            excelDataReader.ReaderDetails = fileName;
                            var response = excelDataReader.ReadData();

                            if (response.Success)
                            {
                                WritePlayers(response.ReturnValue as IEnumerable<PlayersGroup>);
                            }
                            else
                            {
                                XtraMessageBox.Show(string.Format(Resources.SplashForm_Errors_Log,response.Errors.Count));
                                foreach (var exception in response.Errors)
                                {
                                    Logger.Error(exception);
                                }
                                Invoke(new Action(() =>
                                {
                                    lInfo.Text = Resources.SplashForm_Errors_Message;
                                }));
                                SystemSounds.Beep.Play();
                                Thread.Sleep(2000);
                            }
                        }
                    }
                    MainForm.Finished = true;
                }
            });
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
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

            var tigerDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "TigerGenerator");
            if (!Directory.Exists(tigerDirectory))
                Directory.CreateDirectory(tigerDirectory);

            IClusterWorker worker = new ClusterWorker();
            foreach (var group in playersGroups)
            {
                var templateFileName = GetTemplateFileName(group.Players.Count);
                var tigerFile = Path.Combine(tigerDirectory, $"{group.Type}_{group.Weight}.docx");
                File.Copy(templateFileName, tigerFile, true);

                _application = new Application();
                _document = _application.Documents.Open(templateFileName);
                
                _document.Shapes["TextTitle"].TextFrame.TextRange.Text = $"{group.Type}\n{group.Weight}";

                worker.Players = group.Players;
                worker.Work();
                var id = 1;
                foreach (var cluster in worker.Clusters)
                {
                    foreach (var clusterItem in cluster)
                    {
                        _document.Shapes[$"Player_{id++}"].TextFrame.TextRange.Text = clusterItem;

                    }
                }

                _document.Save();
                ReleaseMemory();
            }

            ReleaseMemory();
            stopwatch.Stop();
            Logger.Info($"End WritePLayers. Time = {stopwatch.Elapsed}");
        }

        private string GetTemplateFileName(int playersCount)
        {
            var fileTemplate =Path.Combine(Directory.GetCurrentDirectory(),@"Templates\Template{0}.docx");
            var id = 0;
            if (playersCount <= 4)
                id = 4;
            else if (playersCount <= 8)
                id = 8;
            else id = 16;
            return string.Format(fileTemplate, id);
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
    }
}