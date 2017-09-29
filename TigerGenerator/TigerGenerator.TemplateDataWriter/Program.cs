using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigerGenerator.TemplateDataWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            var writeFile = @"..\";
            var fileTemplate = Path.Combine(Directory.GetCurrentDirectory(), @"Templates\Template{0}.docx");
            var ids = new[] {4, 8, 16};

            //var content = new StringBuilder();
            //using (var reader = new StreamReader(path))
            //{
            //    do
            //    {
            //        string line = reader.ReadLine();
            //        if (!line.Contains(BytesVariable))
            //            content.AppendLine(line);
            //        else
            //            content.AppendLine(string.Format(bytesLineTemplate, winMergeBytes));
            //    } while (!reader.EndOfStream);
            //}

            //File.WriteAllText(path, content.ToString());
            //foreach (var id in ids)
            //{
            //    var fileBytes  = File.ReadAllBytes(string.Format(fileTemplate, id));
            //    var byteString = ConvertToString(fileBytes);

            //}

        }

        private static string ConvertToString(byte[] fileBytes)
        {
            var byteString = string.Empty;

            foreach (var b in fileBytes)
                byteString += b + TemplateWriter.SplitterBytes;

            return byteString.TrimEnd(TemplateWriter.SplitterBytes);
        }
    }
}
