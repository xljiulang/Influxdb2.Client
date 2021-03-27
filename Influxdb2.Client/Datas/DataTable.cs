using System.Collections.Generic;
using System.Threading.Tasks;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 数据表
    /// </summary>
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

        /// <summary>
        /// 从csv读取得到多表格
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static async Task<List<IDataTable>> FromCsvAsync(CsvReader reader)
        {
            var rowIndex = -1;
            var tableIndex = -1;

            var row = new DataRow();
            var table = new DataTable();

            var tables = new List<IDataTable>();
            while (await reader.ReadAsync())
            {
                if (reader.TableIndex != tableIndex)
                {
                    rowIndex = -1;
                    table = new DataTable();
                    tables.Add(table);
                }

                if (reader.RowIndex != rowIndex)
                {
                    row = new DataRow();
                    table.Rows.Add(row);
                }

                if (string.IsNullOrEmpty(reader.Column) == false)
                {
                    row.TryAdd(reader.Column, reader.Value);
                }

                rowIndex = reader.RowIndex;
                tableIndex = reader.TableIndex;
            }

            return tables;
        }
    }
}
