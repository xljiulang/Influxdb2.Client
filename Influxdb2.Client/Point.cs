using Influxdb2.Client.Datas;
using System;
using System.Text;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示一个数据点
    /// </summary>
    public class Point : IPoint
    {
        /// <summary>
        /// 获取行文本协议内容
        /// </summary>
        public string LineProtocol { get; }

        /// <summary>
        /// 数据点
        /// </summary>
        /// <param name="lineProtocol">数据点的行文本协议</param>
        public Point(string lineProtocol)
        {
            this.LineProtocol = lineProtocol;
        }

        /// <summary>
        /// 数据点
        /// </summary>
        /// <param name="lineProtocol">数据点的行文本协议</param>
        public Point(StringBuilder lineProtocol)
        {
            this.LineProtocol = lineProtocol.ToString();
        }

        /// <summary>
        /// 实体数据点
        /// </summary>
        /// <param name="entity">由ColumnTypeAttribute标记属性的实体</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ProtocolException"></exception>
        public Point(object entity)
        {
            var desciptor = EntityDesciptor.Get(entity.GetType());
            var builder = new StringBuilder(desciptor.Measurement);

            foreach (var tag in desciptor.Tags)
            {
                var value = tag.GetStringValue(entity);
                builder.Append(',').Append(tag.Name).Append('=').Append(value);
            }

            var firstField = true;
            foreach (var field in desciptor.Fields)
            {
                var value = field.GetStringValue(entity);
                var divider = firstField ? ' ' : ',';
                builder.Append(divider).Append(field.Name).Append('=').Append(value);
                firstField = false;
            }

            if (desciptor.Timestamp != null)
            {
                var timestamp = desciptor.Timestamp.GetStringValue(entity);
                if (timestamp != null)
                {
                    builder.Append(' ').Append(timestamp);
                }
            }

            this.LineProtocol = builder.ToString();
        }

        /// <summary>
        /// 转换为文本 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.LineProtocol;
        }
    }
}
