namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// 时间区间聚合
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="every">时间区间 1h等</param>
        /// <param name="fn">聚合函数</param>
        /// <param name="column">要聚合的列</param>
        /// <param name="timeSrc"></param>
        /// <param name="timeDst"></param>
        /// <param name="createEmpty"></param>
        /// <returns></returns>
        public static IFlux AggregateWindow(this IFlux flux, string every, string fn, string column = Column.Value, string timeSrc = Column.Stop, string timeDst = Column.Time, bool createEmpty = true)
        {
            return flux.Pipe($"aggregateWindow(every: {every}, fn: {fn}, column: '{column}', timeSrc: '{timeSrc}', timeDst: '{timeDst}', createEmpty: {createEmpty.ToString().ToLower()})");
        }
    }
}
