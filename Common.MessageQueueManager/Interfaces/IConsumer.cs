namespace Common.MessageQueueManager
{
    public interface IConsumer 
    {
        string ProducerKey { get; set; }

        string ConsumerKey { get; set; }
    }
    public interface IConsumer<T> : IConsumer
    {        
        ReceivedFunc<T> ReceivedFunc { get; set; }
    }
}