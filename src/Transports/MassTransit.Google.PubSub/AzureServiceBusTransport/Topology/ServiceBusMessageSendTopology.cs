namespace MassTransit.AzureServiceBusTransport.Topology
{
    using MassTransit.Topology;


    public class ServiceBusMessageSendTopology<TMessage> :
        MessageSendTopology<TMessage>,
        IPubSubMessageSendTopologyConfigurator<TMessage>
        where TMessage : class
    {
    }
}
