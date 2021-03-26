using System.Net.Http;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client
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
            var measurement = context.ParameterValue;
            if (measurement == null)
            {
                throw new InfluxdbException($"值不能为null:{context.ParameterName}");
            }

            var lineString = LineProtocol.GetLineProtocol(measurement);
            context.HttpContext.RequestMessage.Content = new StringContent(lineString);
            return Task.CompletedTask;
        }
    }
}
