using System;
using Microsoft.Azure.WebJobs;

namespace WebJob
{
    class Program
    {
        static void Main()
        {
            var config = new JobHostConfiguration();

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }
            var host = new JobHost(config);
            Console.WriteLine("Starting Host with NSB");
            host.Call(typeof(Functions).GetMethod("Host"));
            host.RunAndBlock();
        }
    }
}
