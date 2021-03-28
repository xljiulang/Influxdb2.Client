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
        private string? columnsString;

        /// <summary>
        /// 所有列
        /// </summary>
        private readonly IList<string> columns;


        /// <summary>
        /// 获取列的数量
        /// </summary>
        public int Count => this.columns.Count;

        /// <summary>
        /// 获取是否为空
        /// </summary>
        public bool IsEmpty => this.columns.Count == 0;

        /// <summary>
        /// 通过索引获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index] => this.columns[index];

        /// <summary>
        /// 返回空集合
        /// </summary>
        public static Columns Empty { get; } = Create();

        /// <summary>
        /// 只包含_value列的集合
        /// </summary>
        public static Columns Value { get; } = Create(Column.Value);

        /// <summary>
        /// 只包含_time列的集合
        /// </summary>
        public static Columns Time { get; } = Create(Column.Time);

        /// <summary>
        /// 只包含_field列的集合
        /// </summary>
        public static Columns Field { get; } = Create(Column.Field);

        /// <summary>
        /// 包含_start和_stop列的集合
        /// </summary>
        public static Columns StartStop { get; } = Create(Column.Start, Column.Stop);

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
        /// 表示多个列名
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public Columns(IList<string> columns)
        {
            this.columns = columns;
        }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.columnsString == null)
            {
                this.columnsString = $"[{string.Join(", ", this.columns.Select(c => @$"""{c}"""))}]";
            }
            return this.columnsString;
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)this.columns).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.columns.GetEnumerator();
        }
    }
}
