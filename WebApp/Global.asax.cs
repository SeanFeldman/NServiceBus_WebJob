using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;
using Shared;

namespace WebApp
{
    public class MvcApplication : HttpApplication
    {
        private static IStartableBus startableBus;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var configuration = new BusConfiguration();
            WireupAutofacContainer(configuration);
            configuration.UseTransport<AzureStorageQueueTransport>();
            configuration.UsePersistence<AzureStoragePersistence>();
            configuration.ApplyMessageConventions();
            configuration.DisableFeature<SecondLevelRetries>();
            configuration.DisableFeature<Sagas>();
            configuration.DisableFeature<TimeoutManager>();
            //~configuration.EnableInstallers();
            startableBus = Bus.Create(configuration);
            startableBus.Start();
        }

        private static void WireupAutofacContainer(BusConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof (MvcApplication).Assembly)
                .PropertiesAutowired();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            configuration.UseContainer<AutofacBuilder>(x => x.ExistingLifetimeScope(container));
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
