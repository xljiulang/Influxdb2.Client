using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 数据行
    /// </summary>
    sealed class DataRow : IDataRow
    {
        /// <summary>
        /// 键值映射
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<string, string?> map = new();

        /// <summary>
        /// 所有列
        /// </summary>
        public IList<string> Columns { get; }

        /// <summary>
        /// 所有值
        /// </summary>
        public IList<string?> Values { get; }


        /// <summary>
        /// 通过列索引获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string? this[int columnIndex] => this.Values[columnIndex]; 

        /// <summary>
        /// 数据行
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        public DataRow(IList<string> columns, IList<string> values)
        {
            var _values = TransformValues(values, columns.Count);
            for (var i = 0; i < columns.Count; i++)
            {
                this.map.TryAdd(columns[i], _values[i]);
            }

            this.Columns = columns;
            this.Values = _values;
        }

        /// <summary>
        /// 变换所有值
        /// </summary>
        /// <param name="values"></param>
        /// <param name="columnCount">列的数量</param>
        /// <returns></returns>
        private static IList<string?> TransformValues(IList<string> values, int columnCount)
        {
            var result = new List<string?>(columnCount);
            foreach (var value in values)
            {
                if (value.Length == 0)
                {
                    result.Add(null);
                }
                else
                {
                    result.Add(value);
                }
            }
            return result;
        }

        /// <summary>
        /// 尝试获取列的数据
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string column, out string? value)
        {
            return this.map.TryGetValue(column, out value);
        }



        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ColumnValue> GetEnumerator()
        {
            return this.ToEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private IEnumerable<ColumnValue> ToEnumerable()
        {
            for (var i = 0; i < this.Columns.Count; i++)
            {
                var column = this.Columns[i];
                var value = this.Values[i];
                yield return new ColumnValue(column, value);
            }
        }
    }
}
