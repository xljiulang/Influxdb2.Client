using Influxdb2.Client.Datas;
using System;
using System.Diagnostics;
using System.Text;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示实体数据点
    /// </summary> 
    [DebuggerDisplay("Measurement = {desciptor.Measurement}")]
    public class EntityPointData : IPointData
    {
        /// <summary>
        /// 数据点描述
        /// </summary>
        private readonly PointDataDesciptor desciptor;

        /// <summary>
        /// 获取数据实体
        /// </summary>
        public object Entity { get; }

        /// <summary>
        /// 实体数据点
        /// </summary>
        /// <param name="entity">ColumnTypeAttribute标记的实体</param>
        /// <exception cref="ArgumentException"></exception>
        public EntityPointData(object entity)
        {
            this.desciptor = PointDataDesciptor.Get(entity.GetType());
            this.Entity = entity;
        }

        /// <summary>
        /// 转换为LineProtocol
        /// 因为c#语法关系，所有字属性名不用编码
        /// </summary>
        /// <returns></returns>
        public string ToLineProtocol()
        {
            var builder = new StringBuilder(desciptor.Measurement);
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
                throw new ArgumentException($"{this.Entity.GetType()}至少有一个{nameof(ColumnType.Field)}标记的属性值不为null");
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
