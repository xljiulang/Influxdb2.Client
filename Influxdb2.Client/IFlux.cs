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
        /// <param name="value">值</param>
        /// <param name="replceSingleQuotes">是否替换单引号为双引号</param>
        /// <returns></returns>
        IFlux Pipe(string value, bool replceSingleQuotes = true);
    }
}
