using Microsoft.Extensions.Hosting;

namespace Common.MessageQueueManager
{
    public abstract class MessageQueueManager : IMessageQueueManager
    {
        protected MessageQueueOptions Options;

        protected string ProjectName;
        
        protected IHostEnvironment Environment;

        internal List<IProducerConfig> ProducerConfigs;

        public List<IConsumer> ActiveConsumers { get; }

        public MessageQueueManager(MessageQueueOptions options, IHostEnvironment environment, List<IProducerConfig> producerConfigs)
        {
            Options = options;
            ProjectName = options.ProjectName;
            Environment = environment;

            ProducerConfigs = producerConfigs;
            ActiveConsumers = new List<IConsumer>();

            ProducerConfigs.ForEach(p => ((ProducerConfig)p).ConsumerConfigs.ForEach(c => { ((ConsumerConfig)c).ProducerConfig = p as ProducerConfig; }));

            //ConsumerConfigs.Add(new ConsumerConfig() { ProjectName = "VideoHub", Key = "Transcode.Listen"  });
            //ConsumerConfigs.Add(new ConsumerConfig() { ProjectName = "VideoHub", Key = "Notification.Listen"  });
            //ConsumerConfigs.Add(new ConsumerConfig() { ProjectName = "VideoHub", Key = "Video.Listen"  });

            Initialize();
        }

        protected abstract void Initialize();

        public abstract void Send<T>(string producerKey, T entity);

        public abstract void Send(string producerKey, string payload);

        public abstract void Receive<T>(string producerKey, string consumerKey, ReceivedFunc<T> func);
    }
}