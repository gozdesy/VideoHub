namespace Common.MessageQueueManager
{
    public class MessageQueueOptions
    {
        public string ProjectName { get; set; }
        public string ActiveTypeName { get; set; }
        public string Uri { get; set; }
        public RabbitMQOptions RabbitMQOptions { get; set; }
    }
}
