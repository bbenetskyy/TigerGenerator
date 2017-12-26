using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Logic.DocumentReaders.Tiger
{
    public class TigerDataReader : IEnhancedDataReader<TigerFileData>
    {
        private object _readerDetails;

        public object ReaderDetails
        {
            get => _readerDetails;
            set
            {
                _readerDetails = value;
                if (_readerDetails is string fileName)
                    FileName = fileName;
            }
        }

        public string FileName { get; set; }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Response<TigerFileData> ReadData()
        {
            throw new NotImplementedException();
        }
    }
}
