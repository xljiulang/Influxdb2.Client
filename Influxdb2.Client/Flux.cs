using System;
using System.Text;

namespace Influxdb2.Client
{
    /// <summary>
    /// 提供IFlux对象的创建
    /// </summary>
    public static partial class Flux
    {
        /// <summary>
        /// From语句
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public static IFlux From(string bucket)
        {
            return new FluxImpl().Pipe($"from(bucket:'{bucket}')");
        }

        private class FluxImpl : IFlux
        {
            private readonly StringBuilder builder = new StringBuilder();

            /// <summary>
            /// 添加管道
            /// </summary>
            /// <param name="value">值</param>
            /// <param name="replceSingleQuotes">是否替换单引号为双引号</param>
            /// <returns></returns>
            public IFlux Pipe(string value, bool replceSingleQuotes = true)
            { 
                if (value == null || replceSingleQuotes == false)
                {
                    this.builder.AppendLine(value);
                }
                else
                {
                    var span = value.ToCharArray().AsSpan();
                    for (var i = 0; i < span.Length; i++)
                    {
                        if (span[i] == '\'')
                        {
                            span[i] = '"';
                        }
                    }
                    this.builder.AppendLine(span.ToString());
                }

                return this;
            }

            public override string ToString()
            {
                return this.builder.ToString();
            }
        }
    }
}
