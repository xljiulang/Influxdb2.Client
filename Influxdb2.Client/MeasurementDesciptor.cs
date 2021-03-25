using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Influxdb2.Client
{
    /// <summary>
    /// Measurement描述
    /// </summary>
    class MeasurementDesciptor
    {
        /// <summary>
        /// 获取名称
        /// </summary>
        public string Name { get; }

        public PropertyDescriptor[] Fields { get; }

        public PropertyDescriptor[] Tags { get; }

        public PropertyDescriptor? Time { get; }


        /// <summary>
        /// Measurement描述
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentException"></exception>
        private MeasurementDesciptor(Type type)
        {
            var properties = type.GetProperties()
                .Where(item => item.CanRead && item.CanWrite)
                .Select(p => new PropertyDescriptor(p))
                .ToArray();

            var times = properties.Where(item => item.Type == ColumnType.Time).ToArray();
            if (times.Length > 1)
            {
                throw new ArgumentException($"{type}只能声明一次ColumnType.Time字段");
            }

            var fields = properties.Where(item => item.Type == ColumnType.Field).ToArray();
            if (fields.Length == 0)
            {
                throw new ArgumentException($"{type}至少声明一个ColumnType.Field字段");
            }

            this.Time = times.FirstOrDefault();
            this.Fields = fields;
            this.Tags = properties.Where(item => item.Type == ColumnType.Tag).ToArray();

            var attr = type.GetCustomAttribute<MeasurementAttribute>();
            this.Name = attr == null || string.IsNullOrEmpty(attr.Name) ? type.Name : attr.Name;
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
