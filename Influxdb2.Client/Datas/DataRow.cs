using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 数据行
    /// </summary>
    sealed class DataRow : Dictionary<string, string?>, IDataRow
    {
        /// <summary>
        /// 获取所有列名
        /// </summary>
        ICollection<string> IDataRow.Columns => this.Keys;

        /// <summary>
        /// 获取所有值
        /// </summary>
        ICollection<string?> IDataRow.Values => this.Values;

        /// <summary>
        /// 获取多列的值
        /// </summary>
        /// <param name="column">列集合</param>
        /// <returns></returns>
        public ColumnValue[] this[Columns columns]
        {
            get
            {
                var index = 0;
                var array = new ColumnValue[columns.Count];
                foreach (var column in columns)
                {
                    this.TryGetValue(column, out var value);
                    array[index] = new ColumnValue(column, value);
                    index += 1;
                }
                return array;
            }
        }

        /// <summary>
        /// 尝试获取列的数据并转换指定类型
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue<TValue>(string column, [MaybeNull] out TValue value)
        {
            if (base.TryGetValue(column, out var stringValue))
            {
                if (typeof(TValue) == typeof(string))
                {
                    value = Unsafe.As<string?, TValue>(ref stringValue);
                }
                else
                {
                    var castValue = ConvertToType(stringValue, typeof(TValue));
                    value = (TValue)castValue;
                }
                return true;
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
        private static object? ConvertToType(string? value, Type targetType)
        {
            var canbeNull = false;
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                canbeNull = true;
                targetType = underlyingType;
            }

            if (value == null)
            {
                return null;
            }
            else if (canbeNull == false)
            {
                throw new NotSupportedException($"不支持将Null值转换为{targetType}");
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
