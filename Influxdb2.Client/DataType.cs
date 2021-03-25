namespace Influxdb2.Client
{
    /// <summary>
    /// Influxdb数据类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 字段
        /// 用于保存自身的值
        /// </summary>
        /// <remarks>
        /// * 一般为自身固有的属性值
        /// </remarks>
        Field = 0,

        /// <summary>
        /// 标签
        /// 用于保存关联的值
        /// </summary>
        /// <remarks>
        /// * 一般为外键的值，该值做为索引保存
        /// </remarks>
        Tag = 1,

        /// <summary>
        /// 时间点
        /// </summary>
        /// <remarks>
        /// * 一个时间点只有一条记录，相当于数据的id
        /// </remarks>
        Time = 2,
    }
}
