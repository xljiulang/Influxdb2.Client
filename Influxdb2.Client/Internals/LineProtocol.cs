using System;
using WebApiClientCore.Internals;

namespace Influxdb2.Client
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
        public static string From(object measurement)
        {
            var desciptor = MeasurementDesciptor.Get(measurement.GetType());
            var builder = new ValueStringBuilder(stackalloc char[1024]);

            builder.Append(desciptor.Name);
            foreach (var tag in desciptor.Tags)
            {
                var tagValue = tag.GetValueString(measurement);
                if (tagValue != null)
                {
                    builder = builder.Append(Comma).Append(tag.Name).Append('=').Append(tagValue);
                }
            }

            var fieldWrited = false;
            foreach (var field in desciptor.Fields)
            {
                var fieldValue = field.GetValueString(measurement);
                if (fieldValue != null)
                {
                    var divider = Comma;
                    if (fieldWrited == false)
                    {
                        divider = Space;
                        fieldWrited = true;
                    }

                    builder = builder.Append(divider).Append(field.Name).Append('=').Append(fieldValue);
                }
            }

            if (fieldWrited == false)
            {
                throw new ArgumentException($"{desciptor.Name}至少一个{nameof(DataType.Field)}有值");
            }

            if (desciptor.Time != null)
            {
                var timestamp = desciptor.Time.GetValueString(measurement);
                if (timestamp != null)
                {
                    builder = builder.Append(Space).Append(timestamp);
                }
            }

            return builder.ToString();
        }

    }
}
