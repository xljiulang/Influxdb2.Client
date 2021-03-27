using System;
using System.Text;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 数据行协议
    /// </summary>
    static class LineProtocol
    {
        private const char Comma = ',';
        private const char Space = ' ';

        /// <summary>
        /// 从measurement转换得到
        /// </summary>
        /// <param name="measurement"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static string GetLineProtocol(object measurement)
        {
            var desciptor = MeasurementDesciptor.Get(measurement.GetType());
            var builder = new StringBuilder().Append(desciptor.Name);

            foreach (var tag in desciptor.Tags)
            {
                var tagValue = tag.GetString(measurement);
                if (tagValue != null)
                {
                    builder.Append(Comma).Append(tag.Name).Append('=').Append(tagValue);
                }
            }

            var fieldWrited = false;
            foreach (var field in desciptor.Fields)
            {
                var fieldValue = field.GetString(measurement);
                if (fieldValue != null)
                {
                    var divider = Comma;
                    if (fieldWrited == false)
                    {
                        divider = Space;
                        fieldWrited = true;
                    }

                    builder.Append(divider).Append(field.Name).Append('=').Append(fieldValue);
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
                    builder.Append(Space).Append(timestamp);
                }
            }

            return builder.ToString();
        }

    }
}
