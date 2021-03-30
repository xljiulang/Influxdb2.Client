using System.Linq;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 表示模型描述器
    /// </summary>
    static class ModelDescriptor<T>
    {
        /// <summary>
        /// 获取所属性描述
        /// </summary>
        public static ModelPropertyDescriptor<T>[] PropertyDescriptors { get; }

        /// <summary>
        /// 模型描述器
        /// </summary> 
        static ModelDescriptor()
        {
            PropertyDescriptors = typeof(T)
                .GetProperties()
                .Where(item => item.CanWrite)
                .Select(item => new ModelPropertyDescriptor<T>(item))
                .ToArray();
        }
    }
}
