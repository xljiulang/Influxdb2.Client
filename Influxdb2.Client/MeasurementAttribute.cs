using System;

namespace Influxdb2.Client
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MeasurementAttribute : Attribute
    {
        public string? Name { get; set; }

        public MeasurementAttribute()
        {
        }

        public MeasurementAttribute(string name)
        {
            this.Name = name;
        }
    }
}
