using System;
using System.IO;
using System.Threading;
using Microsoft.Azure.WebJobs;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;
using Shared;

namespace WebJob
{
    public class Functions
    {
        [NoAutomaticTrigger]
        public static void Host(TextWriter log, CancellationToken cancellationToken)
        {
            var configuration = new BusConfiguration();
            configuration.DisableFeature<SecondLevelRetries>();
            configuration.DisableFeature<Sagas>();
            configuration.DisableFeature<TimeoutManager>();

            configuration.AzureConfigurationSource();
            configuration.UseTransport<AzureStorageQueueTransport>();
            configuration.UsePersistence<AzureStoragePersistence>();
            configuration.ApplyMessageConventions();
            
            var startableBus = Bus.Create(configuration);
            startableBus.Start();
            log.WriteLine("WebJob - bus started");

            var tzi = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");

            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(3000);
            }

            startableBus.Dispose();
            log.WriteLine("Host: Canceled at " + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi));
        }
    }

    internal class MessageForwardingInCaseOfFaultSource : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {
        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig
            {
                ErrorQueue = "error"
            };
        }
    }

    internal class Audit : IProvideConfiguration<AuditConfig>
    {
        public AuditConfig GetConfiguration()
        {
            return new AuditConfig
            {
                QueueName = "audit"
            };
        }
    }
}
