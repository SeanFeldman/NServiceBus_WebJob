﻿using System;
using Contracts.Commands;
using Contracts.Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace WebJob
{
    public class PingHandler : IHandleMessages<Ping>
    {
        public IBus Bus { get; set; }
        private ILog logger = LogManager.GetLogger<PingHandler>();

        public void Handle(Ping message)
        {
            logger.Info("Received Ping: " + message);
            Bus.Reply<Pong>(m =>
            {
                m.OriginalMessage = message.Message;
                m.Timestamp = DateTime.UtcNow;
            });
            logger.Info("Replying with Pong");
        }
    }
}