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
        /// 所有值
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IList<string> values;

        /// <summary>
        /// 所有列
        /// </summary>
        public IList<string> Columns { get; }

        /// <summary>
        /// 通过列索引获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string? this[int columnIndex] => this.GetValue(columnIndex);

        /// <summary>
        /// 获取多列的值
        /// </summary>
        /// <param name="column">列集合</param>
        /// <returns></returns>
        public ColumnValue[] this[Columns columns]
        {
            get
            {
                var index = 0;
                var array = new ColumnValue[columns.Count];
                foreach (var column in columns)
                {
                    this.map.TryGetValue(column, out var value);
                    array[index] = new ColumnValue(column, value);
                    index += 1;
                }
                return array;
            }
        }

        /// <summary>
        /// 数据行
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        public DataRow(IList<string> columns, IList<string> values)
        {
            this.Columns = columns;
            this.values = values;

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                var value = this.GetValue(i);
                this.map.TryAdd(column, value);
            }
        }

        /// <summary>
        /// 获取指定列索引对应的Null处理的值
        /// </summary> 
        /// <param name="columnIndex">列索引</param>
        /// <returns></returns> 
        private string? GetValue(int columnIndex)
        {
            if (columnIndex > this.values.Count)
            {
                return null;
            }

            var value = this.values[columnIndex];
            return value.Length == 0 ? null : value;
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
                var value = this.GetValue(i);
                yield return new ColumnValue(column, value);
            }
        }
    }
}
