using System.Collections.Generic;

namespace Influxdb2.Client
{
    /// <summary>
    /// 数据表
    /// </summary>
    public interface IDataTable
    {
        /// <summary>
        /// 获取是否为空
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 获取所有数据行
        /// </summary>
        IList<IDataRow> Rows { get; }
    }
}
