using Influxdb2.Client.Datas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示查询结果
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        /// 获取所有表格
        /// </summary>
        public IList<IDataTable> Tables { get; }

        /// <summary>
        /// 查询结果
        /// </summary>
        /// <param name="tables">表格</param>
        public QueryResult(IList<IDataTable> tables)
        {
            this.Tables = tables;
        }

        /// <summary>
        /// 从csv解析
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static async Task<QueryResult> ParseAsync(CsvReader reader)
        {
            var rowIndex = int.MinValue;
            var tableIndex = int.MinValue;

            DataRow? row = null;
            DataTable? table = null;

            var tables = new List<IDataTable>();
            while (await reader.ReadAsync())
            {
                if (table == null)
                {
                    table = new DataTable();
                    tables.Add(table);
                }

                if (row == null)
                {
                    row = new DataRow();
                    table.Rows.Add(row);
                }

                if (string.IsNullOrEmpty(reader.Column) == false)
                {
                    row.TryAdd(reader.Column, reader.Value);
                }

                if (rowIndex > reader.RowIndex)
                {
                    row = null;
                }

                if (tableIndex > reader.TableIndex)
                {
                    table = null;
                }

                rowIndex = reader.RowIndex;
                tableIndex = reader.TableIndex;
            }

            return new QueryResult(tables);
        }
    }
}
