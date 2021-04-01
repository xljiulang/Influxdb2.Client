using System;

namespace Influxdb2.Client.Core
{
    /// <summary>
    /// LineProtocol工具
    /// </summary>
    static class LineProtocolUtil
    {
        private static readonly char[] encodeFieldValueChars = new char[] { '"', '\\' };

        /// <summary>
        /// 对标签或字段名进行编码
        /// </summary>
        /// <param name="name">名称</param>
        /// <exception cref="ProtocolException"></exception>
        /// <returns></returns>
        public static string Encode(string? name, bool escapeEqual = true)
        {
            if (name == null)
            {
                throw new ProtocolException($"标签或字段名的值不能为空");
            }

            var builder = new ValueStringBuilder(stackalloc char[256]);
            foreach (var c in name)
            {
                switch (c)
                {
                    case '\r':
                        builder.Append("\\r");
                        continue;
                    case '\n':
                        builder.Append("\\n");
                        continue;
                    case '\t':
                        builder.Append("\\t");
                        continue;
                    case ' ':
                    case ',':
                        builder.Append('\\');
                        break;
                    case '=':
                        if (escapeEqual == true)
                        {
                            builder.Append('\\');
                        }
                        break;
                }
                builder.Append(c);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 对字段内容进行编码
        /// </summary>
        /// <param name="value">字段内容</param> 
        /// <returns></returns>
        public static string? EncodeFieldValue(string? value)
        {
            var builder = new ValueStringBuilder(stackalloc char[256]);
            builder.Append('"');
            if (string.IsNullOrEmpty(value) == false)
            {
                if (value.IndexOfAny(encodeFieldValueChars) < 0)
                {
                    builder.Append(value);
                }
                else
                {
                    foreach (var c in value)
                    {
                        if (c == '"' || c == '\\')
                        {
                            builder.Append('\\');
                        }
                        builder.Append(c);
                    }
                }
            }
            builder.Append('"');
            return builder.ToString();
        }

        /// <summary>
        /// 创建字段值的转换器
        /// </summary>
        /// <param name="fieldType">字段类型</param>
        /// <returns></returns>
        public static Func<object?, string?> CreateFieldValueConverter(Type fieldType)
        {
            if (fieldType == typeof(sbyte) ||
                fieldType == typeof(byte) ||
                fieldType == typeof(short) ||
                fieldType == typeof(int) ||
                fieldType == typeof(long) ||
                typeof(Enum).IsAssignableFrom(fieldType))
            {
                return value => value == null ? Throw() : $"{value}i";
            }

            if (fieldType == typeof(ushort) ||
                fieldType == typeof(uint) ||
                fieldType == typeof(ulong))
            {
                return value => value == null ? Throw() : $"{value}u";
            }

            if (fieldType == typeof(bool) ||
                fieldType == typeof(decimal) ||
                fieldType == typeof(float) ||
                fieldType == typeof(double))
            {
                return value => value == null ? Throw() : value.ToString();
            }

            // 文本类型的Field
            return value =>
            {
                return EncodeFieldValue(value?.ToString());
            };

            static string? Throw()
            {
                throw new ProtocolException("非文本字段的值不能为null");
            }
        }

        /// <summary>
        /// 获取unix纳秒时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long? GetNsTimestamp(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            var value = dateTime.Value;
            if (value.Kind == DateTimeKind.Unspecified)
            {
                value = DateTime.SpecifyKind(value, DateTimeKind.Local);
            }

            var dateTimeOffset = new DateTimeOffset(value);
            return GetNsTimestamp(dateTimeOffset);
        }

        /// <summary>
        /// 获取unix纳秒时间戳
        /// </summary>
        /// <param name="dateTimeOffset"></param>
        /// <returns></returns>
        public static long? GetNsTimestamp(DateTimeOffset? dateTimeOffset)
        {
            return dateTimeOffset == null
                ? default
                : dateTimeOffset.Value.Subtract(DateTimeOffset.UnixEpoch).Ticks * 100;
        }
    }
}
