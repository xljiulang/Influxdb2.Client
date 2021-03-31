using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApiClientCore.Internals;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// 表示LineProtocol的http内容
    /// </summary>
    sealed class LineProtocolContent : HttpContent, ILineProtocolWriter
    {
        private const byte Comma = (byte)',';
        private const byte Space = (byte)' ';
        private const byte Equal = (byte)'=';
        private const byte Line = (byte)'\n';
        private const int Utf8MaxByteCount = 4;
        private const string MediaType = "text/plain";

        private readonly RecyclableBufferWriter<byte> writer = new();

        /// <summary>
        /// LineProtocol的http内容
        /// </summary>
        public LineProtocolContent()
        {
            this.Headers.ContentType = new MediaTypeHeaderValue(MediaType);
        }

        /// <summary>
        /// 写入逗号
        /// </summary> 
        /// <returns></returns>
        public ILineProtocolWriter WriteComma()
        {
            this.writer.Write(Comma);
            return this;
        }

        /// <summary>
        /// 写入=
        /// </summary> 
        /// <returns></returns>
        public ILineProtocolWriter WriteEqual()
        {
            this.writer.Write(Equal);
            return this;
        }

        /// <summary>
        /// 写入空格
        /// </summary> 
        /// <returns></returns>
        public ILineProtocolWriter WriteSpace()
        {
            this.writer.Write(Space);
            return this;
        }

        /// <summary>
        /// 写入\n
        /// </summary> 
        /// <returns></returns>
        public ILineProtocolWriter WriteLine()
        {
            this.writer.Write(Line);
            return this;
        }

        /// <summary>
        /// 写入LineProtocol
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ILineProtocolWriter Write(ReadOnlySpan<byte> value)
        {
            writer.Write(value);
            return this;
        }

        /// <summary>
        /// 写入LineProtocol
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ILineProtocolWriter Write(ReadOnlySpan<char> value)
        {
            if (value.IsEmpty == false)
            {
                var span = this.writer.GetSpan(value.Length * Utf8MaxByteCount);
                var byteCount = Encoding.UTF8.GetBytes(value, span);
                this.writer.Advance(byteCount);
            }
            return this;
        }

        /// <summary>
        /// 序列化到流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var segment = this.writer.WrittenSegment;
            return stream.WriteAsync(segment.Array, segment.Offset, segment.Count);
        }

        /// <summary>
        /// 计算内容长度
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override bool TryComputeLength(out long length)
        {
            length = this.writer.WrittenCount;
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            this.writer.Dispose();
        }

        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Encoding.UTF8.GetString(this.writer.WrittenSpan);
        }
    }
}
