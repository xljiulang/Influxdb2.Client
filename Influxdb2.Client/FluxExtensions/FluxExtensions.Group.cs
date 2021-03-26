namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="columns"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IFlux Group(this IFlux flux, Columns columns, GroupMode mode = GroupMode.By)
        {
            return flux.Pipe(@$"group(columns:{columns}, mode:""{mode.ToString().ToLower()}"")", SingleQuotesBehavior.NoReplace);
        }

    }
}
