namespace MassTransit
{
    public interface IPubSubMessagePublishTopologyConfigurator<TMessage> :
        IPubSubMessagePublishTopologyConfigurator,
        IMessagePublishTopologyConfigurator<TMessage>,
        IServiceBusMessagePublishTopology<TMessage>
        where TMessage : class
    {
    }


    public interface IPubSubMessagePublishTopologyConfigurator :
        IMessagePublishTopologyConfigurator,
        IServiceBusTopicConfigurator
    {
    }
}
