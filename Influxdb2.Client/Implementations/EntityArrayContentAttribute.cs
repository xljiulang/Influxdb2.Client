using System;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// 实体集合内容特性
    /// </summary>
    sealed class EntityArrayContentAttribute : HttpContentAttribute
    {
        protected override Task SetHttpContentAsync(ApiParameterContext context)
        {
            if (context.ParameterValue is not Array entities)
            {
                throw new ArgumentNullException(context.ParameterName);
            }

            var content = new LineProtocolContent();
            context.HttpContext.RequestMessage.Content = content;

            foreach (var entity in entities)
            {
                var point = new Point(entity);
                point.WriteLineProtocol(content);
            }

            return Task.CompletedTask;
        }
    }
}
