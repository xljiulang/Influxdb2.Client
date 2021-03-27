using Influxdb2.Client;
using System;

namespace Influxdb2
{

    class M31
    {
        [ColumnType(ColumnType.Tag)]
        public string CoId { get; set; }


        [ColumnType(ColumnType.Tag)]
        public string LabelId { get; set; }


        [ColumnType(ColumnType.Field)]
        public string Name { get; set; }


        [ColumnType(ColumnType.Field)]
        public int? Age { get; set; }

        [ColumnType(ColumnType.Field)]
        public bool IsOk { get; set; }


        [ColumnType(ColumnType.Timestamp)]
        public DateTimeOffset CreateTime { get; set; }
    }
}
