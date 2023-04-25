namespace MassTransit.AzureServiceBusTransport.Configuration
{
    using MassTransit.Configuration;


    public interface IServiceBusTopologyConfiguration :
        ITopologyConfiguration
    {
        new IPubSubPublishTopologyConfigurator Publish { get; }

        new IPubSubSendTopologyConfigurator Send { get; }

        new IServiceBusConsumeTopologyConfigurator Consume { get; }
    }
}
