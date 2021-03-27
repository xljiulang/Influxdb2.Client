using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// 写入返回检测
    /// </summary>
    sealed class WriteReturnAttribute : SpecialReturnAttribute
    {
        /// <summary>
        /// 检测结果的正确性
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task SetResultAsync(ApiResponseContext context)
        {
            if (context.ResultStatus == ResultStatus.None)
            {
                var response = context.HttpContext.ResponseMessage;
                if (response != null && response.IsSuccessStatusCode == false)
                {
                    var error = await context.JsonDeserializeAsync(typeof(InfuxdbError));
                    if (error is InfuxdbError infuxdbError)
                    {
                        throw new InfluxdbException(infuxdbError);
                    }
                }
            }
        }
    }
}
