using System.Collections.Generic;
using System.Diagnostics;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 数据表
    /// </summary>
    [DebuggerDisplay("RowCount = {Rows.Count}")]
    sealed class DataTable : IDataTable
    {
        /// <summary>
        /// 获取是否为空
        /// </summary>
        public bool IsEmpty => this.Rows.Count == 0;

        /// <summary>
        /// 获取所有数据行
        /// </summary>
        public IList<IDataRow> Rows { get; } = new List<IDataRow>();
    }
}
