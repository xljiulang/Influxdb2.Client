using System;

namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// 时间范围
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static IFlux Range(this IFlux flux, string start)
        {
            return flux.Pipe($"range(start:{start})", SingleQuotesBehavior.NoReplace);
        }

        /// <summary>
        /// 时间范围
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static IFlux Range(this IFlux flux, DateTimeOffset start)
        {
            return flux.Pipe($"range(start:{start:O})", SingleQuotesBehavior.NoReplace);
        }


        /// <summary>
        /// 时间范围
        /// </summary>
        /// <remarks>* start小于stop</remarks>
        /// <param name="flux"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static IFlux Range(this IFlux flux, string start, string stop)
        {
            return flux.Pipe($"range(start:{start}, stop:{stop})", SingleQuotesBehavior.NoReplace);
        }


        /// <summary>
        /// 时间范围
        /// </summary>
        /// <remarks>* start小于stop</remarks>
        /// <param name="flux"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static IFlux Range(this IFlux flux, string start, DateTimeOffset stop)
        {
            return flux.Pipe($"range(start:{start}, stop:{stop:O})", SingleQuotesBehavior.NoReplace);
        }



        /// <summary>
        /// 时间范围
        /// </summary>
        /// <remarks>* start小于stop</remarks>
        /// <param name="flux"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static IFlux Range(this IFlux flux, DateTimeOffset start, string stop)
        {
            return flux.Pipe($"range(start:{start:O}, stop:{stop})", SingleQuotesBehavior.NoReplace);
        }


        /// <summary>
        /// 时间范围
        /// </summary>
        /// <remarks>* start小于stop</remarks>
        /// <param name="flux"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static IFlux Range(this IFlux flux, DateTimeOffset start, DateTimeOffset stop)
        {
            return flux.Pipe($"range(start:{start:O}, stop:{stop:O})", SingleQuotesBehavior.NoReplace);
        }

    }
}
