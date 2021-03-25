using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client
{
    /// <summary>
    /// 写入返回检测
    /// </summary>
    sealed class WriteReturnAttribute : SpecialReturnAttribute
    {
        public override async Task SetResultAsync(ApiResponseContext context)
        {
            if (context.ResultStatus == ResultStatus.None)
            {
                var response = context.HttpContext.ResponseMessage;
                if (response != null && response.IsSuccessStatusCode == false)
                {
                    var error = (WriteError?)await context.JsonDeserializeAsync(typeof(WriteError));
                    if (error != null)
                    {
                        throw new InfluxdbWriteException(error);
                    }
                }
            }
        }
    }
}
