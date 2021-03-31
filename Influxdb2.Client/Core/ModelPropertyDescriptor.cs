using System;
using System.Reflection;

namespace Influxdb2.Client.Core
{
    /// <summary>
    /// 模型属性描述器
    /// </summary>
    sealed class ModelPropertyDescriptor<T> : PropertyDescriptor<T, object?>
    {
        /// <summary>
        /// 值转换器
        /// </summary>
        private readonly Func<string?, object?> valueConverter;

        /// <summary>
        /// 模型属性描述器
        /// </summary>
        /// <param name="property"></param>
        public ModelPropertyDescriptor(PropertyInfo property)
            : base(property)
        {
            var typeAttr = property.GetCustomAttribute<ColumnTypeAttribute>();
            if (typeAttr != null && typeAttr.ColumnType == ColumnType.Timestamp)
            {
                this.Name = Column.Time;
            }

            var nameAttr = property.GetCustomAttribute<ColumnNameAttribute>();
            if (nameAttr != null)
            {
                this.Name = nameAttr.Name;
            }

            this.valueConverter = CreateValueConverter(property.PropertyType);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public void SetStringValue(T instance, string? value)
        {
            var castValue = this.valueConverter.Invoke(value);
            base.SetValue(instance, castValue);
        }

        /// <summary>
        /// 创建转换器
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private static Func<string?, object?> CreateValueConverter(Type targetType)
        {
            var canbeNull = false;
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                canbeNull = true;
                targetType = underlyingType;
            }

            if (targetType == typeof(string))
            {
                return value => value;
            }

            if (targetType == typeof(bool))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : bool.Parse(value))
                    : (value => value == null ? Throw(value) : bool.Parse(value));
            }

            if (targetType == typeof(sbyte))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : sbyte.Parse(value))
                    : (value => value == null ? Throw(value) : sbyte.Parse(value));
            }

            if (targetType == typeof(byte))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : byte.Parse(value))
                    : (value => value == null ? Throw(value) : byte.Parse(value));
            }

            if (targetType == typeof(short))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : short.Parse(value))
                    : (value => value == null ? Throw(value) : short.Parse(value));
            }

            if (targetType == typeof(int))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : int.Parse(value))
                    : (value => value == null ? Throw(value) : int.Parse(value));
            }

            if (targetType == typeof(long))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : long.Parse(value))
                    : (value => value == null ? Throw(value) : long.Parse(value));
            }

            if (targetType == typeof(ushort))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : ushort.Parse(value))
                    : (value => value == null ? Throw(value) : ushort.Parse(value));
            }

            if (targetType == typeof(uint))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : uint.Parse(value))
                    : (value => value == null ? Throw(value) : uint.Parse(value));
            }

            if (targetType == typeof(ulong))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : ulong.Parse(value))
                    : (value => value == null ? Throw(value) : ulong.Parse(value));
            }

            if (targetType == typeof(float))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : float.Parse(value))
                    : (value => value == null ? Throw(value) : float.Parse(value));
            }

            if (targetType == typeof(decimal))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : decimal.Parse(value))
                    : (value => value == null ? Throw(value) : decimal.Parse(value));
            }

            if (targetType == typeof(double))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : double.Parse(value))
                    : (value => value == null ? Throw(value) : double.Parse(value));
            }

            if (targetType == typeof(DateTime))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : DateTime.Parse(value))
                    : (value => value == null ? Throw(value) : DateTime.Parse(value));
            }

            if (targetType == typeof(DateTimeOffset))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : DateTimeOffset.Parse(value))
                    : (value => value == null ? Throw(value) : DateTimeOffset.Parse(value));
            }

            if (typeof(Enum).IsAssignableFrom(targetType) == true)
            {
                return canbeNull == true
                    ? (value => value == null ? null : Enum.Parse(targetType, value))
                    : (value => value == null ? Throw(value) : Enum.Parse(targetType, value));
            }

            if (targetType == typeof(Guid))
            {
                return canbeNull == true
                    ? (value => value == null ? default(object) : Guid.Parse(value))
                    : (value => value == null ? Throw(value) : Guid.Parse(value));
            }

            if (targetType == typeof(Version))
            {
                return value => value == null ? default : Version.Parse(value);
            }

            if (targetType == typeof(Uri))
            {
                return value => value == null ? default : new Uri(value);
            }

            if (targetType == typeof(Type))
            {
                return value => value == null ? default : Type.GetType(value);
            }


            return value => Throw(value);

            object Throw(string? value)
            {
                throw new NotSupportedException($"不支持将文本值{value}转换为类型{targetType}");
            }
        }
    }
}
