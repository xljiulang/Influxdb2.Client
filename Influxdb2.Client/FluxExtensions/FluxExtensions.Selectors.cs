namespace Influxdb2.Client
{
    /// <summary>
    /// Flux扩展
    /// </summary>
    public static partial class FluxExtensions
    {
        /// <summary>
        /// 保留最后n条 
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IFlux Bottom(this IFlux flux, int n)
        {
            return flux.Bottom(n, Columns.Value);
        }

        /// <summary>
        /// 保留最后n条
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="n"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static IFlux Bottom(this IFlux flux, int n, Columns columns)
        {
            return flux.Pipe($"bottom(n: {n}, columns: {columns})", SingleQuotesBehavior.NoReplace);
        }


        /// <summary>
        /// 前n条 
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="n"></param>
        /// <param name="columns">排序列</param>
        /// <returns></returns>
        public static IFlux Top(this IFlux flux, int n)
        {
            return flux.Top(n, Columns.Value);
        }

        /// <summary>
        /// 前n条 
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="n"></param>
        /// <param name="columns">排序列</param>
        /// <returns></returns>
        public static IFlux Top(this IFlux flux, int n, Columns columns)
        {
            return flux.Pipe($"top(n: {n}, columns: {columns})", SingleQuotesBehavior.NoReplace);
        }

        /// <summary>
        /// 取出不是null的第一条
        /// </summary>
        /// <param name="flux"></param>
        /// <returns></returns>
        public static IFlux First(this IFlux flux)
        {
            return flux.Pipe($"first()", SingleQuotesBehavior.NoReplace);
        }

        /// <summary>
        /// 取出不是null的最后一条
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="column">Column used to verify the existence of a value</param>
        /// <returns></returns>
        public static IFlux Last(this IFlux flux, string column = Column.Value)
        {
            return flux.Pipe($"last(column: '{column}')");
        }


        /// <summary>
        /// 最大
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="column">The column to use to calculate the maximum value</param>
        /// <returns></returns>
        public static IFlux Max(this IFlux flux, string column = Column.Value)
        {
            return flux.Pipe($"max(column: '{column}')");
        }

        /// <summary>
        /// 最小
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static IFlux Min(this IFlux flux, string column = Column.Value)
        {
            return flux.Pipe($"min(column: '{column}')");
        }

        /// <summary>
        /// 唯一值
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static IFlux Unique(this IFlux flux, string column = Column.Value)
        {
            return flux.Pipe($"unique(column: '{column}')");
        }

        /// <summary>
        /// 消除重复
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="column">Column on which to track unique values</param>
        /// <returns></returns>
        public static IFlux Distinct(this IFlux flux, string column = Column.Value)
        {
            return flux.Pipe($"distinct(column: '{column}')");
        }
    }
}
