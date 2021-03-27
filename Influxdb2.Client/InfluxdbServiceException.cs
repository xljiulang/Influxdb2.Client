namespace Influxdb2.Client
{
    /// <summary>
    /// Influxdb服务异常
    /// </summary>
    public class InfluxdbServiceException : InfluxdbException
    {
        /// <summary>
        /// 错误内容
        /// </summary>
        public InfuxdbError Error { get; }

        /// <summary>
        /// Influxdb服务异常
        /// </summary>
        /// <param name="infuxdbError"></param>
        public InfluxdbServiceException(InfuxdbError infuxdbError)
            : base(infuxdbError.ToString())
        {
            this.Error = infuxdbError;
        }
    }
}
