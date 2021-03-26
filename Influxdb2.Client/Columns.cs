using System.Linq;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示列的集合
    /// </summary>
    public class Columns
    {
        private readonly string[] columns;

        /// <summary>
        /// 返回空集合
        /// </summary>
        public static Columns Empty { get; } = new Columns();

        /// <summary>
        /// 只包含_value列的集合
        /// </summary>
        public static Columns ValueColumn { get; } = new Columns("_value");

        /// <summary>
        /// 创建多个列的结合
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static Columns Create(params string[] columns)
        {
            return new Columns(columns);
        }

        /// <summary>
        /// 表示多个列名
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public Columns(params string[] columns)
        {
            this.columns = columns;
        }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{string.Join(",", this.columns.Select(c => @$"""{c}"""))}]";
        }
    }
}
