using System;
using System.Threading.Tasks;
using Contracts.Commands;
using Contracts.Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace WebJob
{
    public class PingHandler : IHandleMessages<Ping>
    {
        private ILog logger = LogManager.GetLogger<PingHandler>();

        public async Task Handle(Ping message, IMessageHandlerContext context)
        {
            logger.Info("Received Ping: " + message);
            await context.Reply<Pong>(m =>
            {
                m.OriginalMessage = message.Message;
                m.Timestamp = DateTime.UtcNow;
            }).ConfigureAwait(false);
            logger.Info("Replying with Pong");
        }
    }
}