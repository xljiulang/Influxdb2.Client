using System.Collections.Generic;
using System.Linq;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示列的集合
    /// </summary>
    public class Columns
    {
        private readonly List<string> columnList = new List<string>();

        /// <summary>
        /// 返回空集合
        /// </summary>
        public static Columns Empty => new Columns();

        /// <summary>
        /// 多个值
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static Columns Values(params string[] columns) => Empty.Add(columns);

        /// <summary>
        /// 添加一个或多个列名
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public Columns Add(params string[] columns)
        {
            foreach (var item in columns)
            {
                this.columnList.Add(item);
            }
            return this;
        }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{string.Join(",", columnList.Select(c => @$"""{c}"""))}]";
        }
    }
}
