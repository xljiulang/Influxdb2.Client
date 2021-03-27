using System;
using System.Diagnostics.CodeAnalysis;

namespace Influxdb2.Client
{
    /// <summary>
    /// Infuxdb选项
    /// </summary>
    public class InfuxdbOptions
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        [AllowNull]
        public Uri Host { get; set; }

        /// <summary>
        /// 请求token
        /// 不包括Token前缀
        /// </summary>
        [AllowNull]
        public string Token { get; set; }


        /// <summary>
        /// 默认的Org
        /// </summary>
        public string? DefaultOrg { get; set; }

        /// <summary>
        /// 默认的Bucket
        /// </summary>
        public string? DefaultBucket { get; set; }
    }
}
