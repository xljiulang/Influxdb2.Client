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
			return flux.Pipe($"|> pivot(rowKey:['_time'], columnKey:['_field'], valueColumn:'_value')");
		}
	}
}
