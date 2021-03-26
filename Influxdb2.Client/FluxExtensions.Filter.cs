namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="fnBody">过滤函数的body</param>
        /// <param name="paramName">参数名</param>
        /// <returns></returns>
        public static IFlux Filter(this IFlux flux, string fnBody, string paramName = "r", SingleQuotesBehavior behavior = SingleQuotesBehavior.Replace)
        {
            return flux.Pipe($"filter(fn: ({paramName}) => {fnBody} )", behavior);
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="fnBody">过滤函数的body</param> 
        /// <returns></returns>
        public static IFlux Filter(this IFlux flux, FnBody fnBody, SingleQuotesBehavior behavior = SingleQuotesBehavior.Replace)
        {
            return flux.Pipe($"filter(fn: ({fnBody.ParamName}) => {fnBody})", behavior);
        }
    }
}
