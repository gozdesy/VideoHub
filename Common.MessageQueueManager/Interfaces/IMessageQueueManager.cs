namespace Common.MessageQueueManager
{
    public interface IMessageQueueManager
    {
        List<IConsumer> ActiveConsumers { get; }

        void Send<T>(string producerKey, T entity);

        void Send(string producerKey, string payload);

        void Receive<T>(string producerKey, string consumerKey, ReceivedFunc<T> func);
    }
}