namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// 将_field所有行的值转换为列
        /// </summary>
        /// <param name="flux"></param>
        /// <returns></returns>
        public static IFlux Pivot(this IFlux flux)
        {
            return flux.Pivot(Columns.Time, Columns.Field);
        }

        /// <summary>
        /// 行转列
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="rawKey"></param>
        /// <param name="columnKey"></param>
        /// <param name="valueColumn"></param>
        /// <returns></returns>
        public static IFlux Pivot(this IFlux flux, Columns rawKey, Columns columnKey, string valueColumn = Column.Value)
        {
            return flux.Pipe(@$"pivot(rowKey: {rawKey}, columnKey: {columnKey}, valueColumn: ""{valueColumn}"")", SingleQuotesBehavior.NoReplace);
        }
    }
}
