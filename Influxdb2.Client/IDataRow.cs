using System.Collections.Generic;

namespace Influxdb2.Client
{
    /// <summary>
    /// 数据行
    /// </summary>
    public interface IDataRow
    {
        /// <summary>
        /// 获取所有列名
        /// </summary>
        ICollection<string> Columns { get; }

        /// <summary>
        /// 获取所有值
        /// </summary>
        ICollection<string?> Values { get; }

        /// <summary>
        /// 获取多列的值
        /// </summary>
        /// <param name="column">列集合</param>
        /// <returns></returns>
        ColumnValue[] this[Columns columns] { get; }

        /// <summary>
        /// 尝试获取列的数据
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetValue(string column, out string? value);
    }
}
