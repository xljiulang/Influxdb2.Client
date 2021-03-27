using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// Csv读取器
    /// </summary>
    sealed class CsvReader
    {
        private readonly StreamReader reader;

        private string[] columns = Array.Empty<string>();

        private int columnIndex = -1;

        private int valuePostion = 0;

        /// <summary>
        /// 当前行的文本
        /// </summary>
        private string currentLine = string.Empty;


        /// <summary>
        /// 获取值
        /// </summary>
        public string? Value { get; private set; }

        /// <summary>
        /// 获取列名
        /// </summary>
        public string Column { get; private set; } = string.Empty;

        /// <summary>
        /// 获取行的索引
        /// </summary>
        public int RowIndex { get; private set; } = -1;

        /// <summary>
        /// 获取表格的索引
        /// </summary>
        public int TableIndex { get; private set; } = -1;

        /// <summary>
        /// Csv读取器
        /// </summary>
        /// <param name="stream"></param>
        public CsvReader(Stream stream)
        {
            this.reader = new StreamReader(stream, Encoding.UTF8, false, 8 * 1024, true);
        }

        /// <summary>
        /// 读取一次，读取成功则返回true
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ReadAsync()
        {
            if (this.valuePostion >= this.currentLine.Length)
            {
                if (this.reader.EndOfStream == true)
                {
                    return false;
                }

                this.valuePostion = 0;
                this.currentLine = await this.reader.ReadLineAsync();

                // 空行表示新的表格
                if (string.IsNullOrEmpty(this.currentLine) == true)
                {
                    this.columnIndex = -1;
                    return await ReadAsync();
                }
            }

            // 新表格初始化columns
            if (this.columnIndex < 0)
            {
                this.RowIndex = 0;
                this.TableIndex += 1;

                this.columnIndex = 0;
                this.columns = this.currentLine.Split(',');
                this.currentLine = string.Empty;
            }

            // 读完所有列，重新读取（行可能没有读完)
            if (this.columnIndex >= this.columns.Length)
            {
                this.RowIndex += 1;
                this.columnIndex = 0;
                return await ReadAsync();
            }

            // 读完一行，重新读取（列可能没有读完)
            if (this.valuePostion >= this.currentLine.Length)
            {
                this.RowIndex += 1;
                return await ReadAsync();
            }

            ReadColumnValue();

            return true;
        }

        /// <summary>
        /// 读取列与值
        /// </summary>   
        private void ReadColumnValue()
        {
            this.Column = this.columns[this.columnIndex];
            this.columnIndex += 1;

            var valueSpan = this.currentLine.AsSpan(this.valuePostion);
            var index = FindValueIndex(valueSpan);
            if (index < 0)
            {
                this.Value = DecodeValue(valueSpan);
                this.valuePostion += valueSpan.Length;
            }
            else
            {
                this.Value = DecodeValue(valueSpan.Slice(0, index));
                this.valuePostion += index + 1;
            }
        }

        /// <summary>
        /// 查找值的分隔索引
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        private static int FindValueIndex(ReadOnlySpan<char> span)
        {
            if (span.IsEmpty)
            {
                return -1;
            }

            if (span[0] != '"')
            {
                return span.IndexOf(',');
            }

            var i = 1;
            while (i < span.Length - 1)
            {
                var c = span[i];
                var n = span[i + 1];
                if (c == n && c == '"')
                {
                    i += 2;
                    continue;
                }

                if (c == '"' && n == ',')
                {
                    return i + 1;
                }

                i += 1;
            }

            return -1;
        }

        /// <summary>
        /// 值解码
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        private static string? DecodeValue(ReadOnlySpan<char> span)
        {
            if (span.IsEmpty)
            {
                return null;
            }

            if (span[0] == '"')
            {
                span = span.Slice(1);
            }

            if (span[span.Length - 1] == '"')
            {
                span = span.Slice(0, span.Length - 1);
            }

            var value = span.Contains("\"\"", StringComparison.InvariantCulture)
                ? span.ToString().Replace("\"\"", "\"")
                : span.ToString();

            return string.IsNullOrEmpty(value) ? null : value;
        }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Table[{this.TableIndex}].Row[{this.RowIndex}].{this.Column} = {this.Value}";
        }


        /// <summary>
        /// 读取为多个表
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public async Task<IList<IDataTable>> ReadDataTablesAsync()
        {
            var rowIndex = int.MinValue;
            var tableIndex = int.MinValue;

            var row = new DataRow();
            var table = new DataTable();

            var tables = new List<IDataTable>();
            while (await this.ReadAsync())
            {
                if (this.TableIndex > tableIndex)
                {
                    table = new DataTable();
                    tables.Add(table);
                }

                if (this.RowIndex != rowIndex)
                {
                    row = new DataRow();
                    table.Rows.Add(row);
                }

                if (string.IsNullOrEmpty(this.Column) == false)
                {
                    row.TryAdd(this.Column, this.Value);
                }

                rowIndex = this.RowIndex;
                tableIndex = this.TableIndex;
            }

            return tables;
        }
    }
}
