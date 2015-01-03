using System;

namespace Contracts.Messages
{
    public class Pong
    {
        public string OriginalMessage { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return string.Format("Pong OriginalMessage: {0}, Timestamp: {1}", OriginalMessage, Timestamp);
        }
    }
}