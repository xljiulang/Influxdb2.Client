using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 实体描述
    /// </summary>
    sealed class EntityDesciptor
    {
        /// <summary>
        /// 描述缓存
        /// </summary>
        private static readonly ConcurrentDictionary<Type, EntityDesciptor> cache = new();

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="entityType"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static EntityDesciptor Get(Type entityType)
        {
            return cache.GetOrAdd(entityType, t => new EntityDesciptor(t));
        }


        /// <summary>
        /// 获取Measurement
        /// </summary>
        public string Measurement { get; }

        /// <summary>
        /// utf8表示的Measurement
        /// </summary>
        public byte[] Utf8Measurement { get; }

        /// <summary>
        /// 获取所有字段
        /// </summary>
        public EntityPropertyDescriptor[] Fields { get; }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        public EntityPropertyDescriptor[] Tags { get; }

        /// <summary>
        /// 获取时间点
        /// </summary>
        public EntityPropertyDescriptor? Timestamp { get; }


        /// <summary>
        /// 实体描述
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <exception cref="ArgumentException"></exception>
        private EntityDesciptor(Type entityType)
        {
            var properties = entityType.GetProperties()
                .Where(item => item.CanRead && item.IsDefined(typeof(ColumnTypeAttribute)))
                .Select(p => new EntityPropertyDescriptor(p))
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
            this.Utf8Measurement = Encoding.UTF8.GetBytes(entityType.Name);
            this.Tags = tags;
            this.Fields = fields;
            this.Timestamp = timestamps.FirstOrDefault();
        }
    }
}
