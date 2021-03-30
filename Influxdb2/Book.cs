using Influxdb2.Client;
using System;

namespace Influxdb2
{
    class Book
    {
        [ColumnType(ColumnType.Tag)]
        public string Serie { get; set; }


        [ColumnType(ColumnType.Field)]
        public string Name { get; set; }


        [ColumnType(ColumnType.Field)]
        public decimal? Price { get; set; }


        [ColumnType(ColumnType.Field)]
        public bool? SpecialOffer { get; set; }


        [ColumnType(ColumnType.Timestamp)]
        public DateTimeOffset? CreateTime { get; set; }
    }
}
