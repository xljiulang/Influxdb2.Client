namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// limit
        /// </summary>
        /// <param name="flux"></param>
        /// <returns></returns>
        public static IFlux Limit(this IFlux flux, int n, int offset = 0)
        {
            return flux.Pipe($"limit(n: {n}, offset: {offset})", SingleQuotesBehavior.NoReplace);
        }
    }
}
