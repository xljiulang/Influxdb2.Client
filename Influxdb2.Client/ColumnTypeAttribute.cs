using System;

namespace Influxdb2.Client
{
    /// <summary>
    /// 指示属性对应的Influxdb列类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnTypeAttribute : Attribute
    {
        /// <summary>
        /// 获取指示的列类型
        /// </summary>
        public ColumnType ColumnType { get; }

        /// <summary>
        /// 属性对应的Influxdb列类型
        /// </summary>
        /// <param name="columnType">列类型</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ColumnTypeAttribute(ColumnType columnType)
        {
            if (Enum.IsDefined(typeof(ColumnType), columnType) == false)
            {
                throw new ArgumentOutOfRangeException(nameof(columnType));
            }
            this.ColumnType = columnType;
        }
    }
}
