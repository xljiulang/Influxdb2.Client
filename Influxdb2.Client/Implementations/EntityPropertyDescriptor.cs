using System;
using System.Reflection;
using System.Text;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// 实体的属性描述
    /// </summary>
    sealed class EntityPropertyDescriptor : PropertyDescriptor<object, object?>
    {
        /// <summary>
        /// 获取列类型
        /// </summary>
        public ColumnType ColumnType { get; }

        /// <summary>
        /// 获取utf8名称
        /// </summary>
        public byte[] Utf8Name { get; }

        /// <summary>
        /// 值转换器
        /// </summary>
        private readonly Func<object?, string?> valueConverter;

        /// <summary>
        /// 实体的属性描述
        /// </summary>
        /// <param name="property">属性信息</param>
        public EntityPropertyDescriptor(PropertyInfo property)
            : base(property)
        {
            var typeAttr = property.GetCustomAttribute<ColumnTypeAttribute>();
            if (typeAttr != null)
            {
                this.ColumnType = typeAttr.ColumnType;
            }

            var nameAttr = property.GetCustomAttribute<ColumnNameAttribute>();
            if (nameAttr != null)
            {
                this.Name = LineProtocolUtil.Encode(nameAttr.Name);
            }
            this.Utf8Name = Encoding.UTF8.GetBytes(this.Name);

            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (this.ColumnType == ColumnType.Timestamp)
            {
                this.valueConverter = CreateTimestampConverter(type);
            }
            else if (this.ColumnType == ColumnType.Field)
            {
                this.valueConverter = LineProtocolUtil.CreateFieldValueConverter(type);
            }
            else // 标签
            {
                this.valueConverter = value => LineProtocolUtil.Encode(value?.ToString());
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="instance">实例</param>
        /// <returns></returns>
        public string? GetStringValue(object instance)
        {
            var value = base.GetValue(instance);
            return this.valueConverter.Invoke(value);
        }

        /// <summary>
        /// 获取时间戳转换器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Func<object?, string?> CreateTimestampConverter(Type type)
        {
            if (type == typeof(DateTime))
            {
                return value => LineProtocolUtil.GetNsTimestamp((DateTime?)value).ToString();
            }

            if (type == typeof(DateTimeOffset))
            {
                return value => LineProtocolUtil.GetNsTimestamp((DateTimeOffset?)value)?.ToString();
            }

            throw new NotSupportedException($"属性{type} {this.Name}不支持转换为Timestamp");
        }
    }
}
