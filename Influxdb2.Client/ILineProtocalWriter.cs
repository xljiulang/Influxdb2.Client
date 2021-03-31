using System;

namespace Influxdb2.Client
{
    public interface ILineProtocolWriter
    {

        ILineProtocolWriter Write(ReadOnlySpan<char> value);
    }
}