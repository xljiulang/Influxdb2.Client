namespace Influxdb2.Client
{
    /// <summary>
    /// Influxdb的字段异常
    /// </summary>
    public class InfluxdbFieldException : InfluxdbException
    {
        /// <summary>
        /// 获取字段名
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Influxdb的字段异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fieldName"></param>
        public InfluxdbFieldException(string message, string fieldName)
            : base(message)
        {
            this.FieldName = fieldName;
        }
    }
}
