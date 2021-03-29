using System.Collections.Generic;

namespace Influxdb2.Client
{
    /// <summary>
    /// 数据表集合 
    /// </summary>
    public interface IDataTableCollection : IEnumerable<IDataTable>
    {
        /// <summary>
        /// 获取表格的数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 通过索引获取表格
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IDataTable this[int index] { get; }
    }
}