using System;

namespace Influxdb2.Client
{
    /// <summary>
    /// 自定义数据点
    /// </summary>
    public class CustomPointData : IPointData
    {

        public string ToLineProtocol()
        {
            throw new NotImplementedException();
        }
    }
}
