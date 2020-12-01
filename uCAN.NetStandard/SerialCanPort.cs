using System;
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

        public CanMessage Read() {
            throw new NotSupportedException("not implemented yet");
        }

        public void Close() => SerialPort.Close();

        public void Dispose() => SerialPort.Dispose();
    }
}
