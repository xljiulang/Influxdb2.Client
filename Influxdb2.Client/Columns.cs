using System.Collections.Generic;
using System.Linq;

namespace Influxdb2.Client
{
    public class Columns
    {
        private readonly List<string> columnList = new List<string>();

        public static Columns Empty => new Columns();

        public static Columns Values(params string[] columns) => Empty.Add(columns);

        public Columns Add(params string[] columns)
        {
            foreach (var item in columns)
            {
                this.columnList.Add(item);
            }
            return this;
        }

        public override string ToString()
        {
            return $"[{string.Join(",", columnList.Select(c => @$"""{c}"""))}]";
        }
    }
}
