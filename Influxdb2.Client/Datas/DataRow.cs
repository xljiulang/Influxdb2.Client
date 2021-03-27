﻿using System.Collections.Generic;
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
                var array = new ColumnValue[columns.Count];
                foreach (var column in columns)
                {
                    this.TryGetValue(column, out var value);
                    array[index] = new ColumnValue(column, value);
                    index += 1;
                }
                return array;
            }
        }
    }
}
