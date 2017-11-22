using System;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Logic.DocumentReaders
{
    public interface IDataReader : IDisposable
    {
        object ReaderDetails { get; set; }
        Response ReadData();
    }
}
