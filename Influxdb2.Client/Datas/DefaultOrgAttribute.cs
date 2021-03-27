using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 默认org
    /// </summary>
    sealed class DefaultOrgAttribute : ApiParameterAttribute
    {
        public override Task OnRequestAsync(ApiParameterContext context)
        {
            var org = context.ParameterValue?.ToString();
            if (string.IsNullOrEmpty(org) == true)
            {
                org = context
                    .HttpContext
                    .ServiceProvider
                    .GetRequiredService<IOptionsMonitor<InfuxdbOptions>>()
                    .CurrentValue
                    .DefaultOrg;
            }

            if (string.IsNullOrEmpty(org) == true)
            {
                throw new ArgumentNullException(context.ParameterName, "org不能为空，除非配置了DefaultOrg");
            }

            context.HttpContext.RequestMessage.AddUrlQuery(context.ParameterName, org);
            return Task.CompletedTask;
        }
    }
}
