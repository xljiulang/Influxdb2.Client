using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Influxdb2.Client.Datas
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
        public MeasurementPropertyDescriptor[] Fields { get; }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        public MeasurementPropertyDescriptor[] Tags { get; }

        /// <summary>
        /// 获取时间点
        /// </summary>
        public MeasurementPropertyDescriptor? Time { get; }


        /// <summary>
        /// Measurement描述
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentException"></exception>
        private MeasurementDesciptor(Type type)
        {
            var properties = type.GetProperties()
                .Where(item => item.CanRead && item.IsDefined(typeof(ColumnTypeAttribute)))
                .Select(p => new MeasurementPropertyDescriptor(p))
                .ToArray();

            var times = properties.Where(item => item.ColumnType == ColumnType.Time).ToArray();
            if (times.Length > 1)
            {
                throw new ArgumentException($"{type}至多只能声明一个{nameof(ColumnType.Time)}列的属性");
            }

            var fields = properties.Where(item => item.ColumnType == ColumnType.Field).ToArray();
            if (fields.Length == 0)
            {
                throw new ArgumentException($"{type}至少声明一个{nameof(ColumnType.Field)}列的属性");
            }

            this.Time = times.FirstOrDefault();
            this.Fields = fields;
            this.Tags = properties.Where(item => item.ColumnType == ColumnType.Tag).ToArray();
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
