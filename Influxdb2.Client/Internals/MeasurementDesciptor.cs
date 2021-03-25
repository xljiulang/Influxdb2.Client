using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Influxdb2.Client
{
    /// <summary>
    /// Measurement描述
    /// </summary>
    sealed class MeasurementDesciptor
    {
        /// <summary>
        /// 获取名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 获取所有字段
        /// </summary>
        public PropertyDescriptor[] Fields { get; }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        public PropertyDescriptor[] Tags { get; }

        /// <summary>
        /// 获取时间点
        /// </summary>
        public PropertyDescriptor? Time { get; }


        /// <summary>
        /// Measurement描述
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentException"></exception>
        private MeasurementDesciptor(Type type)
        {
            var properties = type.GetProperties()
                .Where(item => item.CanRead && item.CanWrite && item.IsDefined(typeof(InfluxdbDataTypeAttribute)))
                .Select(p => new PropertyDescriptor(p))
                .ToArray();

            var times = properties.Where(item => item.DataType == DataType.Time).ToArray();
            if (times.Length > 1)
            {
                throw new ArgumentException($"{type}至多只能声明一个{nameof(DataType.Time)}属性");
            }

            var fields = properties.Where(item => item.DataType == DataType.Field).ToArray();
            if (fields.Length == 0)
            {
                throw new ArgumentException($"{type}至少声明一个{nameof(DataType.Field)}属性");
            }

            this.Time = times.FirstOrDefault();
            this.Fields = fields;
            this.Tags = properties.Where(item => item.DataType == DataType.Tag).ToArray();
            this.Name = type.Name;
        }

        /// <summary>
        /// Measurement描述缓存
        /// </summary>
        private static readonly ConcurrentDictionary<Type, MeasurementDesciptor> cache = new ConcurrentDictionary<Type, MeasurementDesciptor>();

        /// <summary>
        /// 获取Measurement描述
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static MeasurementDesciptor Get(Type type)
        {
            return cache.GetOrAdd(type, t => new MeasurementDesciptor(t));
        }
    }
}
