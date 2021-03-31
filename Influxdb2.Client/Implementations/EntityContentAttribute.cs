using System;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// 实体内容特性
    /// </summary>
    sealed class EntityContentAttribute : HttpContentAttribute
    {
        protected override Task SetHttpContentAsync(ApiParameterContext context)
        {
            var entity = context.ParameterValue;
            if (entity == null)
            {
                throw new ArgumentNullException(context.ParameterName);
            }

            var point = new Point(entity);
            var content = new LineProtocolContent();
            point.WriteLineProtocol(content);
            context.HttpContext.RequestMessage.Content = content;

            return Task.CompletedTask;
        }
    }
}
