namespace MassTransit
{
    using System;


    public interface IPubSubSendTopologyConfigurator :
        ISendTopologyConfigurator,
        IServiceBusSendTopology
    {
        Action<IServiceBusEntityConfigurator> ConfigureErrorSettings { set; }
        Action<IServiceBusEntityConfigurator> ConfigureDeadLetterSettings { set; }

        new IPubSubMessageSendTopologyConfigurator<T> GetMessageTopology<T>()
            where T : class;
    }
}
