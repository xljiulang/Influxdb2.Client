namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// 总条数
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static IFlux Count(this IFlux flux, string column = Column.Value)
        {
            return flux.Pipe($"count(column: '{column}')");
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static IFlux Mean(this IFlux flux, string column = Column.Value)
        {
            return flux.Pipe($"mean(column: '{column}')");
        }

        /// <summary>
        /// 中值
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static IFlux Median(this IFlux flux, string column = Column.Value)
        {
            return flux.Pipe($"median(column: '{column}')");
        }

        /// <summary>
        /// 总值
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static IFlux Sum(this IFlux flux, string column = Column.Value)
        {
            return flux.Pipe($"sum(column: '{column}')");
        }
    }
}
