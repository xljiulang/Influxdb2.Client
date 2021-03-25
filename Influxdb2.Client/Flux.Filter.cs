namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class Flux
    {
        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="fnBody">过滤函数的body</param>
        /// <param name="paramName">参数名</param>
        /// <returns></returns>
        public static IFlux Filter(this IFlux flux, string fnBody, string paramName = "r", bool replceSingleQuotes = true)
        {
            return flux.Pipe($"|> filter(fn: ({paramName}) => {fnBody} )", replceSingleQuotes);
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="fn">函数</param> 
        /// <returns></returns>
        public static IFlux Filter(this IFlux flux, FilterFn fn)
        {
            return flux.Pipe($"|> filter(fn: ({fn.ParamName}) => {fn})", false);
        }
    }
}
