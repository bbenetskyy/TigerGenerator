using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
                    FullPath = fileName;
            }
        }

        public string FullPath { get; set; }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Response<TigerFileData> ReadData()
        {
            var response = new Response<TigerFileData>();

            try
            {
                var jObject = JObject.Parse(File.ReadAllText(FullPath));
                response.ReturnValue = jObject.ToObject<TigerFileData>();
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex);
            }

            return response;
        }
    }
}
