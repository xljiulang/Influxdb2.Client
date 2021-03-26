using System.Collections.Generic;

namespace Influxdb2.Client
{
    /// <summary>
    /// 数据行
    /// </summary>
    public interface IDataRow : IEnumerable<string>
    {
        /// <summary>
        /// 获取所有列名
        /// </summary>
        string[] Columns { get; }

        /// <summary>
        /// 尝试获取列的数据
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetValue(string column, out string? value);

        /// <summary>
        /// 转换为强类型
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        TModel ToModel<TModel>() where TModel : new();
    }
}
