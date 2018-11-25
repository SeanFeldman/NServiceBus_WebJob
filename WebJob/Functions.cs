using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        public static IEndpointInstance NSB;

        [NoAutomaticTrigger]
        public static async Task Host(TextWriter log, CancellationToken cancellationToken)
        {
            var configuration = new EndpointConfiguration("WebJob");
            Configuration.ConfigureEndpoint(configuration);

            NSB = await Endpoint.Start(configuration).ConfigureAwait(false);
            log.WriteLine("WebJob - bus started");

            var tzi = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");

            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(3000);
            }

            log.WriteLine("Host: Canceled at " + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi));
        }

        
    }
    
}
