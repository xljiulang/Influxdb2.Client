using System;
using System.Collections.Generic;
using System.Linq;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 数据行
    /// </summary>
    sealed class DataRow : Dictionary<string, string?>, IDataRow
    {
        /// <summary>
        /// 获取所有列名
        /// </summary>
        public string[] Columns => this.Keys.ToArray();

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
                var columnValues = new ColumnValue[columns.Count];
                foreach (var item in columns)
                {
                    this.TryGetValue(item, out var value);
                    columnValues[index] = new ColumnValue(item, value);
                    index += 1;
                }
                return columnValues;
            }
        }

        /// <summary>
        /// 数据行
        /// </summary>
        public DataRow()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
