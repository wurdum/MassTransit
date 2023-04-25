namespace MassTransit
{
    using System;
    using System.Threading;
    using AzureServiceBusTransport;
    using AzureServiceBusTransport.Configuration;
    using Configuration;
    using Topology;


    public static class PubSubBusFactory
    {
        public static IMessageTopologyConfigurator MessageTopology => Cached.MessageTopologyValue.Value;


        public static IBusControl CreateUsingServiceBus(Action<IPubSubBusFactoryConfigurator> configure)
        {
            var topologyConfiguration = new PubSubTopologyConfiguration(MessageTopology);
            var busConfiguration = new PubSubBusConfiguration(topologyConfiguration);

            var configurator = new PubSubBusFactoryConfigurator(busConfiguration);

            configure(configurator);

            return configurator.Build(busConfiguration);
        }


        static class Cached
        {
            internal static readonly Lazy<IMessageTopologyConfigurator> MessageTopologyValue =
                new Lazy<IMessageTopologyConfigurator>(() => new MessageTopology(_entityNameFormatter), LazyThreadSafetyMode.PublicationOnly);

            static readonly IEntityNameFormatter _entityNameFormatter;

            static Cached()
            {
                _entityNameFormatter = new MessageNameFormatterEntityNameFormatter(new PubSubMessageNameFormatter());
            }
        }
    }
}
