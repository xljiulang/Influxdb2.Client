using System;
using System.Text;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示Flux对象
    /// </summary>
    public class Flux : IFlux
    {
        /// <summary>
        /// From语句
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public static IFlux From(string bucket)
        {
            return new Flux(bucket);
        }


        private readonly StringBuilder builder = new StringBuilder();

        /// <summary>
        /// Flux对象
        /// </summary>
        /// <param name="bucket"></param>
        private Flux(string bucket)
        {
            this.builder.AppendLine(@$"from(bucket:""{bucket}"")");
        }

        /// <summary>
        /// 添加管道
        /// </summary>
        /// <param name="value">值，不包含管道符号 </param>
        /// <param name="behavior">单引号处理方式</param>
        /// <returns></returns>
        public IFlux Pipe(string value, SingleQuotesBehavior behavior)
        {
            var pipe = "|> ";
            if (value == null || behavior == SingleQuotesBehavior.NoReplace)
            {
                this.builder.Append(pipe).AppendLine(value);
                return this;
            }

            var span = value.ToCharArray().AsSpan();
            for (var i = 0; i < span.Length; i++)
            {
                if (span[i] == '\'')
                {
                    span[i] = '"';
                }
            }

            this.builder.Append(pipe).AppendLine(span.ToString());
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
