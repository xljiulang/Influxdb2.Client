using System.Reflection;

namespace Influxdb2.Client
{
    /// <summary>
    /// 表示属性描述
    /// </summary>
    class PropertyDescriptor : Property<object, object>
    {
        public ColumnType Type { get; }

        /// <summary>
        /// 属性描述
        /// </summary>
        /// <param name="property">属性信息</param>
        public PropertyDescriptor(PropertyInfo property)
            : base(property)
        {
            var attr = property.GetCustomAttribute<ColumnAttribute>();
            if (attr != null)
            {
                this.Type = attr.Type;
                if (string.IsNullOrEmpty(attr.Name) == false)
                {
                    this.Name = attr.Name;
                }
            }
        }

        /// <summary>
        /// 获取标签值
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns> 
        public string GetTagValue(object instance)
        {
            var value = this.GetValue(instance);
            return $@"""{value}""";
        }
    }

}
