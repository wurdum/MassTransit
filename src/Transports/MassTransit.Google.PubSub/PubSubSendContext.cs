namespace MassTransit
{
    public interface PubSubSendContext :
        SendContext
    {
    }


    public interface ServiceBusSendContext<out T> :
        SendContext<T>,
        PubSubSendContext
        where T : class
    {
    }
}
