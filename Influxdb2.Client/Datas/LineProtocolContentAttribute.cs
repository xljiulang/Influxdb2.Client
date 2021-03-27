using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// LineProtocol内容
    /// </summary>
    sealed class LineProtocolContentAttribute : HttpContentAttribute
    {
        private const string mediaType = "text/plain";

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

            var lineProtocol = LineProtocol.ParseMeasurement(measurement);
            context.HttpContext.RequestMessage.Content = new StringContent(lineProtocol, Encoding.UTF8, mediaType);
            return Task.CompletedTask;
        }
    }
}
