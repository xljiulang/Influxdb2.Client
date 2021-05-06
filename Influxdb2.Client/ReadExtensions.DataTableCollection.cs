namespace Influxdb2.Client
{
    /// <summary>
    /// 数据读取扩展
    /// </summary>
    public static partial class ReadExtensions
    {
        /// <summary>
        /// 确保没有错误的表格
        /// </summary>
        /// <param name="dataTables"></param>
        /// <exception cref="InfluxdbException"></exception>
        /// <returns></returns>
        public static IDataTableCollection EnsureNoError(this IDataTableCollection dataTables)
        {
            const string ErrorColumn = "error";
            foreach (var table in dataTables)
            {
                if (table.TableType == TableType.Error)
                {
                    var err = table.GetFirstValueOrDefault(ErrorColumn) ?? "未知错误 ";
                    throw new InfluxdbException(new InfuxdbError { Err = err });
                }
            }
            return dataTables;
        }

        /// <summary>
        /// 获取包含指定列的的第一个表的首行对应列的值
        /// 获取不到则返回类型默认值
        /// </summary>
        /// <param name="dataTables"></param> 
        /// <param name="column"></param>
        /// <returns></returns> 
        public static string? GetFirstValueOrDefault(this IDataTableCollection dataTables, string column)
        {
            foreach (var table in dataTables)
            {
                if (table.Columns.Contains(column))
                {
                    return table.GetFirstValueOrDefault(column);
                }
            }
            return default;
        }

        /// <summary>
        /// 获取包含指定列的的第一个表的首行对应列的值
        /// 获取不到则返回类型默认值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dataTables"></param> 
        /// <param name="column"></param>
        /// <returns></returns> 
        public static TValue GetFirstValueOrDefault<TValue>(this IDataTableCollection dataTables, string column) where TValue : struct
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
    }
}
