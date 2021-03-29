namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// 移除指定的查询列
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static IFlux Drop(this IFlux flux, Columns columns)
        {
            return flux.Pipe($"drop(columns: {columns})", SingleQuotesBehavior.NoReplace);
        } 
    }
}
