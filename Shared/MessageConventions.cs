using NServiceBus;

namespace Shared
{
    public static class MessageConventions
    {
        public static void ApplyMessageConventions(this BusConfiguration configuration)
        {
            configuration.Conventions().DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("Contracts") 
                && t.Namespace.EndsWith("Commands"));
            configuration.Conventions().DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Contracts") 
                && t.Namespace.EndsWith("Events"));
            configuration.Conventions().DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Contracts") 
                && t.Namespace.EndsWith("Messages"));
        }
    }
}
