using Influxdb2.Client;
using System;

namespace Influxdb2
{
    public class Temperature
    {
        public string Location { get; set; }

        public double Value { get; set; }

        // public DateTimeOffset _time { get; set; }
        [ColumnName(Column.Time)]    
        public DateTimeOffset Time { get; set; }
    }
}
