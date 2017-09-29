using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable UseObjectOrCollectionInitializer

namespace TigerGenerator.TemplateDataWriter
{
    public static class TemplateWriter
    {
        public const char SplitterBytes = ',';

        public static void CreateTemplates()
        {
            // Key = id like 4,8; Value - file bytes from file like Template4, Template16;
            var fileDictionary = new Dictionary<string,string>();
            fileDictionary.Add("1","1,10,11,001");
            foreach (var filePair in fileDictionary)
            {
                var fileBytes = GetTemplateBytes(filePair.Value);
                File.WriteAllBytes(
                    Path.Combine(Directory.GetCurrentDirectory(),
                        $@"Templates\Template{filePair.Key}.docx"), fileBytes);
            }
        }

        private static byte[] GetTemplateBytes(string bytes) =>
            Array.ConvertAll(bytes.Split(SplitterBytes), byte.Parse);
    }
}
