using System;

namespace Influxdb2.Client
{
    public static class LineProtocol
    {
        private const char Comma = ',';
        private const char Space = ' ';
        private const char Quote = '"';

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static string From(object model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var descriptor = MeasurementDesciptor.Get(model.GetType());
            var builder = new ValueStringBuilder(stackalloc char[1024]);
            builder.Append(descriptor.Name);

            foreach (var tag in descriptor.Tags)
            {
                var tagValue = tag.GetValue(model)?.ToString();
                builder.Append(Comma).Append(tag.Name).Append('=').Append(Quote).Append(tagValue).Append(Quote);
            }

            var firstField = true;
            foreach (var field in descriptor.Fields)
            {
                var divider = Comma;
                if (firstField == true)
                {
                    divider = Space;
                    firstField = false;
                }

                var fieldValue = field.GetValue(model)?.ToString();
                builder.Append(divider).Append(field.Name).Append('=').Append(fieldValue);
            }

            return builder.ToString();
        }

    }
}
