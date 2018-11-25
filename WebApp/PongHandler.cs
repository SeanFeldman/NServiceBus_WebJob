using Contracts.Messages;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace WebApp
{
    public class PongHandler : IHandleMessages<Pong>
    {
        private ILog logger = LogManager.GetLogger<PongHandler>();

        public Task Handle(Pong message, IMessageHandlerContext context)
        {
            logger.Info("Received Pong: " + message);
            return Task.CompletedTask;
        }
    }
}