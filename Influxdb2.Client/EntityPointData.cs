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
        /// 实体描述
        /// </summary>
        private readonly EntityDesciptor desciptor;

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
            this.desciptor = EntityDesciptor.Get(entity.GetType());
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
                builder.Append(',').Append(tag.Name).Append('=').Append(value);
            }

            var firstField = true;
            foreach (var field in desciptor.Fields)
            {
                var value = field.GetStringValue(this.Entity);
                var divider = firstField ? ' ' : ',';
                builder.Append(divider).Append(field.Name).Append('=').Append(value);
                firstField = false;
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
