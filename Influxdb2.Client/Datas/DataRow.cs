using System;
using System.Collections.Generic;
using System.Linq;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 数据行
    /// </summary>
    sealed class DataRow : Dictionary<string, string?>, IDataRow
    {
        /// <summary>
        /// 获取所有列名
        /// </summary>
        public string[] Columns => this.Keys.ToArray();

        /// <summary>
        /// 数据行
        /// </summary>
        public DataRow()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// 数据迭代器
        /// </summary>
        /// <returns></returns>
        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        /// <summary>
        /// 转换为强类型
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public TModel ToModel<TModel>() where TModel : new()
        {
            var model = new TModel();
            var descriptor = ModelDescriptor.Get(typeof(TModel));

            foreach (var property in descriptor.PropertyDescriptors)
            {
                if (this.TryGetValue(property.Name, out var value))
                {
                    property.SetValue(model, value!);
                }
            }
            return model;
        }
    }
}
