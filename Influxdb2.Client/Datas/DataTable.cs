using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        /// 尝试获取第一行的指定列的值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="column">指定列</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool TryGetFirstValue<TValue>(string column, [MaybeNullWhen(false)] out TValue value)
        {
            if (this.IsEmpty == true)
            {
                value = default;
                return false;
            }
            return this.Rows.First().TryGetValue(column, out value);
        }

        /// <summary>
        /// 从csv读取得到多表格
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static async Task<IList<IDataTable>> ParseAsync(CsvReader reader)
        {
            var columns = default(IList<string>);
            var table = default(DataTable);
            var tables = new List<IDataTable>();

            while (reader.CanRead == true)
            {
                var csvLine = await reader.ReadlineAsync();
                if (IsValidLine(csvLine) == false)
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

                var row = new DataRow();
                for (var i = 1; i < csvLine.Count; i++)
                {
                    var column = columns[i];
                    var value = csvLine[i];
                    if (value.Length == 0)
                    {
                        value = null;
                    }
                    row.TryAdd(column, value);
                }

                if (table == null)
                {
                    table = new DataTable();
                    tables.Add(table);
                }
                table.Rows.Add(row);
            }

            return tables;
        }



        /// <summary>
        /// csvLine是否有效
        /// </summary> 
        /// <returns></returns>
        private static bool IsValidLine(IList<string> csvLine)
        {
            const string Error = "error";

            if (csvLine.Count == 0)
            {
                return false;
            }

            var value = csvLine[0];
            if (value.StartsWith('#') || value == Error)
            {
                return false;
            }

            return true;
        }
    }
}
