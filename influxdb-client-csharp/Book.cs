using InfluxDB.Client.Core;
using System;

namespace influxdb_client_csharp
{
    [Measurement("Book")]
    class Book
    {
        [Column("Serie", IsTag = true)]
        public string Serie { get; set; }


        [Column("Name")]
        public string Name { get; set; }


        [Column("Price")]
        public double? Price { get; set; }


        [Column("SpecialOffer")]
        public bool? SpecialOffer { get; set; }


        [Column("Time", IsTimestamp = true)]
        public DateTime Time { get; set; }
    }
}
