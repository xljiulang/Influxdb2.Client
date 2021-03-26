namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class Flux
    {
        /// <summary>
        /// 将_field转换为列
        /// </summary>
        /// <param name="flux"></param>
        /// <returns></returns>
        public static IFlux Group(this IFlux flux, Columns columns, GroupMode mode = GroupMode.By)
        {
            return flux.Pipe(@$"|> group(columns:{columns}, mode:""{mode.ToString().ToLower()}"")");
        }

    }
}
