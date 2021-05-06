namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// 使用指定值填写column为null值
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="value"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static IFlux Fill(this IFlux flux, object value, string column = Column.Value)
        {
            return value is string
                ? flux.Pipe($"fill(column: '{column}', value: '{value}')")
                : flux.Pipe($"fill(column: '{column}', value: {value})");

        }
    }
}
