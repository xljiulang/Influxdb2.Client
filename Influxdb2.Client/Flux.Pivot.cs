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
        public static IFlux Pivot(this IFlux flux)
        {
            return flux.Pivot(Columns.Values("_time"), Columns.Values("_field"));
        }

        /// <summary>
        /// 将_field转换为列
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="rawKey"></param>
        /// <param name="columnKey"></param>
        /// <param name="valueColumn"></param>
        /// <returns></returns>
        public static IFlux Pivot(this IFlux flux, Columns rawKey, Columns columnKey, string valueColumn = "_value")
        {
            return flux.Pipe(@$"|> pivot(rowKey:[{rawKey}], columnKey:[{columnKey}], valueColumn:""{valueColumn}"")", false);
        }
    }
}
