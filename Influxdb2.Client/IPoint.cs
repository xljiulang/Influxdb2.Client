namespace Influxdb2.Client
{
    /// <summary>
    /// 数据点
    /// </summary>
    public interface IPoint
    {
        /// <summary>
        /// 写入行文本协议内容
        /// </summary>
        /// <param name="writer">写入器 </param>
        void WriteLineProtocol(ILineProtocolWriter writer);
    }
}
