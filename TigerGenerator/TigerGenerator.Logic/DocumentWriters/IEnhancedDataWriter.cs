using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Logic.DocumentWriters
{
    public interface IEnhancedDataWriter<T>
    {
        string TargetDirectory { get; set; }
        object WriterDetails { get; set; }
        Response<T> WriteData(T data);
    }
}
