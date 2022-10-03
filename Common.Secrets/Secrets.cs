namespace Common.Secrets
{
    public class Secrets : ISecrets
    {
        public const string LoggerMongoConnectionStringKey = "Secrets:Logger:MongoConnectionString";
        public const string VideoHubMongoConnectionStringKey = "Secrets:VideoHub:MongoConnectionString";
        public const string MessageQueueManagerUriKey = "Secrets:MessageQueueManager:Uri";

        public Logger Logger { get; set; }

        public VideoHub VideoHub { get; set; }

        public MessageQueueManager MessageQueueManager { get; set; }

        public Secrets()
        {
            Logger = new Logger();
            VideoHub = new VideoHub();
            MessageQueueManager = new MessageQueueManager();
        }
    }
}