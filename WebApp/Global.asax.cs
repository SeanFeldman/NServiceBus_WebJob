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
        private static IEndpointInstance NSBEndpoint;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var configuration = new EndpointConfiguration("WebApp");
            WireupAutofacContainer(configuration);
            Shared.Configuration.ConfigureEndpoint(configuration);
            NSBEndpoint = Endpoint.Start(configuration).Result;
        }

        private static void WireupAutofacContainer(EndpointConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof (MvcApplication).Assembly)
                .PropertiesAutowired();
         
            builder.Register(x => NSBEndpoint)
                .As<IEndpointInstance>()
                .SingleInstance();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            configuration.UseContainer<AutofacBuilder>(x => x.ExistingLifetimeScope(container));
        }

        protected void Application_End()
        {
            NSBEndpoint.Stop().Wait();
        }
    }
    

}
