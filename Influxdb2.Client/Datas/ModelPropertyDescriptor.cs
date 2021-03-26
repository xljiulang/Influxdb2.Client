using Influxdb2.Client.Datas;
using System;
using System.Reflection;

namespace Influxdb2.Client.Protocols
{
    /// <summary>
    /// 模型属性描述器
    /// </summary>
    sealed class ModelPropertyDescriptor : Property<object, object>
    {
        /// <summary>
        /// 值转换器
        /// </summary>
        private readonly Func<string?, object?>? valueConverter;

        /// <summary>
        /// 模型属性描述器
        /// </summary>
        /// <param name="property"></param>
        public ModelPropertyDescriptor(PropertyInfo property)
            : base(property)
        {
            var attr = property.GetCustomAttribute<ColumnTypeAttribute>();
            if (attr != null && attr.ColumnType == ColumnType.Time)
            {
                this.Name = "_time";
            }

            this.valueConverter = GetValueConverter(property.PropertyType);
        }

        /// <summary>
        /// 获取转换器
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private static Func<string?, object?>? GetValueConverter(Type targetType)
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

            if (targetType == typeof(sbyte))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : sbyte.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : sbyte.Parse(value);
                }
            }

            if (targetType == typeof(byte))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : byte.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : byte.Parse(value);
                }
            }

            if (targetType == typeof(short))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : short.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : short.Parse(value);
                }
            }

            if (targetType == typeof(int))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : int.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : int.Parse(value);
                }
            }

            if (targetType == typeof(long))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : long.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : long.Parse(value);
                }
            }

            if (targetType == typeof(ushort))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : ushort.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : ushort.Parse(value);
                }
            }

            if (targetType == typeof(uint))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : uint.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : uint.Parse(value);
                }
            }

            if (targetType == typeof(ulong))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : ulong.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : ulong.Parse(value);
                }
            }

            if (targetType == typeof(float))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : float.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : float.Parse(value);
                }
            }

            if (targetType == typeof(decimal))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : decimal.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : decimal.Parse(value);
                }
            }

            if (targetType == typeof(double))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : double.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : double.Parse(value);
                }
            }

            if (targetType == typeof(DateTime))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : DateTime.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : DateTime.Parse(value);
                }
            }

            if (targetType == typeof(DateTimeOffset))
            {
                if (canbeNull == true)
                {
                    return value => value == null ? default(object) : DateTimeOffset.Parse(value);
                }
                else
                {
                    return value => value == null ? Throw() : DateTimeOffset.Parse(value);
                }
            }

            if (typeof(Enum).IsAssignableFrom(targetType) == true)
            {
                if (canbeNull == true)
                {
                    return value => value == null ? null : Enum.Parse(targetType, value);
                }
                else
                {
                    return value => value == null ? Throw() : Enum.Parse(targetType, value);
                }
            }

            object Throw()
            {
                throw new NotSupportedException($"不支持将文本值转换为类型{targetType}");
            }

            return null;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public override void SetValue(object instance, object value)
        {
            if (this.valueConverter == null)
            {
                throw new NotSupportedException($"不支持将文本值转换为类型{this.Info.PropertyType}");
            }

            var valueCast = this.valueConverter.Invoke((string?)value);
            base.SetValue(instance, valueCast!);
        }
    }
}
