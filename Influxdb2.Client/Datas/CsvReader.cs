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

        /// <summary>
        /// 空白行
        /// </summary>
        private static readonly IList<string> emptyLine = new List<string>().AsReadOnly();

        /// <summary>
        /// 获取是否可以读取
        /// </summary>
        public bool CanRead => this.reader.EndOfStream == false;

        /// <summary>
        /// Csv读取器
        /// </summary>
        /// <param name="stream"></param>
        public CsvReader(Stream stream)
        {
            this.reader = new StreamReader(stream, Encoding.UTF8, false, 8 * 1024, true);
        }

        /// <summary>
        /// 读取一行
        /// </summary>
        /// <returns></returns>
        public async Task<IList<string>> ReadlineAsync()
        {
            var content = await this.reader.ReadLineAsync();
            return string.IsNullOrEmpty(content) ? emptyLine : Parse(content);
        }

        /// <summary>
        /// 解析内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static IList<string> Parse(ReadOnlySpan<char> content)
        {
            var cells = new List<string>();
            while (true)
            {
                var length = GetCellValueLength(content);
                if (length < 0)
                {
                    var stringValue = DecodeCellValue(content);
                    cells.Add(stringValue);
                    break;
                }
                else
                {
                    var value = content.Slice(0, length);
                    var stringValue = DecodeCellValue(value);
                    cells.Add(stringValue);
                    content = content[(length + 1)..];
                }
            }

            return cells;
        }


        /// <summary>
        /// 从内容查找单元的值长度
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        private static int GetCellValueLength(ReadOnlySpan<char> content)
        {
            if (content.IsEmpty == true)
            {
                return -1;
            }

            if (content[0] != '"')
            {
                return content.IndexOf(',');
            }

            var i = 1;
            while (i < content.Length - 1)
            {
                var c = content[i];
                var n = content[i + 1];
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
        /// 解码单元的值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        private static string DecodeCellValue(ReadOnlySpan<char> value)
        {
            if (value.IsEmpty == true)
            {
                return string.Empty;
            }

            if (value[0] == '"' && value[^1] == '"')
            {
                value = value[1..^1];
            }

            return value.ToString().Replace("\"\"", "\"");
        }
    }
}
