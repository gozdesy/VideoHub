namespace Common.MessageQueueManager
{
    public class ProducerConfig  : IProducerConfig
    {
        public string ProjectName { get; set; }

        public string ProducerKey { get; set; }

        
        public List<IConsumerConfig> ConsumerConfigs;

        public ProducerConfig()
        {
            ConsumerConfigs = new List<IConsumerConfig>();  
        }
    }
}
