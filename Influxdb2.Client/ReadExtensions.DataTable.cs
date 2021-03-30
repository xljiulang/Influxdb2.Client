using System.Collections.Generic;
using System.Linq;

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
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static IList<TModel> ToModels<TModel>(this IDataTable dataTable) where TModel : new()
        {
            return dataTable.Select(item => item.ToModel<TModel>()).ToList();
        }

        /// <summary>
        /// 获取首行指定列的值
        /// 获取不到则返回类型默认值
        /// </summary> 
        /// <param name="dataTable"></param>
        /// <param name="column"></param>
        /// <returns></returns> 
        public static string? GetFirstValueOrDefault(this IDataTable dataTable, string column)  
        {
            return dataTable.Count > 0
                ? dataTable[0].GetValueOrDefault(column)
                : default;
        }

        /// <summary>
        /// 获取首行指定列的值
        /// 获取不到则返回类型默认值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dataTable"></param>
        /// <param name="column"></param>
        /// <returns></returns> 
        public static TValue GetFirstValueOrDefault<TValue>(this IDataTable dataTable, string column) where TValue : struct
        {
            return dataTable.Count > 0
                ? dataTable[0].GetValueOrDefault<TValue>(column)
                : default;
        }
    }
}
