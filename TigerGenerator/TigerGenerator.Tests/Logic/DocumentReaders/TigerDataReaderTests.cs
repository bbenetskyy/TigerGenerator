﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigerGenerator.Logic.DocumentReaders;
using TigerGenerator.Logic.DocumentReaders.Tiger;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Tests.Logic.DocumentReaders
{
    [TestClass]
    public class TigerDataReaderTests
    {
        private string _filePath = "TestDataFile.tiger";
        private IEnhancedDataReader<TigerFileData> _iReader;
        private TigerDataReader _tReader;


        [TestInitialize]
        public void Init()
        {
            _tReader = new TigerDataReader();
            _iReader = _tReader;
        }


        [TestMethod]
        public void TestFileName()
        {
            _iReader.ReaderDetails = 1;
            Assert.AreEqual(_tReader.ReaderDetails, 1);
            Assert.IsNull(_tReader.FullPath);

            _iReader.ReaderDetails = _filePath;
            Assert.AreEqual(_tReader.ReaderDetails, _filePath);
            Assert.AreEqual(_tReader.FullPath, _filePath);

            _iReader.ReaderDetails = 1;
            Assert.AreEqual(_tReader.ReaderDetails, 1);
            Assert.AreEqual(_tReader.FullPath, _filePath);


            _iReader.ReaderDetails = "test";
            Assert.AreEqual(_tReader.ReaderDetails, "test");
            Assert.AreEqual(_tReader.FullPath, "test");
        }


        [TestMethod]
        public void ReadCorrectFile()
        {
            var team = "First Team";
            _iReader.ReaderDetails = _filePath;
            var respone = _iReader.ReadData();
            var returnValue = respone.ReturnValue as TigerFileData;

            Assert.IsTrue(respone.Success);
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(returnValue.Players.Length, 2);
            Assert.AreEqual(returnValue.Total.FirstPlace.Player.Team, team);
        }
    }
}
