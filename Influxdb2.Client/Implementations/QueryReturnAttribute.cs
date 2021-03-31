using Influxdb2.Client.Datas;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// 查询返回处理
    /// </summary>
    sealed class QueryReturnAttribute : SpecialReturnAttribute
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
                if (response == null)
                {
                    return;
                }

                if (response.IsSuccessStatusCode == false)
                {
                    var error = await context.JsonDeserializeAsync(typeof(InfuxdbError));
                    if (error is InfuxdbError infuxdbError)
                    {
                        throw new InfluxdbException(infuxdbError);
                    }
                }

                var encoding = response.Content.GetEncoding(); 
                using var stream = await response.Content.ReadAsStreamAsync();
                var csvReader = new CsvReader(stream, encoding);
                context.Result = await DataTableCollection.ParseAsync(csvReader);
            }
        }
    }
}
