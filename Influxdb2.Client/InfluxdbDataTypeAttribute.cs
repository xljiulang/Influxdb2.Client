using System;

namespace Influxdb2.Client
{
    /// <summary>
    /// 指示属性对应的Influxdb数据类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InfluxdbDataTypeAttribute : Attribute
    {
        /// <summary>
        /// 获取指示的数据类型
        /// </summary>
        public DataType DataType { get; }

        /// <summary>
        /// 指示属性对应的Influxdb数据类型
        /// </summary>
        /// <param name="dataType">Influxdb数据类型</param>
        public InfluxdbDataTypeAttribute(DataType dataType)
        {
            this.DataType = dataType;
        }
    }
}
