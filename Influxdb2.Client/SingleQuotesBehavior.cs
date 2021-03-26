namespace Influxdb2.Client
{
    /// <summary>
    /// 单引号处理方式
    /// </summary>
    public enum SingleQuotesBehavior
    {
        /// <summary>
        /// 替换为双引号
        /// </summary>
        Replce,

        /// <summary>
        /// 不替换单引号为双引号
        /// </summary>
        NoReplace
    }
}
