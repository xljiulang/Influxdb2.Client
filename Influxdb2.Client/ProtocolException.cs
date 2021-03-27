using System;

namespace Influxdb2.Client
{
    /// <summary>
    /// 协议异常
    /// </summary>
    public class ProtocolException : Exception
    {
        /// <summary>
        /// 协议异常
        /// </summary>
        /// <param name="message">提示消息</param>
        public ProtocolException(string message)
            : base(message)
        {
        }
    }
}
