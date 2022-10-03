using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Common.MessageQueueManager
{
    public class MessageQueueManagerFactory : IMessageQueueManagerFactory
    {
        private readonly IHostEnvironment _environment;
        private readonly IOptions<MessageQueueOptions> _options;
        private readonly List<IProducerConfig> _producerConfigs;

        public MessageQueueManagerFactory(IOptions<MessageQueueOptions> options, IHostEnvironment environment, List<IProducerConfig> producerConfigs)
        {
            _options = options;
            _environment = environment;
            _producerConfigs = producerConfigs;
        }

        public IMessageQueueManager Create() 
        {
            if (!Enum.TryParse<MessageQueueProviderType>(_options.Value.ActiveTypeName, out MessageQueueProviderType type))
                type = MessageQueueProviderType.RabbitMQ;
            return Create(type, _options.Value);
        }

        private IMessageQueueManager Create(MessageQueueProviderType type, MessageQueueOptions options)
        {
            switch (type)
            {
                case MessageQueueProviderType.RabbitMQ:
                    return new RabbitMQManager(options, _environment, _producerConfigs);
                default:
                    return null;
            }
        }
    }
}