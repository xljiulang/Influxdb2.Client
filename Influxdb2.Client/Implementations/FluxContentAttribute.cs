using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// 表示vnd.flux的内部
    /// </summary>
    sealed class FluxContentAttribute : HttpContentAttribute
    {
        private const string MediaType = "application/vnd.flux";

        protected override Task SetHttpContentAsync(ApiParameterContext context)
        {
            var flux = context.ParameterValue?.ToString();
            var fluxContent = new StringContent(flux, Encoding.UTF8, MediaType);
            context.HttpContext.RequestMessage.Content = fluxContent;
            return Task.CompletedTask;
        }
    }
}
