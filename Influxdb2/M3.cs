using Influxdb2.Client;
using System;

namespace Influxdb2
{

    class M3
    {
        [ColumnType(ColumnType.Tag)]
        public string CoId { get; set; }


        [ColumnType(ColumnType.Tag)]
        public string LabelId { get; set; }


        [ColumnType(ColumnType.Field)]
        public string Name { get; set; }


        [ColumnType(ColumnType.Field)]
        public int? Age { get; set; }


        [ColumnType(ColumnType.Time)]
        public DateTimeOffset CreateTime { get; set; }
    }
}
