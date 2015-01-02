using Contracts.Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace WebApp
{
    public class PongHandler : IHandleMessages<Pong>
    {
        private ILog logger = LogManager.GetLogger<PongHandler>();

        public void Handle(Pong message)
        {
            logger.Info("Received Pong message");
        }
    }
}