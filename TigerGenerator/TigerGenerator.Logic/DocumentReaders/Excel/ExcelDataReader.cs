using JetBrains.Annotations;
using Microsoft.Office.Interop.Excel;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using TigerGenerator.Logic.Helpers;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Logic.DocumentReaders.Excel
{
    public class ExcelDataReader : IDataReader, INotifyChanges
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private object _readerDetails;
        private Application _excel = null;
        private Workbook _wb = null;
        private Worksheet _sheet = null;
        private Range _usedRange = null;

        public event EventHandler<string> SendNotification;
        public object ReaderDetails
        {
            get { return _readerDetails; }
            set
            {
                _readerDetails = value;
                if (_readerDetails is string fileName)
                    FileName = fileName;
            }
        }

        public string FileName { get; set; }

        ~ExcelDataReader()
        {
            ReleaseMemory();
        }

        private void ReleaseMemory()
        {
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            if (_usedRange != null)
            {
                Marshal.ReleaseComObject(_usedRange);
                _usedRange = null;
            }
            if (_sheet != null)
            {
                Marshal.ReleaseComObject(_sheet);
                _sheet = null;
            }

            if (_wb != null)
            {
                //close and release
                _wb.Close();
                Marshal.ReleaseComObject(_wb);
                _wb = null;
            }

            if (_excel != null)
            {
                //quit and release
                _excel.Quit();
                Marshal.ReleaseComObject(_excel);
                _excel = null;
            }
        }

        public Response ReadData()
        {
            //todo move all strings to resources even log strings
            Logger.Info($"Starting ReadData from FileName =  {FileName}; ReaderDetails = {ReaderDetails}");
            SendNotification?.Invoke(this, "Reading the file...");

            var stopwatch = new Stopwatch();
            var response = new Response();
            stopwatch.Start();

            _excel = new Application();
            _wb = _excel.Workbooks.Open(FileName);
            _sheet = GetFirstSheet(_wb);

            if (_sheet == null)
            {
                response.Errors.Add(new FileLoadException("Sheet not found"));
            }
            else
            {
                _usedRange = _sheet.UsedRange;
                response.ReturnValue = GetDataModels(_usedRange, (1, 1));
                if (!((IEnumerable<PlayersGroup>)response.ReturnValue).Any())
                {
                    response.Errors.Add(new FileLoadException("Values not found"));
                }
            }
            stopwatch.Stop();
            Logger.Info($"Finish ReadData. Success = {response.Success}; Errors = {response.Errors.Count}");
            Logger.Info($"ReadData reading time {stopwatch.Elapsed}");
            return response;
        }


        [CanBeNull]
        private Worksheet GetFirstSheet([NotNull] Workbook workbook)
        {
            return workbook.Sheets.Count != 0
                ? workbook.Sheets[1]
                : null;
        }

        [NotNull]
        private IEnumerable<PlayersGroup> GetDataModels([NotNull] Range range, (int Row, int Column) positions)
        {
            var resList = new List<PlayersGroup>();
            PlayersGroup playersGroup = null;
            for (var row = positions.Row; row <= range.Rows.Count; row++)
            {
                if (PlayerGroupReaderHelper.IsNewGroup(range, row, positions.Column))
                {
                    playersGroup = PlayerGroupReaderHelper.GetGroup(range, row, positions.Column);
                    resList.Add(playersGroup);
                }
                else if (PlayerGroupReaderHelper.IsPlayer(range, row, positions.Column))
                {
                    if (playersGroup == null)
                    {
                        Logger.Warn($"playersGroup == null; resList = {resList.Count}; Row = {row}; Column = {positions.Column}");
                    }
                    else
                    {
                        playersGroup.Players.Add(PlayerGroupReaderHelper.GetPlayer(range, row, positions.Column));
                    }
                }
            }

            return resList;
        }

        public void Dispose()
        {
            ReleaseMemory();
        }

    }
}
