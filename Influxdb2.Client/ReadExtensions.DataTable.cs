using System.Diagnostics.CodeAnalysis;

namespace Influxdb2.Client
{
    /// <summary>
    /// 数据读取扩展
    /// </summary>
    public static partial class ReadExtensions
    {
        /// <summary>
        /// 获取首行指定列的值
        /// 获取不到则返回类型默认值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dataTable"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [return: MaybeNull]
        public static TValue GetFirstValueOrDefault<TValue>(this IDataTable dataTable, string column)
        {
            return dataTable.Count > 0 
                ? dataTable[0].GetValueOrDefault<TValue>(column) 
                : default;
        }
    }
}
