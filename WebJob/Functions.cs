using System;
using System.IO;
using System.Threading;
using Microsoft.Azure.WebJobs;

namespace WebJob
{
    public class Functions
    {
        [NoAutomaticTrigger]
        public static void Host(TextWriter log, CancellationToken cancellationToken)
        {
            var tzi = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");

            while (!cancellationToken.IsCancellationRequested)
            {
                log.WriteLine("Host: still running at " + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi));
                Thread.Sleep(3000);
            }

            log.WriteLine("Host: Canceled at " + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi));
        }
    }
}
