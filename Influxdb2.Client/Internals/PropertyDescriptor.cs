using System;
using System.Reflection;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示属性描述
    /// </summary>
    sealed class PropertyDescriptor : Property<object, object>
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        public DataType DataType { get; }

        /// <summary>
        /// 值转换器
        /// </summary>
        private Func<object?, string?> valueConverter;

        /// <summary>
        /// 属性描述
        /// </summary>
        /// <param name="property">属性信息</param>
        public PropertyDescriptor(PropertyInfo property)
            : base(property)
        {
            var attr = property.GetCustomAttribute<InfluxdbDataTypeAttribute>();
            if (attr != null)
            {
                this.DataType = attr.DataType;
            }

            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (this.DataType == DataType.Time)
            {
                this.valueConverter = this.GetTimeConverter(type);
            }
            else if (this.DataType == DataType.Field)
            {
                this.valueConverter = this.GetFieldConverter(type);
            }
            else
            {
                this.valueConverter = value => value?.ToString();
            }
        }

        /// <summary>
        /// 获取值的文本表示
        /// </summary>
        /// <param name="instance">实例</param>
        /// <returns></returns>
        public string? GetValueString(object instance)
        {
            var value = this.GetValue(instance);
            return this.valueConverter.Invoke(value);
        }

        /// <summary>
        /// Field转换器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Func<object?, string?> GetFieldConverter(Type type)
        {
            if (type == typeof(sbyte) ||
                type == typeof(byte) ||
                type == typeof(short) ||
                type == typeof(int) ||
                type == typeof(long) ||
                typeof(Enum).IsAssignableFrom(type))
            {
                return value => value == null ? null : $"{value}i";
            }

            if (type == typeof(ushort) ||
                type == typeof(uint) ||
                type == typeof(ulong))
            {
                return value => value == null ? null : $"{value}u";
            }

            if (type == typeof(decimal) ||
                type == typeof(float) ||
                type == typeof(double))
            {
                return value => value?.ToString();
            }

            return value => value is null ? null : $@"""{value}""";
        }

        /// <summary>
        /// 时间转换器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Func<object?, string?> GetTimeConverter(Type type)
        {
            if (type == typeof(DateTime))
            {
                return value => GetNsTimestamp((DateTime?)value);
            }

            if (type == typeof(DateTimeOffset))
            {
                return value => GetNsTimestamp((DateTimeOffset?)value);
            }

            throw new InfluxdbFieldException($"不支持转换为时间类型：{type}", this.Name);
        }

        private static string? GetNsTimestamp(DateTime? value)
        {
            return value == null ? null : GetNsTimestamp(new DateTimeOffset(value.Value));
        }

        private static string? GetNsTimestamp(DateTimeOffset? value)
        {
            return value == null ? null : (value.Value.Subtract(DateTimeOffset.UnixEpoch).Ticks * 100).ToString();
        }
    }
}
