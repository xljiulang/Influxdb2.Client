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
        /// 因为c#语法关系，所有字属性名不用编码
        /// </summary>
        /// <returns></returns>
        public string ToLineProtocol()
        {
            var desciptor = PointDataDesciptor.Get(this.Entity.GetType());
            var builder = new StringBuilder(desciptor.MeasurementName);

            foreach (var tag in desciptor.Tags)
            {
                var value = tag.GetStringValue(this.Entity);
                if (value != null)
                {
                    builder.Append(',').Append(tag.Name).Append('=').Append(value);
                }
            }

            var fieldWrited = false;
            foreach (var field in desciptor.Fields)
            {
                var value = field.GetStringValue(this.Entity);
                if (value != null)
                {
                    var divider = ',';
                    if (fieldWrited == false)
                    {
                        divider = ' ';
                        fieldWrited = true;
                    }
                    builder.Append(divider).Append(field.Name).Append('=').Append(value);
                }
            }

            if (fieldWrited == false)
            {
                throw new ArgumentException($"{desciptor.MeasurementName}至少有一个{nameof(ColumnType.Field)}列不为null");
            }

            if (desciptor.Timestamp != null)
            {
                var timestamp = desciptor.Timestamp.GetStringValue(this.Entity);
                if (timestamp != null)
                {
                    builder.Append(' ').Append(timestamp);
                }
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
    }
}
