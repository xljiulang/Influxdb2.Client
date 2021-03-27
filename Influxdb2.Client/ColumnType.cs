namespace Influxdb2.Client
{
    /// <summary>
    /// Influxdb列类型
    /// </summary>
    public enum ColumnType
    {
        /// <summary>
        /// 标签     
        /// </summary>
        /// <remarks>
        /// <para>* 标签的值不能为空</para>
        /// <para>* 标签的值以文本形式进行索引存储</para>
        /// <para>* 标签适合用于做过滤条件</para>
        /// </remarks>
        Tag = 1,

        /// <summary> 
        /// 字段
        /// </summary>
        /// <remarks>
        /// <para>* 逻辑、整数、浮点或文本类型</para>
        /// <para>* 非文本类型不可空</para>
        /// <para>* 文本类型换行符自动被过滤</para>
        /// <para>* 实体的属性至少包含一个字段</para>
        /// </remarks>
        Field = 0,

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <remarks> 
        /// <para>* 支持修饰DateTimeOffset或DateTime类型</para>
        /// <para>* 当转换为LineProtocol时映射为unix纳秒级的时间戳</para>
        /// <para>* 当转换为Entity或Model时对应_time的列名</para>
        /// </remarks>
        Timestamp = 2,
    }
}
