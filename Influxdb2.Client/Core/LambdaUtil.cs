using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Influxdb2.Client.Core
{
    /// <summary>
    /// 表示式委托工具类
    /// </summary>
    static class LambdaUtil
    {
        /// <summary>
        /// 创建属性的设置委托
        /// </summary>
        /// <typeparam name="TDeclaring"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="property">属性</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static Action<TDeclaring, TProperty> CreateSetAction<TDeclaring, TProperty>(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            // (TDeclaring instance, TProperty value) => 
            //    ((declaringType)instance).Set_XXX( (propertyType)value )

            var paramInstance = Expression.Parameter(typeof(TDeclaring));
            var paramValue = Expression.Parameter(typeof(TProperty));

            var bodyInstance = (Expression)paramInstance;
            var bodyValue = (Expression)paramValue;

            if (typeof(TDeclaring) != property.DeclaringType)
            {
                bodyInstance = Expression.Convert(bodyInstance, property.DeclaringType);
            }

            if (typeof(TProperty) != property.PropertyType)
            {
                bodyValue = Expression.Convert(bodyValue, property.PropertyType);
            }

            var bodyCall = Expression.Call(bodyInstance, property.GetSetMethod(), bodyValue);
            return Expression.Lambda<Action<TDeclaring, TProperty>>(bodyCall, paramInstance, paramValue).Compile();
        }


        /// <summary>
        /// 创建属性的获取委托
        /// </summary>
        /// <typeparam name="TDeclaring"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="property">属性</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static Func<TDeclaring, TProperty> CreateGetFunc<TDeclaring, TProperty>(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (property.DeclaringType == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return CreateGetFunc<TDeclaring, TProperty>(property.DeclaringType, property.Name, property.PropertyType);
        }

        /// <summary>
        /// 创建属性的获取委托
        /// </summary>
        /// <typeparam name="TDeclaring"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="declaringType">实例的类型</param>
        /// <param name="propertyName">属性的名称</param>
        /// <param name="propertyType">属性的类型</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static Func<TDeclaring, TProperty> CreateGetFunc<TDeclaring, TProperty>(Type declaringType, string propertyName, Type? propertyType = null)
        {
            if (declaringType == null)
            {
                throw new ArgumentNullException(nameof(declaringType));
            }

            if (string.IsNullOrEmpty(propertyName) == true)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            // (TDeclaring instance) => (propertyType)((declaringType)instance).propertyName

            var paramInstance = Expression.Parameter(typeof(TDeclaring));

            var bodyInstance = (Expression)paramInstance;
            if (typeof(TDeclaring) != declaringType)
            {
                bodyInstance = Expression.Convert(bodyInstance, declaringType);
            }

            var bodyProperty = (Expression)Expression.Property(bodyInstance, propertyName);
            if (typeof(TProperty) != propertyType)
            {
                bodyProperty = Expression.Convert(bodyProperty, typeof(TProperty));
            }

            return Expression.Lambda<Func<TDeclaring, TProperty>>(bodyProperty, paramInstance).Compile();
        }
    }
}