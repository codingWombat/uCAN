using System;
using System.IO;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using crozone.SerialPorts.Abstractions;
using static uCAN.Util;

namespace uCAN {
    public sealed class SerialCanPort : ICanPort {
        private readonly byte[] oneByteReadBuf = new byte[1];

        public ISerialPort SerialPort { get; }

        public int BaudRate {
            get => SerialPort.BaudRate;
            set => SerialPort.BaudRate = value;
        }

        public int NumberOfFilters => throw new NotSupportedException("not implemented yet");

        public SerialCanPort(ISerialPort serialPort) => SerialPort = CheckNonNull(serialPort);

        public bool SetAcceptanceFilter(int fid, int code, int mask, int isExt) => throw new NotSupportedException("not implemented yet");

        public async Task OpenAsync() {
            SerialPort.Open();
            await SerialPort.BaseStream.WriteAsync("V\rN\rO\r".ToByteArray());
        }

        private static byte Check(int b) {
            if(b < 0 || b > 0xff) throw new IOException("unexpected end of stream");
            if(b == '\r') throw new IOException("unexpected end of line");
            if(b == 0x07) throw new InvalidOperationException("byte 0x07 received");
            return (byte)b;
        }

        private async Task<byte> ReadByte(CancellationToken cancel = default) {
            int num = await SerialPort.BaseStream.ReadAsync(oneByteReadBuf, cancel);
            if(num < 1) throw new IOException("unexpected end of stream");
            return Check(oneByteReadBuf[0]);
        }

        private async Task ReadBytes(byte[] buf, int offset, int count) {
            await SerialPort.BaseStream.ReadAllBytesAsync(buf, offset, count);
            for(int i = offset; i < offset + count; i++) {
                Check(buf[i]);
            }
        }

        private async Task<byte[]> ReadBytes(int len) {
            var ret = new byte[len];
            await ReadBytes(ret, 0, ret.Length);
            return ret;
        }

        private async Task<string> ReadAscii(int len) {
            var tmp1 = await ReadBytes(len);
            var tmp2 = new char[tmp1.Length];
            for(int i = 0; i < tmp1.Length; i++) {
                tmp2[i] = (char)tmp1[i];
            }
            return new string(tmp2);
        }
        private static string ToHex(long value, int len) {
            var str = Convert.ToString(value, 16);
            if(str.Length > len) throw new ArgumentException(value + " does not fit within " + len + " hex digits");
            return "0".Repeat(len - str.Length) + str;
        }

        //port of SLCanAdapter.cpp SLCanAdapter_p::receive()
        public async Task<CanMessage> ReadAsync(CancellationToken cancel = default) {
            while(true) {
                var start = await ReadByte(cancel);
                int idLen;
                bool isExt = false;
                if(start == 'T') {
                    idLen = 8;
                    isExt = true;
                }
                else if(start == 't') idLen = 3;
                else {
                    while(await ReadByte() != '\r') { } //skip line
                    continue;
                }
                uint id = Convert.ToUInt32(await ReadAscii(idLen), 16);
                int len = Convert.ToInt32(((char)await ReadByte()).ToString(), 16);
                var rawData = await ReadAscii(len * 2);
                var data = new byte[len];
                for(int i = 0; i < len; i++) {
                    data[i] = Convert.ToByte(rawData.Substring(i * 2, 2), 16);
                }
                var end = SerialPort.BaseStream.ReadByte();
                if(end != '\r') throw new IOException("end of line expected, got " + end);
                return new CanMessage {
                    Id = id,
                    IsExtended = isExt,
                    Data = data
                };
            }
        }

        public async Task WriteAsync(CanMessage msg, CancellationToken cancel = default) {
            var b = new StringBuilder();
            if(msg.IsExtended) b.Append("T").Append(ToHex(msg.Id, 8));
            else b.Append("t").Append(ToHex(msg.Id, 3));
            b.Append(ToHex(msg.Data.Length, 1));
            foreach(var e in msg.Data) {
                b.Append(Convert.ToString(e, 16));
            }
            b.Append("\r");
            await SerialPort.BaseStream.WriteAsync(b.ToByteArray(), cancel);
        }

        public async Task CloseAsync() {
            using(SerialPort) {
                await SerialPort.BaseStream.WriteAsync("C\r".ToByteArray());
            }
        }

        public void Dispose() => CloseAsync().Wait();

        public async ValueTask DisposeAsync() => await CloseAsync();
    }
}
