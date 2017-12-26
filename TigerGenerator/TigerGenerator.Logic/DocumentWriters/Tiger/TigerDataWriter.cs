using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigerGenerator.Logic.Cluster;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Logic.DocumentWriters.Tiger
{
    public class TigerDataWriter : IEnhancedDataWriter<TigerFileData>
    {
        public string TargetDirectory { get; set; }

        private object _writerDetails;
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

        public Response<TigerFileData> WriteData(TigerFileData data)
        {
            var response = new Response<TigerFileData>();
            try
            {
                CreateDirectory();
                var filePath = Path.Combine(TargetDirectory, FileName);
                File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex);
            }
            return response;
        }

        private void CreateDirectory()
        {
            if (!Directory.Exists(TargetDirectory))
                Directory.CreateDirectory(TargetDirectory);
        }
    }
}
