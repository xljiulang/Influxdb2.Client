using Influxdb2.Client.Datas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示动态定义的数据点
    /// </summary>
    [DebuggerDisplay("Measurement = {Measurement}")]
    public class DynamicPointData : IPointData
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ColumnValueCollection tags = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ColumnValueCollection fields = new();

        /// <summary>
        /// 获取umeasurement
        /// </summary>
        public string Measurement { get; }

        /// <summary>
        /// 获取unix纳秒时间戳
        /// </summary>
        public long? Timestamp { get; private set; }

        /// <summary>
        /// 获取标签集合
        /// </summary>
        public ICollection<ColumnValue> Tags => this.tags;

        /// <summary>
        /// 获取字段集合
        /// </summary>
        public ICollection<ColumnValue> Fields => this.fields;

        /// <summary>
        /// 动态定义的数据点
        /// </summary>
        /// <param name="measurement">measurement</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DynamicPointData(string measurement)
        {
            this.Measurement = LineProtocolUtil.Encode(measurement);
        }

        /// <summary>
        /// 添加一个标签
        /// </summary>
        /// <param name="name">标签名</param>
        /// <param name="value">标签值，所有类型都当作文本处理</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public DynamicPointData SetTag(string name, string value)
        {
            name = LineProtocolUtil.Encode(name);
            value = LineProtocolUtil.Encode(value);
            this.tags.Add(name, value);
            return this;
        }

        /// <summary>
        /// 添加文本字段
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="value">文本值</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public DynamicPointData SetField(string name, string? value)
        {
            name = LineProtocolUtil.Encode(name);
            value = LineProtocolUtil.EncodeFieldValue(value);
            this.fields.Add(name, value);
            return this;
        }

        /// <summary>
        /// 添加非文本字段
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="value">非文本值</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public DynamicPointData SetField(string name, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var fileName = LineProtocolUtil.Encode(name);
            var fieldValue = LineProtocolUtil.CreateFieldValueEncoder(value.GetType())(value);
            if (fieldValue == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.fields.Add(fileName, fieldValue);
            return this;
        }

        /// <summary>
        /// 设置unix纳秒时间戳
        /// </summary>
        /// <param name="value">unix纳秒时间戳</param>
        /// <returns></returns>
        public DynamicPointData SetTimestamp(long value)
        {
            this.Timestamp = value;
            return this;
        }

        /// <summary>
        /// 设置unix纳秒时间戳
        /// </summary>
        /// <param name="value">unix纳秒时间戳</param>
        /// <returns></returns>
        public DynamicPointData SetTimestamp(DateTimeOffset value)
        {
            this.Timestamp = LineProtocolUtil.GetNsTimestamp(value);
            return this;
        }

        /// <summary>
        /// 设置unix纳秒时间戳
        /// </summary>
        /// <param name="timestamp">unix纳秒时间戳</param>
        /// <returns></returns>
        public DynamicPointData SetTimestamp(DateTime value)
        {
            this.Timestamp = LineProtocolUtil.GetNsTimestamp(value);
            return this;
        }

        /// <summary>
        /// 转换为LineProtocol
        /// </summary>
        /// <returns></returns>
        public string ToLineProtocol()
        {
            if (this.Fields.Count == 0)
            {
                throw new ArgumentException($"至少设置一个Field值");
            }

            var builder = new StringBuilder(this.Measurement);
            foreach (var item in this.Tags.OrderBy(item => item.Column))
            {
                builder.Append(',').Append(item.Column).Append('=').Append(item.Value);
            }

            var firstField = true;
            foreach (var item in this.Fields.OrderBy(item => item.Column))
            {
                var divider = firstField ? ' ' : ',';
                builder.Append(divider).Append(item.Column).Append('=').Append(item.Value);
                firstField = false;
            }

            if (this.Timestamp != null)
            {
                builder.Append(' ').Append(this.Timestamp.ToString());
            }

            return builder.ToString();
        }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToLineProtocol();
        }

        /// <summary>
        /// 创建一个CustomPointData
        /// </summary>
        /// <param name="measurement">measurement</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static DynamicPointData Create(string measurement)
        {
            return new DynamicPointData(measurement);
        }


        /// <summary>
        /// ColumnValue集合
        /// </summary>
        private class ColumnValueCollection : ICollection<ColumnValue>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly List<ColumnValue> columnValues = new();

            int ICollection<ColumnValue>.Count => this.columnValues.Count;

            bool ICollection<ColumnValue>.IsReadOnly => true;

            /// <summary>
            /// 添加列与值
            /// </summary>
            /// <param name="column"></param>
            /// <param name="value"></param>
            /// <exception cref="ArgumentException"></exception>
            public void Add(string column, string? value)
            {
                if (this.columnValues.Any(item => item.Column == column))
                {
                    throw new ArgumentException($"不允许添加重复的列: {column}", nameof(column));
                }
                this.columnValues.Add(new ColumnValue(column, value));
            }

            void ICollection<ColumnValue>.Add(ColumnValue item)
            {
                throw new InvalidOperationException();
            }

            void ICollection<ColumnValue>.Clear()
            {
                throw new InvalidOperationException();
            }

            bool ICollection<ColumnValue>.Contains(ColumnValue item)
            {
                return this.columnValues.Contains(item);
            }

            void ICollection<ColumnValue>.CopyTo(ColumnValue[] array, int arrayIndex)
            {
                this.columnValues.CopyTo(array, arrayIndex);
            }

            IEnumerator<ColumnValue> IEnumerable<ColumnValue>.GetEnumerator()
            {
                return this.columnValues.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.columnValues.GetEnumerator();
            }

            bool ICollection<ColumnValue>.Remove(ColumnValue item)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
