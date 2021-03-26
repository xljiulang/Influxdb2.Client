using System;
using System.Text;

namespace Influxdb2.Client
{
    /// <summary>
    /// 过滤函数
    /// </summary>
    public class FilterFn
    {
        private readonly StringBuilder builder = new StringBuilder();

        /// <summary>
        /// 返回r参数名的过滤函数
        /// </summary>
        public static FilterFn R => new FilterFn("r");

        /// <summary>
        /// 获取参数名
        /// </summary>
        public string ParamName { get; }

        /// <summary>
        /// 过滤函数
        /// </summary>
        /// <param name="paramName">参数名</param>
        private FilterFn(string paramName)
        {
            this.ParamName = paramName;
        }

        /// <summary>
        /// and计算
        /// </summary>
        /// <param name="fn"></param>
        /// <returns></returns>
        public FilterFn And(FilterFn fn)
        {
            if (this.ParamName != fn.ParamName)
            {
                throw new ArgumentException($"{nameof(fn)}的{nameof(ParamName)}必须为{ParamName}");
            }
            this.And();
            this.builder.Append($"({fn})");
            return this;
        }

        /// <summary>
        /// and关键字
        /// </summary>
        /// <returns></returns>
        public FilterFn And()
        {
            this.builder.Append(" and ");
            return this;
        }

        /// <summary>
        /// or计算
        /// </summary>
        /// <param name="fn"></param>
        /// <returns></returns> 
        public FilterFn Or(FilterFn fn)
        {
            if (this.ParamName != fn.ParamName)
            {
                throw new ArgumentException($"{nameof(fn)}的{nameof(ParamName)}必须为{ParamName}");
            }

            this.Or();
            this.builder.Append($"({fn})");
            return this;
        }

        /// <summary>
        /// or关键字
        /// </summary>
        /// <returns></returns>
        public FilterFn Or()
        {
            this.builder.Append(" or ");
            return this;
        }

        /// <summary>
        /// 匹配Measurement
        /// </summary>
        /// <param name="measurementName">名</param>
        /// <returns></returns>
        public FilterFn MatchMeasurement(string measurementName)
        {
            return this.WhenColumn("_measurement", "==", measurementName);
        }

        /// <summary>
        /// 匹配_field列
        /// </summary>
        /// <param name="fieldName">名</param>
        /// <returns></returns>
        public FilterFn MatchField(string fieldName)
        {
            return this.WhenColumn("_field", "==", fieldName);
        }

        /// <summary>
        /// 匹配标签
        /// </summary>
        /// <param name="tagName">名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public FilterFn MatchTag(string tagName, string value)
        {
            return this.WhenColumn(tagName, "==", value);
        }

        /// <summary>
        /// 比较列的值
        /// </summary> 
        /// <param name="columnName">列名</param>
        /// <param name="op">比较符号</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public FilterFn WhenColumn(string columnName, string op, object value)
        {
            if (value is string stringValue)
            {
                builder.Append(@$"{this.ParamName}.{columnName} {op} ""{stringValue}""");
            }
            else
            {
                builder.Append(@$"{this.ParamName}.{columnName} {op} {value}");
            }
            return this;
        }


        /// <summary>
        /// 追加body
        /// </summary>
        /// <param name="fnBody"></param>
        /// <returns></returns>
        public FilterFn Then(string fnBody)
        {
            builder.Append(" ").Append(fnBody);
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
        public static FilterFn operator &(FilterFn fn1, FilterFn fn2)
        {
            return fn1.And(fn2);
        }

        /// <summary>
        /// Or
        /// </summary>
        /// <param name="fn1"></param>
        /// <param name="fn2"></param>
        /// <returns></returns>
        public static FilterFn operator |(FilterFn fn1, FilterFn fn2)
        {
            return fn1.Or(fn2);
        }
    }
}
