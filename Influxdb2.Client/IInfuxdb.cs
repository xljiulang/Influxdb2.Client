using Influxdb2.Client.Datas;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client
{
    /// <summary>
    /// Infuxdb操作的接口
    /// </summary>
    public interface IInfuxdb
    {
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="measurement">ColumnType标记的数据</param>
        /// <param name="bucket">空间名</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        [WriteReturn]
        [HttpPost("/api/v2/write")]
        Task WriteAsync([Required, LineProtocolContent] object measurement, [DefaultBucket] string? bucket = default, [DefaultOrg] string? org = default);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="flux">flux表达式</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        [QueryReturn]
        [HttpPost("/api/v2/query")]
        Task<DataTables> QueryAsync([Required, FluxContent] IFlux flux, [DefaultOrg] string? org = default);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="flux">flux表达式</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        [QueryReturn]
        [HttpPost("/api/v2/query")]
        Task<DataTables> QueryAsync([Required, FluxContent] string flux, [DefaultOrg] string? org = default);
    }
}
