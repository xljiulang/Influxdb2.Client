namespace Influxdb2.Client
{
    /// <summary>
    /// 数据行
    /// </summary>
    public interface IDataRow
    {
        /// <summary>
        /// 获取多列的值
        /// </summary>
        /// <param name="column">列集合</param>
        /// <returns></returns>
        ColumnValue[] this[Columns columns] { get; }

        /// <summary>
        /// 是否包含指定的列
        /// </summary>
        bool ContainColumn(string column);

        /// <summary>
        /// 尝试获取列的数据
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetValue(string column, out string? value);
    }
}
