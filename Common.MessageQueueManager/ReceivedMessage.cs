namespace Common.MessageQueueManager
{
    public class ReceivedMessage<T>
    {
        public T Data { get; set; }
    }

    public delegate void ReceivedFunc<T>(ReceivedMessage<T> receivedMessage);
}
