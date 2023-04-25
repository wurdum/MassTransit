namespace MassTransit.AzureServiceBusTransport
{
    using System;
    using Transports;


    public class PubSubMessageNameFormatter :
        IMessageNameFormatter
    {
        readonly IMessageNameFormatter _formatter;

        public PubSubMessageNameFormatter(string namespaceSeparator = default)
        {
            _formatter = string.IsNullOrWhiteSpace(namespaceSeparator)
                ? new DefaultMessageNameFormatter("---", "--", "/", "-")
                : new DefaultMessageNameFormatter("---", "--", namespaceSeparator, "-");
        }

        public MessageName GetMessageName(Type type)
        {
            return _formatter.GetMessageName(type);
        }
    }
}
