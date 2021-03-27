using System;
using System.Text;

namespace Influxdb2.Client.Datas
{
    /// <summary>
    /// LineProtocol工具
    /// </summary>
    static class LineProtocolUtil
    {
        /// <summary>
        /// 对名称进行编码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string EncodeName(string name)
        {
            var span = name.AsSpan();
            if (span.IndexOfAny(",= \r\n") >= 0)
            {
                var buidler = new StringBuilder();
                foreach (var c in span)
                {
                    if (c == '\r' || c == '\n')
                    {
                        continue;
                    }
                    if (c == ',' || c == '=' || c == ' ')
                    {
                        buidler.Append('\\');
                    }
                    buidler.Append(c);
                }
                return buidler.ToString();
            }
            return name;
        }

        /// <summary>
        /// 对内容进行编码
        /// </summary>
        /// <param name="value"></param> 
        /// <returns></returns>
        public static string EncodeValue(string value)
        {
            var span = value.AsSpan();
            if (span.IndexOfAny("\"\r\n") >= 0)
            {
                var buidler = new StringBuilder();
                foreach (var c in span)
                {
                    if (c == '\r' || c == '\n')
                    {
                        continue;
                    }
                    if (c == '"')
                    {
                        buidler.Append('\\');
                    }
                    buidler.Append(c);
                }
                return buidler.ToString();
            }
            return value;
        }
    }
}
