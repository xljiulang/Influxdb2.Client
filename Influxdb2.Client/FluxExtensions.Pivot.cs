namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// 将_field转换为列
        /// </summary>
        /// <param name="flux"></param>
        /// <returns></returns>
        public static IFlux Pivot(this IFlux flux)
        {
            return flux.Pivot(Columns.TimeColumn, Columns.FieldColumn);
        }

        /// <summary>
        /// 将_field转换为列
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="rawKey"></param>
        /// <param name="columnKey"></param>
        /// <param name="valueColumn"></param>
        /// <returns></returns>
        public static IFlux Pivot(this IFlux flux, Columns rawKey, Columns columnKey, string valueColumn = ValueColumnName)
        {
            return flux.Pipe(@$"pivot(rowKey:{rawKey}, columnKey:{columnKey}, valueColumn:""{valueColumn}"")", SingleQuotesBehavior.NoReplace);
        }
    }
}
