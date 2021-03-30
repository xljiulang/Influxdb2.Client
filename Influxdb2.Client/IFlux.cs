namespace Influxdb2.Client
{
    /// <summary>
    /// 表示Flux查询对象
    /// </summary>
    public interface IFlux
    {
        /// <summary>
        /// 添加管道
        /// </summary>
        /// <param name="content">内容，不包含管道符号 </param>
        /// <param name="behavior">单引号处理方式</param>
        /// <returns></returns>
        IFlux Pipe(string content, SingleQuotesBehavior behavior = SingleQuotesBehavior.Replace);
    }
}
