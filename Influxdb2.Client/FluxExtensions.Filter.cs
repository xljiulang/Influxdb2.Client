namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// 过滤结果
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="fnBody">过滤函数的body</param>
        /// <param name="paramName">参数名</param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public static IFlux Filter(this IFlux flux, string fnBody, string paramName = "r", SingleQuotesBehavior behavior = SingleQuotesBehavior.Replace)
        {
            return flux.Pipe($"filter(fn: ({paramName}) => {fnBody} )", behavior);
        }

        /// <summary>
        /// 过滤结果
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="fnBody">过滤函数的body</param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public static IFlux Filter(this IFlux flux, FnBody fnBody, SingleQuotesBehavior behavior = SingleQuotesBehavior.Replace)
        {
            return flux.Pipe($"filter(fn: ({fnBody.ParamName}) => {fnBody})", behavior);
        }
    }
}
