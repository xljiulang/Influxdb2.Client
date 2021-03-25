using Influxdb2.Client;
using System;

namespace Influxdb2
{

    class M3
    {
        [InfluxdbDataType(DataType.Tag)]
        public string CoId { get; set; }


        [InfluxdbDataType(DataType.Tag)]
        public string LabelId { get; set; }


        [InfluxdbDataType(DataType.Field)]
        public string Name { get; set; }


        [InfluxdbDataType(DataType.Field)]
        public int? Age { get; set; }


        [InfluxdbDataType(DataType.Time)]
        public DateTimeOffset CreateTime { get; set; }
    }
}
