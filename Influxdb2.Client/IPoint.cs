namespace Influxdb2.Client
{
    /// <summary>
    /// 表示一个数据点
    /// </summary>
    public interface IPoint
    {
        /// <summary>
        /// 转换为LineProtocol
        /// </summary>
        /// <returns></returns>
        string ToLineProtocol();
    }
}
