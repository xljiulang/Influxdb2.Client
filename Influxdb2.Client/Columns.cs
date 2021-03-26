using System.Linq;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示列的集合
    /// </summary>
    public class Columns
    {
        private readonly string columnsString;

        /// <summary>
        /// 返回空集合
        /// </summary>
        public static Columns Empty { get; } = new Columns();

        /// <summary>
        /// 只包含_value列的集合
        /// </summary>
        public static Columns ValueColumn { get; } = new Columns("_value");

        /// <summary>
        /// 只包含_time列的集合
        /// </summary>
        public static Columns TimeColumn { get; } = new Columns("_time");


        /// <summary>
        /// 只包含_field列的集合
        /// </summary>
        public static Columns FieldColumn { get; } = new Columns("_field");

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
            this.columnsString = $"[{string.Join(",", columns.Select(c => @$"""{c}"""))}]";
        }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.columnsString;
        }
    }
}
