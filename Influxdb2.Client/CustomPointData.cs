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
    /// 表示自定义数据点
    /// </summary>
    [DebuggerDisplay("MeasurementName = {MeasurementName}")]
    public class CustomPointData : IPointData
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ColumnValueCollection tags = new ColumnValueCollection();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ColumnValueCollection fields = new ColumnValueCollection();

        /// <summary>
        /// 获取umeasurement名称
        /// </summary>
        public string MeasurementName { get; }

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
        /// 自定义数据点
        /// </summary>
        /// <param name="measurementName">measurement名称</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CustomPointData(string measurementName)
        {
            var name = LineProtocolUtil.EncodeMeasurement(measurementName);
            this.MeasurementName = name ?? throw new ArgumentNullException(nameof(measurementName));
        }

        /// <summary>
        /// 添加一个标签
        /// </summary>
        /// <param name="name">标签名</param>
        /// <param name="value">标签值，所有类型都当作文本处理</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public CustomPointData SetTag(string name, object value)
        {
            var tagName = LineProtocolUtil.EncodeTagName(name);
            if (string.IsNullOrEmpty(tagName))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var tagValue = LineProtocolUtil.EncodeTagValue(value);
            if (string.IsNullOrEmpty(tagValue))
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.tags.Add(tagName, tagValue);
            return this;
        }

        /// <summary>
        /// 添加一个字段
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="value">字段值</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public CustomPointData SetField(string name, object value)
        {
            var fileName = LineProtocolUtil.EncodeFielName(name);
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

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
        public CustomPointData SetTimestamp(long? value)
        {
            this.Timestamp = value;
            return this;
        }

        /// <summary>
        /// 设置unix纳秒时间戳
        /// </summary>
        /// <param name="value">unix纳秒时间戳</param>
        /// <returns></returns>
        public CustomPointData SetTimestamp(DateTimeOffset? value)
        {
            this.Timestamp = LineProtocolUtil.GetNsTimestamp(value);
            return this;
        }

        /// <summary>
        /// 设置unix纳秒时间戳
        /// </summary>
        /// <param name="timestamp">unix纳秒时间戳</param>
        /// <returns></returns>
        public CustomPointData SetTimestamp(DateTime? value)
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

            var builder = new StringBuilder(this.MeasurementName);
            foreach (var item in this.Tags.OrderBy(item => item.Column))
            {
                builder.Append(',').Append(item.Column).Append('=').Append(item.Value);
            }

            var index = 0;
            foreach (var item in this.Fields.OrderBy(item => item.Column))
            {
                var divider = index == 0 ? ' ' : ',';
                builder.Append(divider).Append(item.Column).Append('=').Append(item.Value);
                index += 1;
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
        /// 指定Measurement
        /// </summary>
        /// <param name="measurementName">Measurement名</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static CustomPointData Measurement(string measurementName)
        {
            return new CustomPointData(measurementName);
        }


        /// <summary>
        /// ColumnValue集合
        /// </summary>
        private class ColumnValueCollection : ICollection<ColumnValue>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly List<ColumnValue> columnValues = new List<ColumnValue>();

            int ICollection<ColumnValue>.Count => this.columnValues.Count;

            bool ICollection<ColumnValue>.IsReadOnly => true;

            /// <summary>
            /// 添加列与值
            /// </summary>
            /// <param name="column"></param>
            /// <param name="value"></param>
            /// <exception cref="ArgumentException"></exception>
            public void Add(string column, string value)
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
