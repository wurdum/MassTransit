namespace MassTransit
{
    public interface IPubSubMessageSendTopologyConfigurator<TMessage> :
        IMessageSendTopologyConfigurator<TMessage>,
        IServiceBusMessageSendTopology<TMessage>,
        IServiceBusMessageSendTopologyConfigurator
        where TMessage : class
    {
    }


    public interface IServiceBusMessageSendTopologyConfigurator :
        IMessageSendTopologyConfigurator
    {
    }
}
