namespace Influxdb2.Client
{
    /// <summary>
    /// Infuxdb错误信息
    /// </summary>
    public class InfuxdbError
    {
        /// <summary>
        /// 业务码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public string? Op { get; set; }

        /// <summary>
        /// 错误内容
        /// </summary>
        public string? Err { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public int? Line { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Code)}: {this.Code} {nameof(Message)}: {this.Message}";
        }
    }
}
