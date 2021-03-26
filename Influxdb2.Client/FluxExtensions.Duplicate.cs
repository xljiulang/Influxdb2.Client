namespace Influxdb2.Client
{
    public static partial class FluxExtensions
	{
		/// <summary>
		/// 复制列为新的列
		/// </summary>
		/// <param name="flux"></param>
		/// <param name="column"></param>
		/// <param name="aliasAs"></param>
		/// <returns></returns>
		public static IFlux Duplicate(this IFlux flux, string column, string aliasAs)
		{
			return flux.Pipe($"duplicate(column: '{column}', as: '{aliasAs}')");
		}
	}
}
