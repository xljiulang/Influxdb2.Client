using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Influxdb2.Client
{
    /// <summary>
    /// 数据操作扩展
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// 尝试获取第一行的指定列的值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="column">指定列</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool TryGetFirstValue<TValue>(this IDataTable dataTable, string column, [MaybeNullWhen(false)] out TValue value)
        {
            if (dataTable.Rows.Count == 0)
            {
                value = default;
                return false;
            }
            return dataTable.Rows[0].TryGetValue(column, out value);
        }

        /// <summary>
        /// 尝试获取列的数据并转换指定类型
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dataRow"></param>
        /// <param name="column">列名</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetValue<TValue>(this IDataRow dataRow, string column, [MaybeNullWhen(false)] out TValue value)
        {
            if (dataRow.TryGetValue(column, out var stringValue))
            {
                if (stringValue != null)
                {
                    if (typeof(TValue) == typeof(string))
                    {
                        value = Unsafe.As<string, TValue>(ref stringValue);
                    }
                    else
                    {
                        var castValue = ConvertToType(stringValue, typeof(TValue));
                        value = (TValue)castValue;
                    }
                    return true;
                }
            }

            value = default;
            return false;
        }

        /// <summary>
        /// 转换为目标类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="targetType">转换的目标类型</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        private static object ConvertToType(string value, Type targetType)
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                targetType = underlyingType;
            }

            if (targetType.IsEnum == true)
            {
                return Enum.Parse(targetType, value.ToString(), true);
            }

            if (typeof(IConvertible).IsAssignableFrom(targetType))
            {
                return ((IConvertible)value).ToType(targetType, null);
            }

            if (typeof(DateTimeOffset) == targetType)
            {
                return DateTimeOffset.Parse(value.ToString());
            }

            if (typeof(Guid) == targetType)
            {
                return Guid.Parse(value.ToString());
            }

            throw new NotSupportedException($"不支持将值{value}转换为{targetType}");
        }
    }
}
