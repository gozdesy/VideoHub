namespace Common.MessageQueueManager.Providers.RabbitMQ
{
    public class RabbitMQProducerConfig : ProducerConfig
    {
        public string ExchangeName { get; set; }

        public string RoutingKey { get; set; }

        public RabbitMQProducerConfig(ProducerConfig producerConfig, string prefix)
        {
            ProjectName = producerConfig.ProjectName;
            ProducerKey = producerConfig.ProducerKey;

            ExchangeName = $"{ProjectName}.{prefix}.exchange.{ProducerKey}".ToLower();  // "vh.dev.exchange.video.uploaded"
            RoutingKey = ProducerKey.ToLower();    // video.uploaded

            var consumerConfigs = RabbitMQConsumerConfig.ToRabbitMQConsumerConfigList(this, producerConfig.ConsumerConfigs, prefix);
            ConsumerConfigs.AddRange(consumerConfigs);
        }

        public static List<RabbitMQProducerConfig> ToRabbitMQProducerConfigList(List<IProducerConfig> list, string prefix)
        {
            return list.Select(c => new RabbitMQProducerConfig((ProducerConfig) c, prefix)).ToList();
        }
    }
}
