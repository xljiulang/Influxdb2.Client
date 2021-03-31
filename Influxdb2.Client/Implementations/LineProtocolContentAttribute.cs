using System;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// LineProtocol内容
    /// </summary>
    sealed class LineProtocolContentAttribute : HttpContentAttribute
    {
        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Task SetHttpContentAsync(ApiParameterContext context)
        {
            var entity = context.ParameterValue;
            if (entity == null)
            {
                throw new ArgumentNullException(context.ParameterName);
            }

            if (entity is not IPoint point)
            {
                point = new Point(entity);
            }

            var content = new LineProtocolContent();
            point.WriteLineProtocol(content);
            context.HttpContext.RequestMessage.Content = content;

            return Task.CompletedTask;
        }
    }
}
