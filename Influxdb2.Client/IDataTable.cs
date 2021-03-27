using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Influxdb2.Client
{
    /// <summary>
    /// 数据表
    /// </summary>
    public interface IDataTable
    {
        /// <summary>
        /// 获取是否为空
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 获取所有数据行
        /// </summary>
        IList<IDataRow> Rows { get; }

        /// <summary>
        /// 尝试获取第一行的指定列的值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="column">指定列</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool TryGetFirstValue<TValue>(string column, [MaybeNull] out TValue value);
    }
}
