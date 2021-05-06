namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static IFlux Sort(this IFlux flux, bool desc = false)
        {
            return flux.Sort(Columns.Value, desc);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="columns"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static IFlux Sort(this IFlux flux, Columns columns, bool desc = false)
        {
            return flux.Pipe($"sort(columns:{columns}, desc:{desc.ToString().ToLower()})", SingleQuotesBehavior.NoReplace);
        }
    }
}
