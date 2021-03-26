using Influxdb2.Client.Datas;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client
{
    /// <summary>
    /// InfuxdbClient的接口
    /// </summary>
    [LoggingFilter]
    public interface IInfuxdbClient
    {
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="measurement"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        [WriteReturn]
        [HttpPost("/api/v2/write")]
        Task WriteAsync([Required, LineProtocolContent] object measurement, [Required] string org, [Required] string bucket);
    }
}
