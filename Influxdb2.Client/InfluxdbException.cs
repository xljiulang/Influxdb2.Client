using System;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示Influxdb异常
    /// </summary>
    public class InfluxdbException : Exception
    {
        /// <summary>
        /// Influxdb异常
        /// </summary>
        /// <param name="message"></param>
        public InfluxdbException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Influxdb异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InfluxdbException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
