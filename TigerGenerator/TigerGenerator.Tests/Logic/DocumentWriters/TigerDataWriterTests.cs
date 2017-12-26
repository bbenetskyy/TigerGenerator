using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TigerGenerator.Logic.DocumentWriters;
using TigerGenerator.Logic.DocumentWriters.Tiger;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Tests.Logic.DocumentWriters
{
    [TestClass]
    public class TigerDataWriterTests
    {
        private string _filePath = @"TestFiles\OutFile.tiger";
        private IEnhancedDataWriter<TigerFileData> _iWriter;
        private TigerDataWriter _tWriter;
        private TigerFileData _testData;


        [TestInitialize]
        public void Init()
        {
            _tWriter = new TigerDataWriter();
            _iWriter = _tWriter;

            var playerOne = new Player
            {
                Initials = "First Player",
                Mentor = "First Mentor",
                Team = "First Team",
                Used = true
            };
            var playerTwo = new Player
            {
                Initials = "Second Player",
                Mentor = "Second Mentor",
                Team = "Second Team",
                Used = true
            };
            var tigerPlayerOne = new TigerPlayer
            {
                Player = playerOne,
                FullScore = 3,
                Matches = new List<Match>
                {
                    new Match
                    {
                        LocalScore = 1,
                        Played = true,
                        Position = 1
                    },

                    new Match
                    {
                        LocalScore = 4,
                        Played = false,
                        Position = 1
                    }
                }
            };
            var tigerPlayerTwo = new TigerPlayer
            {
                Player = playerTwo,
                FullScore = 2,
                Matches = new List<Match>
                {
                    new Match
                    {
                        LocalScore = 0,
                        Played = true,
                        Position = 1
                    },

                    new Match
                    {
                        LocalScore = 3,
                        Played = false,
                        Position = 1
                    }
                }
            };

            _testData = new TigerFileData
            {
                Total = new Total
                {
                    FirstPlace = tigerPlayerOne,
                    UnOfficialPlaces = new List<WonPlace>
                    {
                        new WonPlace
                        {
                            Player = tigerPlayerTwo,
                            Position = 2,
                            Title = "Looser"
                        }
                    }
                },
                Players = new TigerPlayer[2]
                {
                tigerPlayerOne,
                tigerPlayerTwo
                }
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _iWriter = null;
            _tWriter = null;

            if (File.Exists(_filePath))
                File.Delete(_filePath);

            if (Directory.Exists(_filePath.Split('\\')[0]))
                Directory.Delete(_filePath.Split('\\')[0]);
        }

        [TestMethod]
        public void TestFileName()
        {
            _iWriter.WriterDetails = 1;
            Assert.AreEqual(_tWriter.WriterDetails, 1);
            Assert.IsNull(_tWriter.FileName);

            _iWriter.WriterDetails = _filePath;
            Assert.AreEqual(_tWriter.WriterDetails, _filePath);
            Assert.AreEqual(_tWriter.FileName, _filePath);

            _iWriter.WriterDetails = 1;
            Assert.AreEqual(_tWriter.WriterDetails, 1);
            Assert.AreEqual(_tWriter.FileName, _filePath);


            _iWriter.WriterDetails = "test";
            Assert.AreEqual(_tWriter.WriterDetails, "test");
            Assert.AreEqual(_tWriter.FileName, "test");
        }

        [TestMethod]
        public void TestTargetDirectory()
        {
            _iWriter.TargetDirectory = _filePath;
            Assert.AreEqual(_tWriter.TargetDirectory, _filePath);

            _iWriter.TargetDirectory = null;
            Assert.IsNull(_tWriter.TargetDirectory);
        }

        [TestMethod]
        public void TestFileCreation()
        {
            _iWriter.TargetDirectory = _filePath.Split('\\')[0];
            _iWriter.WriterDetails = _filePath.Split('\\')[1];
            var response = _iWriter.WriteData(_testData);

            Assert.IsTrue(response.Success);
            Assert.IsTrue(File.Exists(_filePath));
        }

        [TestMethod]
        public void TestFileContent()
        {
            _iWriter.TargetDirectory = _filePath.Split('\\')[0];
            _iWriter.WriterDetails = _filePath.Split('\\')[1];
            var response = _iWriter.WriteData(_testData);

            using (var reader = new StreamReader(_filePath))
            {
                var jObject = JObject.Parse(reader.ReadToEnd());
                Assert.IsNotNull(jObject);

                var model = jObject.ToObject<TigerFileData>();
                Assert.IsNotNull(model);

                Assert.AreEqual(model.Total.FirstPlace.FullScore, _testData.Total.FirstPlace.FullScore);
                Assert.AreEqual(model.Players.Length, _testData.Players.Length);
            }
        }
    }
}
