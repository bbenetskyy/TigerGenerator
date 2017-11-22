using System.Collections.Generic;
using System.Text;

namespace TigerGenerator.Logic.Helpers
{
    public class MultiLineStringBuilder
    {
        private readonly List<StringBuilder> builders = new List<StringBuilder>();

        public void AppendLine(string stringLine)
        {
            builders.Add(new StringBuilder(stringLine));
        }

        public StringBuilder this[int index] => builders[index];

        public int Count => builders.Count;

        public override string ToString()
        {
            var resBuilder = new StringBuilder();
            foreach (var currentBuilder in builders)
            {
                resBuilder.AppendLine(currentBuilder.ToString());
            }
            return resBuilder.ToString();
        }
    }
}
