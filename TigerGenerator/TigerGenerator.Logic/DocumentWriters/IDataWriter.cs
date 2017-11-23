using System.Collections.Generic;
using TigerGenerator.Logic.Cluster;
using TigerGenerator.Logic.Models;

namespace TigerGenerator.Logic.DocumentWriters
{
    public interface IDataWriter
    {
        string TargetDirectory { get; set; }
        object WriterDetails { get; set; }
        Response WriteData(List<SimpleCluster> clusters);
        Response WriteData(string text);
    }
}
