namespace Influxdb2.Client
{
    public static partial class FluxExtensions
	{
		public static IFlux Duplicate(this IFlux flux, string column, string aliasAs)
		{
			return flux.Pipe($"duplicate(column: '{column}', as: '{aliasAs}')");
		}
	}
}
