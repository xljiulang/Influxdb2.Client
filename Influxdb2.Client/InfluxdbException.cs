using System;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示Influxdb服务器异常
    /// </summary>
    public class InfluxdbException : Exception
    {
        /// <summary>
        /// 错误内容
        /// </summary>
        public InfuxdbError Error { get; }

        /// <summary>
        /// Influxdb服务器异常
        /// </summary>
        /// <param name="infuxdbError"></param>
        public InfluxdbException(InfuxdbError infuxdbError)
            : base(infuxdbError.ToString())
        {
            this.Error = infuxdbError;
        }
    }
}
