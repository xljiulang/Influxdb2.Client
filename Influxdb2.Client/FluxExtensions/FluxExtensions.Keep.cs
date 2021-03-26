namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    { 
        /// <summary>
        /// 保保留指定的查询列
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static IFlux Keep(this IFlux flux, Columns columns)
        {
            return flux.Pipe($"keep(columns:{columns})", SingleQuotesBehavior.NoReplace);
        }
    }
}
