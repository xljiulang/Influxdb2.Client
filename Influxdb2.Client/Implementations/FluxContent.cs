using System.Net.Http;
using System.Text;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// 表示flux查询内容
    /// </summary>
    sealed class FluxContent : StringContent
    {
        private const string MediaType = "application/vnd.flux";

        /// <summary>
        /// flux查询内容
        /// </summary>
        /// <param name="flux"></param>
        public FluxContent(string flux)
            : base(flux, Encoding.UTF8, MediaType)
        {
        }
    }
}
