using Influxdb2.Client.Implementations;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client
{
    /// <summary>
    /// Infuxdb操作的接口
    /// </summary>
    [InfuxdbReturn]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IInfuxdbApi
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="flux">flux表达式</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        [HttpPost("/api/v2/query")]
        Task<Stream> QueryAsync(HttpContent flux, string org);

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="entity">ColumnTypeAttribute标记的实体</param>
        /// <param name="bucket">空间名</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        [HttpPost("/api/v2/write")]
        Task WriteAsync(HttpContent lineProtocol, string bucket, string org);
    }
}
