namespace Common.MessageQueueManager.Providers.RabbitMQ
{
    public class RabbitMQConsumerConfig : ConsumerConfig
    {
        public string ExchangeName { get; set; }

        public string RoutingKey { get; set; }

        public string QueueName { get; set; }

        public RabbitMQConsumerConfig(RabbitMQProducerConfig producerConfig, ConsumerConfig consumerConfig, string prefix)
        {
            ProducerConfig = producerConfig;

            ConsumerKey = consumerConfig.ConsumerKey;
            ExchangeName = producerConfig.ExchangeName;
            RoutingKey = producerConfig.RoutingKey;
            QueueName = $"{producerConfig.ProjectName}.{prefix}.queue.{ConsumerKey}".ToLower(); // $"vh.{_entityNamePrefix}.queue.transcode"
        }

        public static List<RabbitMQConsumerConfig> ToRabbitMQConsumerConfigList(RabbitMQProducerConfig producerConfig, List<IConsumerConfig> list, string prefix)
        {
            return list.Select(c => new RabbitMQConsumerConfig(producerConfig, (ConsumerConfig)c, prefix)).ToList();
        }
    }
}
