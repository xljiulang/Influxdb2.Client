using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示列的集合
    /// </summary>
    public class Columns : IEnumerable<string>
    {
        private readonly string columnsString;

        /// <summary>
        /// 所有列
        /// </summary>
        private readonly IEnumerable<string> columns;

        /// <summary>
        /// 获取列的数量
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// 获取是否为空
        /// </summary>
        public bool IsEmpty => this.Count == 0;


        /// <summary>
        /// 返回空集合
        /// </summary>
        public static Columns Empty { get; } = new Columns();

        /// <summary>
        /// 只包含_value列的集合
        /// </summary>
        public static Columns Value { get; } = new Columns(Column.Value);

        /// <summary>
        /// 只包含_time列的集合
        /// </summary>
        public static Columns Time { get; } = new Columns(Column.Time);

        /// <summary>
        /// 只包含_field列的集合
        /// </summary>
        public static Columns Field { get; } = new Columns(Column.Field);

        /// <summary>
        /// 包含_start和_stop列的集合
        /// </summary>
        public static Columns StartStop { get; } = new Columns("_start", "_stop");

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
            this.Count = columns.Length;
            this.columnsString = $"[{string.Join(", ", columns.Select(c => @$"""{c}"""))}]";
        }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.columnsString;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.columns.GetEnumerator();
        }
    }
}
