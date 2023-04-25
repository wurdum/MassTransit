namespace MassTransit.AzureServiceBusTransport.Topology
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using MassTransit.Topology;
    using Metadata;
    using Util;


    public class ServiceBusPublishTopology :
        PublishTopology,
        IPubSubPublishTopologyConfigurator
    {
        readonly IMessageTopology _messageTopology;

        public ServiceBusPublishTopology(IMessageTopology messageTopology)
        {
            _messageTopology = messageTopology;
        }

        IServiceBusMessagePublishTopology<T> IServiceBusPublishTopology.GetMessageTopology<T>()
        {
            return GetMessageTopology<T>() as IPubSubMessagePublishTopologyConfigurator<T>;
        }

        public string FormatSubscriptionName(string subscriptionName)
        {
            string name;
            if (subscriptionName.Length > 50)
            {
                string hashed;
                using (var hasher = SHA1.Create())
                {
                    var buffer = Encoding.UTF8.GetBytes(subscriptionName);
                    var hash = hasher.ComputeHash(buffer);
                    hashed = FormatUtil.Formatter.Format(hash).Substring(0, 6);
                }

                name = $"{subscriptionName.Substring(0, 43)}-{hashed}";
            }
            else
                name = subscriptionName;

            return name;
        }

        public string GenerateSubscriptionName(string entityName, string hostScope)
        {
            if (entityName == null)
                throw new ArgumentNullException(nameof(entityName));

            return FormatSubscriptionName(string.IsNullOrWhiteSpace(hostScope) ? entityName : $"{entityName}-{hostScope}");
        }

        IPubSubMessagePublishTopologyConfigurator IPubSubPublishTopologyConfigurator.GetMessageTopology(Type messageType)
        {
            return GetMessageTopology(messageType) as IPubSubMessagePublishTopologyConfigurator;
        }

        public BrokerTopology GetPublishBrokerTopology()
        {
            var builder = new PublishEndpointBrokerTopologyBuilder(this);

            ForEachMessageType<IServiceBusMessagePublishTopology>(x =>
            {
                x.Apply(builder);

                builder.Topic = null;
            });

            return builder.BuildBrokerTopology();
        }

        IPubSubMessagePublishTopologyConfigurator<T> IPubSubPublishTopologyConfigurator.GetMessageTopology<T>()
        {
            return GetMessageTopology<T>() as IPubSubMessagePublishTopologyConfigurator<T>;
        }

        protected override IMessagePublishTopologyConfigurator CreateMessageTopology<T>(Type type)
        {
            var messageTopology = new ServiceBusMessagePublishTopology<T>(this, _messageTopology.GetMessageTopology<T>());

            var connector = new ImplementedMessageTypeConnector<T>(this, messageTopology);

            ImplementedMessageTypeCache<T>.EnumerateImplementedTypes(connector);

            OnMessageTopologyCreated(messageTopology);

            return messageTopology;
        }


        class ImplementedMessageTypeConnector<TMessage> :
            IImplementedMessageType
            where TMessage : class
        {
            readonly ServiceBusMessagePublishTopology<TMessage> _messagePublishTopologyConfigurator;
            readonly IPubSubPublishTopologyConfigurator _publishTopology;

            public ImplementedMessageTypeConnector(IPubSubPublishTopologyConfigurator publishTopology,
                ServiceBusMessagePublishTopology<TMessage> messagePublishTopologyConfigurator)
            {
                _publishTopology = publishTopology;
                _messagePublishTopologyConfigurator = messagePublishTopologyConfigurator;
            }

            public void ImplementsMessageType<T>(bool direct)
                where T : class
            {
                IPubSubMessagePublishTopologyConfigurator<T> messageTopology = _publishTopology.GetMessageTopology<T>();

                _messagePublishTopologyConfigurator.AddImplementedMessageConfigurator(messageTopology, direct);
            }
        }
    }
}
