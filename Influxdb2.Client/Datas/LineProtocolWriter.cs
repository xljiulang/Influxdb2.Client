using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Internals;

namespace Influxdb2.Client.Datas
{
    class LineProtocolWriter : Disposable, ILineProtocolWriter
    {
        private readonly RecyclableBufferWriter<byte> bufferWriter = new();

        public int ByteCount => this.bufferWriter.WrittenCount;         

        public ILineProtocolWriter Write(ReadOnlySpan<char> value)
        {
            var span = this.bufferWriter.GetSpan(value.Length * 4);
            var length = Encoding.UTF8.GetBytes(value, span);
            this.bufferWriter.Advance(length);
            return this;
        }

        public Task CopyToStreamAsync(Stream stream)
        {
            var segment = this.bufferWriter.WrittenSegment;
            return stream.WriteAsync(segment.Array, segment.Offset, segment.Count);
        }

        protected override void Dispose(bool disposing)
        {
            this.bufferWriter.Dispose();
        }
    }
}
