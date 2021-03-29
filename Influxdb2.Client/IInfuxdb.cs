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
        /// <param name="entity">ColumnTypeAttribute标记的实体</param>
        /// <param name="bucket">空间名</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        [WriteReturn]
        [HttpPost("/api/v2/write")]
        Task WriteAsync([Required, LineProtocolContent] object entity, [DefaultBucket] string? bucket = default, [DefaultOrg] string? org = default);

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="point">数据点</param>
        /// <param name="bucket">空间名</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        [WriteReturn]
        [HttpPost("/api/v2/write")]
        Task WriteAsync([Required, LineProtocolContent] IPoint point, [DefaultBucket] string? bucket = default, [DefaultOrg] string? org = default);


        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="flux">flux表达式</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        [QueryReturn]
        [HttpPost("/api/v2/query")]
        Task<IDataTableCollection> QueryAsync([Required, FluxContent] IFlux flux, [DefaultOrg] string? org = default);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="flux">flux表达式</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        [QueryReturn]
        [HttpPost("/api/v2/query")]
        Task<IDataTableCollection> QueryAsync([Required, FluxContent] string flux, [DefaultOrg] string? org = default);
    }
}
