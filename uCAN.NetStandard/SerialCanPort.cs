using System;
using System.IO;
using System.Text;
using crozone.SerialPorts.Abstractions;
using static uCAN.Util;

namespace uCAN {
    public sealed class SerialCanPort : ICanPort {
        public ISerialPort SerialPort { get; }

        public int BaudRate {
            get => SerialPort.BaudRate;
            set => SerialPort.BaudRate = value;
        }

        public int NumberOfFilters => throw new NotSupportedException("not implemented yet");

        public SerialCanPort(ISerialPort serialPort) => SerialPort = CheckNonNull(serialPort);

        public bool SetAcceptanceFilter(int fid, int code, int mask, int isExt) => throw new NotSupportedException("not implemented yet");

        public void Open() => SerialPort.Open();

        private static byte Check(int b) {
            if(b < 0 || b > 0xff) throw new IOException("unexpected end of stream");
            if(b == '\r') throw new IOException("unexpected end of line");
            if(b == 0x07) throw new InvalidOperationException("byte 0x07 received");
            return (byte)b;
        }

        private byte ReadByte() => Check(SerialPort.BaseStream.ReadByte());

        private void ReadBytes(byte[] buf, int offset, int count) {
            SerialPort.BaseStream.ReadAllBytes(buf, offset, count);
            for(int i = offset; i < offset + count; i++) {
                Check(buf[i]);
            }
        }

        private byte[] ReadBytes(int len) {
            var ret = new byte[len];
            ReadBytes(ret, 0, ret.Length);
            return ret;
        }

        private string ReadAscii(int len) {
            var tmp1 = ReadBytes(len);
            var tmp2 = new char[tmp1.Length];
            for(int i = 0; i < tmp1.Length; i++) {
                tmp2[i] = (char)tmp1[i];
            }
            return new string(tmp2);
        }

        //port of SLCanAdapter.cpp SLCanAdapter_p::receive()
        public CanMessage Read() {
            while(true) {
                var start = ReadByte();
                int idLen;
                bool isExt = false;
                if(start == 'T') {
                    idLen = 8;
                    isExt = true;
                }
                else if(start == 't') idLen = 3;
                else {
                    while(ReadByte() != '\r') { } //skip line
                    continue;
                }
                uint id = Convert.ToUInt32(ReadAscii(idLen), 16);
                int len = Convert.ToInt32(((char)ReadByte()).ToString(), 16);
                var rawData = ReadAscii(len * 2);
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

        public void Close() => SerialPort.Close();

        public void Dispose() => SerialPort.Dispose();
    }
}
