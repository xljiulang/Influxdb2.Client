namespace Influxdb2.Client
{
    public static partial class FluxExtensions
	{
		/// <summary>
		/// 选择时间小时范围
		/// </summary>
		/// <param name="flux"></param>
		/// <param name="start"></param>
		/// <param name="stop"></param>
		/// <param name="timeColumn"></param>
		/// <returns></returns>
		public static IFlux HourSelection(this IFlux flux, int start, int stop, string timeColumn = Column.Time)
		{
			return flux.Pipe($"hourSelection(start: {start}, stop: {stop}, timeColumn: '{timeColumn}')");
		}
	}
}
