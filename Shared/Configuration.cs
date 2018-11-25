using Microsoft.Azure;
using NServiceBus;
using NServiceBus.Features;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class Configuration
    {
        public static void ConfigureEndpoint(EndpointConfiguration configuration)
        {
            var recoverability = configuration.Recoverability();
            recoverability.Delayed(
                delayed =>
                {
                    delayed.NumberOfRetries(0);
                });
            configuration.DisableFeature<Sagas>();
            configuration.DisableFeature<TimeoutManager>();
            configuration.UseSerialization<NewtonsoftSerializer>();
            configuration.DisableFeature<MessageDrivenSubscriptions>();

            var transport = configuration.UseTransport<AzureStorageQueueTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(
                assembly: typeof(Contracts.Commands.Ping).Assembly,
                destination: "WebJob");

            var persistence = configuration.UsePersistence<InMemoryPersistence>();
            //struggling to get this one to work...so using InMemoryPersistence
            //var persistence = configuration.UsePersistence<AzureStoragePersistence>();
            //var pString = ConfigurationManager.ConnectionStrings["NServicesBus/Persistence"].ConnectionString;
            //persistence.ConnectionString(pString);

            configuration.ApplyMessageConventions();
            configuration.AuditProcessedMessagesTo(
                auditQueue: "webjobAuditQueue",
                timeToBeReceived: TimeSpan.FromMinutes(1));
            configuration.AuditProcessedMessagesTo(
                auditQueue: "webjobAuditQueue",
                timeToBeReceived: TimeSpan.FromMinutes(10));

            configuration.EnableInstallers();
        }
    }
}
