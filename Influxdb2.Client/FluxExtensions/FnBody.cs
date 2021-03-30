using System;
using System.Text;

namespace Influxdb2.Client
{
    /// <summary>
    /// 过滤函数的Body
    /// </summary>
    public class FnBody
    {
        private readonly StringBuilder builder = new();

        /// <summary>
        /// 返回r参数名的过滤函数
        /// </summary>
        public static FnBody R => new("r");

        /// <summary>
        /// 获取参数名
        /// </summary>
        public string ParamName { get; }

        /// <summary>
        /// 过滤函数
        /// </summary>
        /// <param name="paramName">参数名</param>
        private FnBody(string paramName)
        {
            this.ParamName = paramName;
        }

        /// <summary>
        /// and计算
        /// </summary>
        /// <param name="fn"></param>
        /// <returns></returns>
        public FnBody And(FnBody fn)
        {
            return this.ParamName == fn.ParamName
                ? this.And().Then($"({fn})")
                : throw new ArgumentException($"{nameof(fn)}的{nameof(ParamName)}必须为{ParamName}");
        }

        /// <summary>
        /// and关键字
        /// </summary>
        /// <returns></returns>
        public FnBody And()
        {
            return this.Then("and");
        }

        /// <summary>
        /// or计算
        /// </summary>
        /// <param name="fn"></param>
        /// <returns></returns> 
        public FnBody Or(FnBody fn)
        {
            return this.ParamName == fn.ParamName
                ? this.Or().Then($"({fn})")
                : throw new ArgumentException($"{nameof(fn)}的{nameof(ParamName)}必须为{ParamName}");
        }

        /// <summary>
        /// or关键字
        /// </summary>
        /// <returns></returns>
        public FnBody Or()
        {
            return this.Then("or");
        }

        /// <summary>
        /// measurement名等于
        /// </summary>
        /// <param name="measurement">名</param>
        /// <returns></returns>
        public FnBody MeasurementEquals(string measurement)
        {
            return this.ColumnEquals("_measurement", measurement);
        }

        /// <summary>
        /// 指定列的值为目标值
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public FnBody ColumnEquals(string column, object value)
        {
            return this.Column(column, "==", value);
        }

        /// <summary>
        /// 当列符合条件时
        /// </summary> 
        /// <param name="column">列名</param>
        /// <param name="op">比较符号</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public FnBody Column(string column, string op, object value)
        {
            return value is string stringValue
                ? this.Then(@$"{this.ParamName}.{column} {op} ""{stringValue}""")
                : this.Then(@$"{this.ParamName}.{column} {op} {value}");
        }

        /// <summary>
        /// 追加内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public FnBody Then(string content)
        {
            builder.Append(' ').Append(content).Append(' ');
            return this;
        }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.builder.ToString();
        }

        /// <summary>
        /// And
        /// </summary>
        /// <param name="fn1"></param>
        /// <param name="fn2"></param>
        /// <returns></returns>
        public static FnBody operator &(FnBody fn1, FnBody fn2)
        {
            return fn1.And(fn2);
        }

        /// <summary>
        /// Or
        /// </summary>
        /// <param name="fn1"></param>
        /// <param name="fn2"></param>
        /// <returns></returns>
        public static FnBody operator |(FnBody fn1, FnBody fn2)
        {
            return fn1.Or(fn2);
        }
    }
}
