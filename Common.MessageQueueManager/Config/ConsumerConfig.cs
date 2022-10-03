namespace Common.MessageQueueManager
{
    public class ConsumerConfig : IConsumerConfig
    {
        public string ConsumerKey { get; set; }

        public bool InUse { get; set; }

        public ProducerConfig ProducerConfig { get; set; }
    }
}
