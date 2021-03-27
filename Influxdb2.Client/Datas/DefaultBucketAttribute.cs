using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 默认bucket
    /// </summary>
    sealed class DefaultBucketAttribute : ApiParameterAttribute
    {
        public override Task OnRequestAsync(ApiParameterContext context)
        {
            var bucket = context.ParameterValue?.ToString();
            if (string.IsNullOrEmpty(bucket) == true)
            {
                bucket = context
                    .HttpContext
                    .ServiceProvider
                    .GetRequiredService<IOptionsMonitor<InfuxdbOptions>>()
                    .CurrentValue
                    .DefaultBucket;
            }

            if (string.IsNullOrEmpty(bucket) == true)
            {
                throw new ArgumentNullException(context.ParameterName, "bucket不能为空，除非配置了DefaultBucket");
            }

            context.HttpContext.RequestMessage.AddUrlQuery(context.ParameterName, bucket);
            return Task.CompletedTask;
        }
    }
}
