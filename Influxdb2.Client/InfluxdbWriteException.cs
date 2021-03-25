namespace Influxdb2.Client
{
    /// <summary>
    /// Influxdb写入异常
    /// </summary>
    public class InfluxdbWriteException : InfluxdbException
    {
        /// <summary>
        /// 错误内容
        /// </summary>
        public WriteError Error { get; }

        /// <summary>
        /// Influxdb写入异常
        /// </summary>
        /// <param name="writeError"></param>
        public InfluxdbWriteException(WriteError writeError)
            : base(writeError.ToString())
        {
            Error = writeError;
        }
    }
}
