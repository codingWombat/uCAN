using System;

namespace uCAN {
    public class CanMessage {
        public uint Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsExtended { get; set; }

        public byte[] Data { get; set; }
    }
}
