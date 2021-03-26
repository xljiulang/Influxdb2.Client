using System;
using System.Text;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示Flux对象
    /// </summary>
    public sealed class Flux : IFlux
    {
        /// <summary>
        /// From语句
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public static IFlux From(string bucket)
        {
            var flux = new Flux();
            flux.builder.AppendLine(@$"from(bucket:""{bucket}"")");
            return flux;
        }

        /// <summary>
        /// union查询
        /// </summary>
        /// <param name="table1"></param>
        /// <param name="table2"></param>
        public static IFlux Union(IFlux table1, IFlux table2)
        {
            var flux = new Flux();
            flux.builder.AppendLine($"union(tables: [{table1}, {table2}])");
            return flux;
        }

        private readonly StringBuilder builder = new StringBuilder();

        /// <summary>
        /// Flux对象
        /// </summary>
        private Flux()
        {
        }

        /// <summary>
        /// 添加管道
        /// </summary>
        /// <param name="value">值，不包含管道符号 </param>
        /// <param name="behavior">单引号处理方式</param>
        /// <returns></returns>
        public IFlux Pipe(string value, SingleQuotesBehavior behavior)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }

            if (behavior == SingleQuotesBehavior.Replce)
            {
                var span = value.ToCharArray().AsSpan();
                for (var i = 0; i < span.Length; i++)
                {
                    if (span[i] == '\'')
                    {
                        span[i] = '"';
                    }

                }
                value = span.ToString();
            }

            this.builder.Append("|> ").AppendLine(value);
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
    }
}
