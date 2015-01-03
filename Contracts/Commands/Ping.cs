using System;

namespace Contracts.Commands
{
    public class Ping
    {
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return string.Format("PING Message: {0}, Timestamp: {1}", Message, Timestamp);
        }
    }
}
