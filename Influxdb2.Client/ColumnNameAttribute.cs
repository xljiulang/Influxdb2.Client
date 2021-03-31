using Influxdb2.Client.Core;
using System;

namespace Influxdb2.Client
{
    /// <summary>
    /// Influxdb列名特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// 获取列名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 表示列名
        /// </summary>
        /// <param name="name">列名</param>
        /// <exception cref="ProtocolException"></exception>
        public ColumnNameAttribute(string name)
        {
            this.Name = LineProtocolUtil.Encode(name);
        }
    }
}
