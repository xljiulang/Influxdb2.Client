using System;
using System.Reflection;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示属性描述
    /// </summary>
    class PropertyDescriptor : Property<object, object>
    {
        public ColumnType ColumnType { get; }

        private Func<object, string?> valueConvert;

        /// <summary>
        /// 属性描述
        /// </summary>
        /// <param name="property">属性信息</param>
        public PropertyDescriptor(PropertyInfo property)
            : base(property)
        {
            var attr = property.GetCustomAttribute<ColumnAttribute>();
            if (attr != null)
            {
                this.ColumnType = attr.Type;
                if (string.IsNullOrEmpty(attr.Name) == false)
                {
                    this.Name = attr.Name;
                }
            }

            var type = property.PropertyType;
            if (this.ColumnType == ColumnType.Time)
            {
                this.valueConvert = GetTimeConverter(type);
            }
            else if (this.ColumnType == ColumnType.Field)
            {

            }
            else
            {
                this.valueConvert = GetTagConverter(type);
            }
        }

        public string? GetStringValue(object instance)
        {
            var value = this.GetValue(instance);
            return this.valueConvert(value);
        }

        private static Func<object, string?> GetTagConverter(Type type)
        {
            return (object value) => $@"""{value}""";
        }


        private static Func<object, string?> GetFieldConverter(Type type)
        {
            if( type== typeof(int ))
        }


        private static Func<object, string?> GetTimeConverter(Type type)
        {
            if (type == typeof(DateTime?))
            {
                return (object value) =>
                {
                    var val = (DateTime?)value;
                    return val == null ? null : new DateTimeOffset(val.Value).ToUnixTimeMilliseconds().ToString();
                };
            }

            if (type == typeof(DateTimeOffset?))
            {
                return (object value) =>
                {
                    var val = (DateTimeOffset?)value;
                    return val?.ToUnixTimeMilliseconds().ToString();
                };
            }

            if (type == typeof(DateTime))
            {
                return (object value) => new DateTimeOffset((DateTime)value).ToUnixTimeMilliseconds().ToString();
            }

            if (type == typeof(DateTimeOffset))
            {
                return (object value) => ((DateTimeOffset)value).ToUnixTimeMilliseconds().ToString();
            }

            throw new NotSupportedException($"不支持时间类型{type}");
        }

    }

}
