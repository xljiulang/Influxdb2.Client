using System;

namespace Influxdb2.Client
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class ColumnAttribute : Attribute
    {
        public ColumnType Type { get; }

        public string? Name { get; set; }

        public ColumnAttribute(ColumnType type)
        {
            this.Type = type;
        }
    }
}
