using System;

namespace Influxdb2.Client
{
    /// <summary>
    /// 行文本协议写入器
    /// </summary>
    public interface ILineProtocolWriter
    {
        /// <summary>
        /// 写入逗号
        /// </summary> 
        /// <returns></returns>
        ILineProtocolWriter WriteComma();

        /// <summary>
        /// 写入=
        /// </summary> 
        /// <returns></returns>
        ILineProtocolWriter WriteEqual();

        /// <summary>
        /// 写入空格
        /// </summary> 
        /// <returns></returns>
        ILineProtocolWriter WriteSpace();

        /// <summary>
        /// 写入\n
        /// </summary> 
        /// <returns></returns>
        ILineProtocolWriter WriteLine();

        /// <summary>
        /// 写入行文本协议内容
        /// </summary>
        /// <param name="value">内容</param>
        /// <returns></returns>
        ILineProtocolWriter Write(ReadOnlySpan<byte> value);

        /// <summary>
        /// 写入行文本协议内容
        /// </summary>
        /// <param name="value">内容</param>
        /// <returns></returns>
        ILineProtocolWriter Write(ReadOnlySpan<char> value);
    }
}