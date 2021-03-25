using System.Text;

namespace Influxdb2.Client
{
    public class FilterFn
    {
        private readonly StringBuilder builder = new StringBuilder();

        public static FilterFn R => new FilterFn("r");

        public string ParamName { get; }

        private FilterFn(string paramName)
        {
            this.ParamName = paramName;
        }

        public FilterFn And()
        {
            this.builder.Append(" and ");
            return this;
        }

        public FilterFn And(FilterFn fn)
        {
            this.And();
            this.builder.Append($"({fn})");
            return this;
        }
        public FilterFn Or()
        {
            this.builder.Append($" or ");
            return this;
        }

        public FilterFn Or(FilterFn fn)
        {
            this.Or();
            this.builder.Append($"({fn})");
            return this;
        }

        /// <summary>
        /// 选择Measurement
        /// </summary>
        /// <param name="measurement">名</param>
        /// <returns></returns>
        public FilterFn MatchMeasurement(string measurement)
        {
            builder.Append(@$"{this.ParamName}._measurement == ""{measurement}""");
            return this;
        }

        /// <summary>
        /// 选择字段
        /// </summary>
        /// <param name="field">名</param>
        /// <returns></returns>
        public FilterFn MatchField(string field)
        {
            builder.Append(@$"{this.ParamName}._field == ""{field}""");
            return this;
        }

        /// <summary>
        /// 字段值匹配
        /// </summary> 
        /// <param name="op">比较符号</param>
        /// <param name="tagValue">值</param>
        /// <returns></returns>
        public FilterFn MatchValue(string op, object value)
        {
            if (value is string stringValue)
            {
                builder.Append(@$"{this.ParamName}._value {op} ""{stringValue}""");
            }
            else
            {
                builder.Append(@$"{this.ParamName}._value {op} {value}");
            }
            return this;
        }

        /// <summary>
        /// 匹配pivot产生的列
        /// </summary> 
        /// <param name="name">列名</param>
        /// <param name="op">比较符号</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public FilterFn MatchColumn(string name, string op, object value)
        {
            if (value is string stringValue)
            {
                builder.Append(@$"{this.ParamName}.{name} {op} ""{stringValue}""");
            }
            else
            {
                builder.Append(@$"{this.ParamName}.{name} {op} {value}");
            }
            return this;
        }

        /// <summary>
        /// 标签匹配
        /// </summary>
        /// <param name="name">名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public FilterFn MatchTag(string name, string value)
        {
            builder.Append(@$"{this.ParamName}.{name} == ""{value}""");
            return this;
        }

        public override string ToString()
        {
            return this.builder.ToString();
        }

        public static FilterFn operator &(FilterFn fn1, FilterFn fn2)
        {
            return fn1.And(fn2);
        }

        public static FilterFn operator |(FilterFn fn1, FilterFn fn2)
        {
            return fn1.Or(fn2);
        }
    }
}
