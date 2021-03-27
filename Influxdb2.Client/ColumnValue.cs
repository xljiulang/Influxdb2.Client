using System;
using System.Diagnostics;

namespace Influxdb2.Client
{
    /// <summary>
    /// 列与值
    /// </summary>
    [DebuggerDisplay("{Column} = {Value}")]
    public struct ColumnValue : IEquatable<ColumnValue>
    {
        /// <summary>
        /// 获取或设置列名
        /// </summary>
        public string Column { get; }

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public string? Value { get; }

        /// <summary>
        /// 列与值
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public ColumnValue(string column, string? value)
        {
            this.Column = column;
            this.Value = value;
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ColumnValue other)
        {
            return this.Column == other.Column && this.Value == other.Value;
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is ColumnValue other && this.Equals(other);
        }

        /// <summary>
        /// 获取哈希
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Column, this.Value);
        }
    }
}
