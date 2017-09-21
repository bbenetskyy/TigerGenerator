using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Logic.DocumentReaders
{
    public interface IDataReader
    {

        Response ReadData();
    }
}
