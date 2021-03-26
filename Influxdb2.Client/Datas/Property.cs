using System;
using System.Reflection;
using WebApiClientCore.Internals;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 表示属性
    /// </summary>
    /// <typeparam name="TDeclaring">定义属性的类型</typeparam>
    /// <typeparam name="TProperty">属性类型</typeparam>
    class Property<TDeclaring, TProperty>
    {
        /// <summary>
        /// 获取器
        /// </summary>
        private readonly Func<TDeclaring, TProperty>? geter;

        /// <summary>
        /// 设置器
        /// </summary>
        private readonly Action<TDeclaring, TProperty>? seter;

        /// <summary>
        /// 获取属性名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 获取属性信息
        /// </summary>
        public PropertyInfo Info { get; }

        /// <summary>
        /// 属性
        /// </summary>
        /// <param name="property">属性信息</param>
        public Property(PropertyInfo property)
        {
            this.Name = property.Name;
            this.Info = property;

            if (property.CanRead == true)
            {
                this.geter = LambdaUtil.CreateGetFunc<TDeclaring, TProperty>(property);
            }

            if (property.CanWrite == true)
            {
                this.seter = LambdaUtil.CreateSetAction<TDeclaring, TProperty>(property);
            }
        }

        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <param name="instance">实例</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <returns></returns>
        public TProperty GetValue(TDeclaring instance)
        {
            return this.geter == null
                ? throw new NotSupportedException()
                : this.geter.Invoke(instance);
        }

        /// <summary>
        /// 设置属性的值
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="value">值</param>
        public virtual void SetValue(TDeclaring instance, TProperty value)
        {
            this.seter?.Invoke(instance, value);
        }
    }
}