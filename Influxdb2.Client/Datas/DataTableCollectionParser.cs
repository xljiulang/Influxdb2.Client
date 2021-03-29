using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// DataTable解析器
    /// </summary>
    static class DataTableCollectionParser
    {
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

        /// <summary>
        /// csvLine是否有效
        /// </summary> 
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
