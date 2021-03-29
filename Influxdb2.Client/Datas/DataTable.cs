using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 数据表
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    sealed class DataTable : IDataTable
    {
        /// <summary>
        /// 获取所有数据行
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<IDataRow> rows = new List<IDataRow>();

        /// <summary>
        /// 获取列的集合
        /// </summary>
        public IList<string> Columns { get; }

        /// <summary>
        /// 获取数据行的数量
        /// </summary>
        public int Count => this.rows.Count;

        /// <summary>
        /// 通过行索引获取数据行
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <returns></returns>
        public IDataRow this[int rowIndex] => this.rows[rowIndex];

        /// <summary>
        /// 数据表
        /// </summary>
        /// <param name="columns">列集合</param>
        public DataTable(IList<string> columns)
        {
            this.Columns = columns;
        }

        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="dataRow"></param>
        public void AddDataRow(IDataRow dataRow)
        {
            this.rows.Add(dataRow);
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IDataRow> GetEnumerator()
        {
            return this.rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
