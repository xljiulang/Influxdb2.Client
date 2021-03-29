using Influxdb2.Client.Datas;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Influxdb2.Client
{
    /// <summary>
    /// 数据读取扩展
    /// </summary>
    public static partial class ReadExtensions
    {
        /// <summary>
        /// 获取包含指定列的的第一个表的首行对应列的值
        /// 获取不到则返回类型默认值
        /// </summary>
        /// <typeparam name="TValue"></typeparam> 
        /// <param name="column"></param>
        /// <returns></returns>
        [return: MaybeNull]
        public static TValue GetFirstValueOrDefault<TValue>(this IDataTableCollection dataTables, string column)
        {
            foreach (var table in dataTables)
            {
                if (table.Columns.Contains(column))
                {
                    return table.GetFirstValueOrDefault<TValue>(column);
                }
            }
            return default;
        }


        /// <summary>
        /// 以_time列为分组条件合并所有数据行并转换为指定强类型模型
        /// </summary>
        /// <param name="dataTables"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static IList<TModel> ToModels<TModel>(this IDataTableCollection dataTables) where TModel : new()
        {
            return dataTables.ToModels<TModel>(Columns.Time);
        }

        /// <summary>
        /// 以指定多列为分组条件合并所有数据行并转换为指定强类型模型
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dataTables"></param>
        /// <param name="groupColumns">列值相同的行，将整合到一个model实例</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static IList<TModel> ToModels<TModel>(this IDataTableCollection dataTables, Columns groupColumns) where TModel : new()
        {
            if (groupColumns.IsEmpty)
            {
                throw new ArgumentException("必须指定至少一个column");
            }

            var result = new List<TModel>();
            var descriptor = ModelDescriptor.Get(typeof(TModel));
            var rowGroups = dataTables
                .SelectMany(item => item)
                .GroupBy(item => item[groupColumns], ColumnValuesEqualityComparer.Instance);

            foreach (var group in rowGroups)
            {
                var model = new TModel();
                var firstRow = group.First(); // 同一组的非field属性，共用分组的第一条记录的值              
                var fieldValueMap = default(Dictionary<string, string?>);

                foreach (var property in descriptor.PropertyDescriptors)
                {
                    if (firstRow.TryGetValue(property.Name, out var value))
                    {
                        property.SetStringValue(model, value);
                    }
                    else if (property.IsFieldColumn != false)
                    {
                        if (fieldValueMap == null)
                        {
                            fieldValueMap = CreateFiledValueMap(group);
                        }

                        if (fieldValueMap.TryGetValue(property.Name, out value))
                        {
                            property.SetStringValue(model, value);
                        }
                    }
                }

                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 创建_field与_value的映射关系
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private static Dictionary<string, string?> CreateFiledValueMap(IEnumerable<IDataRow> rows)
        {
            var fieldValues = new Dictionary<string, string?>();
            foreach (var row in rows)
            {
                if (row.TryGetValue(Column.Field, out var field))
                {
                    if (field != null && row.TryGetValue(Column.Value, out var value))
                    {
                        fieldValues.TryAdd(field, value);
                    }
                }
            }
            return fieldValues;
        }


        /// <summary>
        /// ColumnValue集合相等比较
        /// </summary>
        private class ColumnValuesEqualityComparer : IEqualityComparer<ColumnValue[]>
        {
            /// <summary>
            /// 获取实例
            /// </summary>
            public static IEqualityComparer<ColumnValue[]> Instance { get; } = new ColumnValuesEqualityComparer();

            /// <summary>
            /// 是否相等
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(ColumnValue[] x, ColumnValue[] y)
            {
                // 比较哈希即可
                return x.Length == y.Length;
            }

            /// <summary>
            /// 获取哈希
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(ColumnValue[] obj)
            {
                var hashCode = 0;
                foreach (var item in obj)
                {
                    hashCode ^= item.GetHashCode();
                }
                return hashCode;
            }
        }
    }
}
