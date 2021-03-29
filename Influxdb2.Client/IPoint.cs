namespace Influxdb2.Client
{
    /// <summary>
    /// 数据点
    /// </summary>
    public interface IPoint
    {
        /// <summary>
        /// 获取行文本协议内容
        /// </summary>
        string LineProtocol { get; }
    }
}
