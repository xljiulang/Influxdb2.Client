namespace Influxdb2.Client
{
    /// <summary>
    /// Influxdb列类型
    /// </summary>
    public enum ColumnType
    {
        /// <summary>
        /// 标签
        /// 文本类型
        /// </summary>
        /// <remarks>
        /// * 标签用于过滤，该值做为索引保存
        /// </remarks>
        Tag = 1,

        /// <summary>
        /// 字段
        /// 整数、浮点或文本
        /// </summary>
        /// <remarks>
        /// * 数据点在至少包含一个field
        /// </remarks>
        Field = 0,

        /// <summary>
        /// unix纳秒级的时间戳
        /// </summary>
        /// <remarks> 
        /// </remarks>
        Timestamp = 2,
    }
}
