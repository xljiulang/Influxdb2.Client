using Influxdb2.Client.Implementations;
using System;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示一个数据点
    /// </summary>
    public class Point : IPoint
    {
        private readonly object entity;
        private readonly EntityDesciptor desciptor;

        /// <summary>
        /// 实体数据点
        /// </summary>
        /// <param name="entity">由ColumnTypeAttribute标记属性的实体</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ProtocolException"></exception>
        public Point(object entity)
        {
            this.entity = entity;
            this.desciptor = EntityDesciptor.Get(entity.GetType());
        }

        /// <summary>
        /// 写入行文本协议内容
        /// </summary>
        /// <param name="writer">写入器 </param>
        public void WriteLineProtocol(ILineProtocolWriter writer)
        {
            writer.Write(desciptor.Utf8Measurement);
            foreach (var tag in desciptor.Tags)
            {
                var value = tag.GetStringValue(entity);
                writer.WriteComma().Write(tag.Utf8Name).WriteEqual().Write(value);
            }

            var firstField = true;
            foreach (var field in desciptor.Fields)
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

                var value = field.GetStringValue(entity);
                writer.Write(field.Utf8Name).WriteEqual().Write(value);
            }

            if (desciptor.Timestamp != null)
            {
                var timestamp = desciptor.Timestamp.GetStringValue(entity);
                if (timestamp != null)
                {
                    writer.WriteSpace().Write(timestamp);
                }
            }
        }
    }
}
