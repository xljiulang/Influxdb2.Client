using Influxdb2.Client.Datas;
using System;
using System.Text;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示实体数据点
    /// </summary>
    public class EntityPointData : IPointData
    {
        /// <summary>
        /// 获取数据实体
        /// </summary>
        public object Entity { get; }

        /// <summary>
        /// 实体数据点
        /// </summary>
        /// <param name="entity">ColumnTypeAttribute标记的实体</param>
        public EntityPointData(object entity)
        {
            this.Entity = entity;
        }

        /// <summary>
        /// 转换为LineProtocol
        /// </summary>
        /// <returns></returns>
        public string ToLineProtocol()
        {
            var desciptor = PointDataDesciptor.Get(this.Entity.GetType());
            var builder = new StringBuilder().Append(desciptor.Measurement);

            foreach (var tag in desciptor.Tags)
            {
                var tagValue = tag.GetString(this.Entity);
                if (tagValue != null)
                {
                    var encodeValue = LineProtocolUtil.EncodeName(tagValue);
                    builder.Append(',').Append(tag.Name).Append('=').Append(encodeValue);
                }
            }

            var fieldWrited = false;
            foreach (var field in desciptor.Fields)
            {
                var fieldValue = field.GetString(this.Entity);
                if (fieldValue != null)
                {
                    var divider = ',';
                    if (fieldWrited == false)
                    {
                        divider = ' ';
                        fieldWrited = true;
                    }

                    var encodeValue = LineProtocolUtil.EncodeValue(fieldValue);
                    builder.Append(divider).Append(field.Name).Append('=').Append('"').Append(encodeValue).Append('"');
                }
            }

            if (fieldWrited == false)
            {
                throw new ArgumentException($"{desciptor.Measurement}至少有一个{nameof(ColumnType.Field)}列不为null");
            }

            if (desciptor.Time != null)
            {
                var timestamp = desciptor.Time.GetString(this.Entity);
                if (timestamp != null)
                {
                    builder.Append(' ').Append(timestamp);
                }
            }

            return builder.ToString();
        }
    }
}
