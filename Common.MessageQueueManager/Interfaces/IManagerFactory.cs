namespace Common.MessageQueueManager
{
    public interface IMessageQueueManagerFactory
    {
        IMessageQueueManager Create();
    }
}