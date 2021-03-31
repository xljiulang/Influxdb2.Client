using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// 表示表格集合
    /// </summary> 
    sealed class DataTableCollection : IDataTableCollection
    {
        /// <summary>
        /// 所有表格
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<IDataTable> tables;

        /// <summary>
        /// 获取表格的数量
        /// </summary>
        public int Count => this.tables.Count;

        /// <summary>
        /// 通过索引获取表格
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IDataTable this[int index] => this.tables[index];

        /// <summary>
        /// 表格集合
        /// </summary>
        /// <param name="tables">表格</param>
        public DataTableCollection(IList<IDataTable> tables)
        {
            this.tables = tables;
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IDataTable> GetEnumerator()
        {
            return this.tables.GetEnumerator();
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.tables.GetEnumerator();
        }

        /// <summary>
        /// 从csv读取得到多表格
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static async Task<DataTableCollection> ParseAsync(CsvReader reader)
        {
            var columns = default(IList<string>);
            var table = default(DataTable);
            var tables = new List<IDataTable>();

            while (reader.CanRead == true)
            {
                var csvLine = await reader.ReadlineAsync();
                if (csvLine.Count == 0 || csvLine[0].StartsWith('#'))
                {
                    columns = null;
                    table = null;
                    continue;
                }

                if (columns == null)
                {
                    columns = csvLine;
                    continue;
                }

                var row = new DataRow(columns, csvLine);
                if (table == null)
                {
                    table = new DataTable(columns);
                    tables.Add(table);
                }
                table.AddDataRow(row);
            }

            return new DataTableCollection(tables);
        }
    }
}
