using System.Collections.Generic;

namespace Influxdb2.Client
{
    /// <summary>
    /// 数据表
    /// </summary>
    public interface IDataTable : IEnumerable<IDataRow>
    {
        /// <summary>
        /// 获取列的集合
        /// </summary>
        IList<string> Columns { get; }

        /// <summary>
        /// 获取数据行的数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 通过行索引获取数据行
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <returns></returns>
        IDataRow this[int rowIndex] { get; }
    }
}
