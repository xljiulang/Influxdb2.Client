using System;
using System.Text;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 数据行协议
    /// </summary>
    static class LineProtocol
    {
        /// <summary>
        /// 解析Measurement
        /// </summary>
        /// <param name="measurement"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static string ParseMeasurement(object measurement)
        {
            var desciptor = MeasurementDesciptor.Get(measurement.GetType());
            var builder = new StringBuilder().Append(desciptor.Name);

            foreach (var tag in desciptor.Tags)
            {
                var tagValue = tag.GetString(measurement);
                if (tagValue != null)
                {
                    var encodeValue = EncodeName(tagValue);
                    builder.Append(',').Append(tag.Name).Append('=').Append(encodeValue);
                }
            }

            var fieldWrited = false;
            foreach (var field in desciptor.Fields)
            {
                var fieldValue = field.GetString(measurement);
                if (fieldValue != null)
                {
                    var divider = ',';
                    if (fieldWrited == false)
                    {
                        divider = ' ';
                        fieldWrited = true;
                    }

                    var encodeValue = EncodeValue(fieldValue);
                    builder.Append(divider).Append(field.Name).Append('=').Append('"').Append(encodeValue).Append('"');
                }
            }

            if (fieldWrited == false)
            {
                throw new ArgumentException($"{desciptor.Name}至少有一个{nameof(ColumnType.Field)}列不为null");
            }

            if (desciptor.Time != null)
            {
                var timestamp = desciptor.Time.GetString(measurement);
                if (timestamp != null)
                {
                    builder.Append(' ').Append(timestamp);
                }
            }

            return builder.ToString();
        }


        /// <summary>
        /// 对名称进行编码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string EncodeName(string name)
        {
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
        /// 对内容进行编码
        /// </summary>
        /// <param name="value"></param> 
        /// <returns></returns>
        private static string EncodeValue(string value)
        {
            var span = value.AsSpan();
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
            return value;
        }
    }
}
