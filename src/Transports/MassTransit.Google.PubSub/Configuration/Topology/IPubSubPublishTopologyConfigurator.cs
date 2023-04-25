namespace MassTransit
{
    using System;


    public interface IPubSubPublishTopologyConfigurator :
        IPublishTopologyConfigurator,
        IServiceBusPublishTopology
    {
        new IPubSubMessagePublishTopologyConfigurator<T> GetMessageTopology<T>()
            where T : class;

        new IPubSubMessagePublishTopologyConfigurator GetMessageTopology(Type messageType);
    }
}
