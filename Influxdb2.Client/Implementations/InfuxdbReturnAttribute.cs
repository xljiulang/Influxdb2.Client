using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// Influxdb返回验证
    /// </summary>
    sealed class InfuxdbReturnAttribute : SpecialReturnAttribute
    {
        /// <summary>
        /// 设置结果
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
