namespace MassTransit.AzureServiceBusTransport
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using MassTransit.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Transports;


    public class ServiceBusRegistrationBusFactory :
        TransportRegistrationBusFactory<IPubSubReceiveEndpointConfigurator>
    {
        readonly PubSubBusConfiguration _busConfiguration;
        readonly Action<IBusRegistrationContext, IPubSubBusFactoryConfigurator> _configure;

        public ServiceBusRegistrationBusFactory(Action<IBusRegistrationContext, IPubSubBusFactoryConfigurator> configure)
            : this(new PubSubBusConfiguration(new PubSubTopologyConfiguration(PubSubBusFactory.MessageTopology)), configure)
        {
        }

        ServiceBusRegistrationBusFactory(PubSubBusConfiguration busConfiguration,
            Action<IBusRegistrationContext, IPubSubBusFactoryConfigurator> configure)
            : base(busConfiguration.HostConfiguration)
        {
            _configure = configure;

            _busConfiguration = busConfiguration;
        }

        public override IBusInstance CreateBus(IBusRegistrationContext context, IEnumerable<IBusInstanceSpecification> specifications, string busName)
        {
            var configurator = new PubSubBusFactoryConfigurator(_busConfiguration);

            var options = context.GetRequiredService<IOptionsMonitor<AzureServiceBusTransportOptions>>().Get(busName);
            if (!string.IsNullOrWhiteSpace(options.ConnectionString))
                configurator.Host(options.ConnectionString);

            return CreateBus(configurator, context, _configure, specifications);
        }

        protected override IBusInstance CreateBusInstance(IBusControl bus, IHost<IPubSubReceiveEndpointConfigurator> host,
            IHostConfiguration hostConfiguration, IBusRegistrationContext context)
        {
            return new ServiceBusInstance(bus, host, hostConfiguration, context);
        }
    }
}
