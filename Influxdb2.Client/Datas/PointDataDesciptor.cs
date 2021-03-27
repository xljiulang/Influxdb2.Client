using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 数据点描述
    /// </summary>
    sealed class PointDataDesciptor
    {
        /// <summary>
        /// 描述缓存
        /// </summary>
        private static readonly ConcurrentDictionary<Type, PointDataDesciptor> cache = new();

        /// <summary>
        /// 获取数据点描述
        /// </summary>
        /// <param name="entityType"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static PointDataDesciptor Get(Type entityType)
        {
            return cache.GetOrAdd(entityType, t => new PointDataDesciptor(t));
        }


        /// <summary>
        /// 获取Measurement
        /// </summary>
        public string Measurement { get; }

        /// <summary>
        /// 获取所有字段
        /// </summary>
        public PointDataPropertyDescriptor[] Fields { get; }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        public PointDataPropertyDescriptor[] Tags { get; }

        /// <summary>
        /// 获取时间点
        /// </summary>
        public PointDataPropertyDescriptor? Timestamp { get; }


        /// <summary>
        /// 数据点描述
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <exception cref="ArgumentException"></exception>
        private PointDataDesciptor(Type entityType)
        {
            var properties = entityType.GetProperties()
                .Where(item => item.CanRead && item.IsDefined(typeof(ColumnTypeAttribute)))
                .Select(p => new PointDataPropertyDescriptor(p))
                .OrderBy(item => item.Name)
                .ToArray();

            var tags = properties.Where(item => item.ColumnType == ColumnType.Tag).ToArray();

            var fields = properties.Where(item => item.ColumnType == ColumnType.Field).ToArray();
            if (fields.Length == 0)
            {
                throw new ArgumentException($"{entityType}至少声明一个{nameof(ColumnType.Field)}标记的属性");
            }

            var timestamps = properties.Where(item => item.ColumnType == ColumnType.Timestamp).ToArray();
            if (timestamps.Length > 1)
            {
                throw new ArgumentException($"{entityType}至多只能声明一个{nameof(ColumnType.Timestamp)}标记的属性");
            }

            this.Measurement = entityType.Name;
            this.Tags = tags;
            this.Fields = fields;
            this.Timestamp = timestamps.FirstOrDefault();
        }
    }
}
