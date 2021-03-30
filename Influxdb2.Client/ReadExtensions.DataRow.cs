using Influxdb2.Client.Datas;
using System;
using System.Runtime.CompilerServices;

namespace Influxdb2.Client
{
    /// <summary>
    /// 数据读取扩展
    /// </summary>
    public static partial class ReadExtensions
    {
        /// <summary>
        /// 转换为强类型模型
        /// </summary>
        /// <typeparam name="TModel">模型类型</typeparam>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static TModel ToModel<TModel>(this IDataRow dataRow) where TModel : new()
        {
            var model = new TModel();
            foreach (var property in ModelDescriptor<TModel>.PropertyDescriptors)
            {
                if (dataRow.TryGetValue(property.Name, out var stringValue))
                {
                    property.SetStringValue(model, stringValue);
                }
            }
            return model;
        }

        /// <summary>
        /// 获取指定列的值
        /// 获取失败则返回null
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string? GetValueOrDefault(this IDataRow dataRow, string column)
        {
            dataRow.TryGetValue(column, out var value);
            return value;
        }

        /// <summary>
        /// 获取指定列的值
        /// 获取失败则返回类型默认值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dataRow"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TValue>(this IDataRow dataRow, string column) where TValue : struct
        {
            if (dataRow.TryGetValue(column, out var value) == false || value == null)
            {
                return default;
            }

            if (typeof(TValue) == typeof(DateTimeOffset))
            {
                var datetimeOffset = DateTimeOffset.Parse(value);
                return Unsafe.As<DateTimeOffset, TValue>(ref datetimeOffset);
            }

            return (TValue)((IConvertible)value).ToType(typeof(TValue), null);
        }
    }
}
