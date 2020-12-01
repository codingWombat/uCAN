﻿using System;

namespace uCAN {
    public interface ICanPort : IDisposable {
        int BaudRate { get; set; }

        /// <summary>
        /// The number of acceptance filters available.
        /// </summary>
        int NumberOfFilters { get; }

        /// <summary>
        /// Adds an acceptance filter.
        /// Different filters can be configured, depending on implementation.
        /// A "1" in the mask means that the corresponding bit in code is relevant.
        /// </summary>
        /// <param name="fid">filter id</param>
        /// <param name="code">acceptance code</param>
        /// <param name="mask">acceptance mask</param>
        /// <param name="isExt">true for extended message filter</param>
        /// <returns></returns>
        bool SetAcceptanceFilter(int fid, int code, int mask, int isExt);

        void Open();

        /// <summary>
        /// Reads the next message from the underlying stream.
        /// </summary>
        CanMessage Read();

        void Close();
    }
}