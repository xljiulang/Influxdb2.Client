using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Influxdb2.Client.Datas
{
    class LineProtocolContent : HttpContent
    {
        private const string mediaType = "text/plain";

        private readonly LineProtocolWriter writer;

        public LineProtocolContent(LineProtocolWriter writer)
        {
            this.writer = writer;
            this.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return this.writer.CopyToStreamAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = this.writer.ByteCount;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            this.writer.Dispose();
        }
    }
}
