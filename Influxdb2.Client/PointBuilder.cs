using Influxdb2.Client.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示数据点的创建器 
    /// </summary> 
    public class PointBuilder
    {
        private long? timestamp;
        private readonly string measurement;
        private readonly ColumnValueCollection tags = new();
        private readonly ColumnValueCollection fields = new();

        /// <summary>
        /// 数据点的创建器 
        /// </summary>
        /// <param name="measurement">measurement</param>
        /// <exception cref="ProtocolException"></exception>
        public PointBuilder(string measurement)
        {
            this.measurement = LineProtocolUtil.Encode(measurement, escapeEqual: false);
        }

        /// <summary>
        /// 添加一个标签
        /// </summary>
        /// <param name="name">标签名</param>
        /// <param name="value">标签值，所有类型都当作文本处理</param>
        /// <exception cref="ProtocolException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public PointBuilder SetTag(string name, string value)
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
        /// <exception cref="ProtocolException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public PointBuilder SetField(string name, string? value)
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
        /// <exception cref="ProtocolException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public PointBuilder SetField(string name, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var fileName = LineProtocolUtil.Encode(name);
            var fieldValue = LineProtocolUtil.CreateFieldValueConverter(value.GetType())(value);
            this.fields.Add(fileName, fieldValue);
            return this;
        }

        /// <summary>
        /// 设置unix纳秒时间戳
        /// </summary>
        /// <param name="value">unix纳秒时间戳</param>
        /// <returns></returns>
        public PointBuilder SetTimestamp(long? value)
        {
            this.timestamp = value;
            return this;
        }

        /// <summary>
        /// 设置unix纳秒时间戳
        /// </summary>
        /// <param name="value">unix纳秒时间戳</param>
        /// <returns></returns>
        public PointBuilder SetTimestamp(DateTimeOffset? value)
        {
            this.timestamp = LineProtocolUtil.GetNsTimestamp(value);
            return this;
        }

        /// <summary>
        /// 设置unix纳秒时间戳
        /// </summary>
        /// <param name="value">unix纳秒时间戳</param>
        /// <returns></returns>
        public PointBuilder SetTimestamp(DateTime? value)
        {
            this.timestamp = LineProtocolUtil.GetNsTimestamp(value);
            return this;
        }

        /// <summary>
        /// 创建一个Point
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public IPoint Build()
        {
            return new BuilderPoint(this);
        }

        /// <summary>
        /// Builder对应的数据点
        /// </summary>
        private class BuilderPoint : IPoint
        {
            private readonly PointBuilder builder;

            public BuilderPoint(PointBuilder builder)
            {
                this.builder = builder;
            }

            /// <summary>
            /// 写入行文本协议内容
            /// </summary>
            /// <exception cref="InvalidOperationException"></exception> 
            /// <param name="writer">写入器 </param>
            public void WriteTo(ILineProtocolWriter writer)
            {
                if (this.builder.fields.Count == 0)
                {
                    throw new InvalidOperationException($"至少设置一个Field值");
                }

                writer.Write(this.builder.measurement);
                foreach (var item in this.builder.tags.OrderBy(item => item.Column))
                {
                    writer.WriteComma().Write(item.Column).WriteEqual().Write(item.Value);
                }

                var firstField = true;
                foreach (var item in this.builder.fields.OrderBy(item => item.Column))
                {
                    if (firstField == true)
                    {
                        firstField = false;
                        writer.WriteSpace();
                    }
                    else
                    {
                        writer.WriteComma();
                    }
                    writer.Write(item.Column).WriteEqual().Write(item.Value);
                }

                if (this.builder.timestamp != null)
                {
                    writer.WriteSpace().Write(this.builder.timestamp.ToString());
                }
            }
        }

        /// <summary>
        /// ColumnValue集合
        /// </summary>
        private class ColumnValueCollection : IEnumerable<ColumnValue>
        {
            private readonly HashSet<string> columns = new();
            private readonly List<ColumnValue> columnValues = new();

            public int Count => this.columnValues.Count;

            /// <summary>
            /// 添加列与值
            /// </summary>
            /// <param name="column"></param>
            /// <param name="value"></param>
            /// <exception cref="ArgumentException"></exception>
            public void Add(string column, string? value)
            {
                if (this.columns.Add(column) == false)
                {
                    throw new ArgumentException($"不允许添加重复的列: {column}", nameof(column));
                }
                this.columnValues.Add(new ColumnValue(column, value));
            }

            IEnumerator<ColumnValue> IEnumerable<ColumnValue>.GetEnumerator()
            {
                return this.columnValues.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.columnValues.GetEnumerator();
            }
        }
    }

}
