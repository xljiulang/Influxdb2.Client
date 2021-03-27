using System;
using System.Text;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// LineProtocol工具
    /// </summary>
    static class LineProtocolUtil
    {
        /// <summary>
        /// 对Measurement编码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string? EncodeMeasurement(string? name)
        {
            return EncodeTagName(name);
        }

        /// <summary>
        /// 对标签名进行编码
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static string? EncodeTagName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var span = name.AsSpan();
            if (span.IndexOfAny(",= \r\n") >= 0)
            {
                var buidler = new StringBuilder();
                foreach (var c in span)
                {
                    if (c == '\r' || c == '\n')
                    {
                        continue;
                    }
                    if (c == ',' || c == '=' || c == ' ')
                    {
                        buidler.Append('\\');
                    }
                    buidler.Append(c);
                }
                return buidler.ToString();
            }
            return name;
        }

        /// <summary>
        /// 对标签值进行编码
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string? EncodeTagValue(object? value)
        {
            return EncodeTagName(value?.ToString());
        }


        /// <summary>
        /// 对字段名进行编码
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public static string? EncodeFielName(string? name)
        {
            return EncodeTagName(name);
        }

        /// <summary>
        /// 对字段内容进行编码
        /// </summary>
        /// <param name="value">字段内容</param> 
        /// <returns></returns>
        public static string? EncodeFieldValue(object? value)
        {
            var stringValue = value?.ToString();
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            var span = stringValue.AsSpan();
            if (span.IndexOfAny("\"\r\n") >= 0)
            {
                var buidler = new StringBuilder();
                foreach (var c in span)
                {
                    if (c == '\r' || c == '\n')
                    {
                        continue;
                    }
                    if (c == '"')
                    {
                        buidler.Append('\\');
                    }
                    buidler.Append(c);
                }
                return buidler.ToString();
            }
            return stringValue;
        }


        /// <summary>
        /// 创建字段值的编码器
        /// </summary>
        /// <param name="fieldType">字段类型</param>
        /// <returns></returns>
        public static Func<object?, string?> CreateFieldValueEncoder(Type fieldType)
        {
            if (fieldType == typeof(sbyte) ||
                fieldType == typeof(byte) ||
                fieldType == typeof(short) ||
                fieldType == typeof(int) ||
                fieldType == typeof(long) ||
                typeof(Enum).IsAssignableFrom(fieldType))
            {
                return value => value == null ? null : $"{value}i";
            }

            if (fieldType == typeof(ushort) ||
                fieldType == typeof(uint) ||
                fieldType == typeof(ulong))
            {
                return value => value == null ? null : $"{value}u";
            }

            if (fieldType == typeof(bool) ||
                fieldType == typeof(decimal) ||
                fieldType == typeof(float) ||
                fieldType == typeof(double))
            {
                return value => value?.ToString();
            }

            return value =>
            {
                var encodeValue = EncodeFieldValue(value);
                return encodeValue == null ? null : @$"""{encodeValue}""";
            };
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
            if (dateTimeOffset == null)
            {
                return null;
            }
            return dateTimeOffset.Value.Subtract(DateTimeOffset.UnixEpoch).Ticks * 100;
        }
    }
}
