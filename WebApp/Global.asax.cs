using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;
using Shared;

namespace WebApp
{
    public class MvcApplication : HttpApplication
    {
        public static IStartableBus startableBus;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var configuration = new BusConfiguration();
            configuration.DisableFeature<SecondLevelRetries>();
            configuration.DisableFeature<Sagas>();
            configuration.DisableFeature<TimeoutManager>();
            configuration.AzureConfigurationSource();
            configuration.UseTransport<AzureStorageQueueTransport>();
            configuration.UsePersistence<AzureStoragePersistence>();
            configuration.ApplyMessageConventions();
            //~configuration.EnableInstallers();
            startableBus = Bus.Create(configuration);
            startableBus.Start();
        }

        protected void Application_End()
        {
            startableBus.Dispose();
        }
    }

    internal class MappingConfigurationSource : IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            return new UnicastBusConfig
            {
                MessageEndpointMappings = new MessageEndpointMappingCollection
                {
                    new MessageEndpointMapping
                    {
                         Endpoint = "webjob",
                         AssemblyName = "Contracts",
                    }
                }
            };
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
