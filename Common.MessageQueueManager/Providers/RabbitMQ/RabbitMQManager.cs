using Common.MessageQueueManager.Providers.RabbitMQ;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Common.MessageQueueManager
{
    public class RabbitMQManager : MessageQueueManager, IDisposable
    {
        private RabbitMQOptions _options;
        private string _prefix;

        private IConnection _connection;
        private IModel _channel;
        private bool disposedValue;

        internal List<RabbitMQProducerConfig> _rabbitMQProducerConfigs;
        public RabbitMQManager(MessageQueueOptions options, IHostEnvironment environment, List<IProducerConfig> producerConfigs) : base(options, environment, producerConfigs)
        {}

        protected override void Initialize() 
        {
            _options = Options.RabbitMQOptions;

            _prefix = (Environment.IsProduction() ? String.Empty : Environment.EnvironmentName).ToLower();

            var factory = new ConnectionFactory();
            factory.Uri = new Uri(Options.Uri);
            factory.ClientProvidedName = $"App:{Options.ProjectName}";
            factory.AutomaticRecoveryEnabled = true;

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            //_channel.BasicQos(0, 1, false);  // accept only one unack-ed message at a time

            // Producer Config
            //ProducerConfigs = new List<RabbitMQProducerConfig>();  

            _rabbitMQProducerConfigs = RabbitMQProducerConfig.ToRabbitMQProducerConfigList(ProducerConfigs, _prefix);

            //_rabbitMQProducerConfigs.Add(new RabbitMQProducerConfig() { ProjectName = "VideoHub", Key = "Video.Uploaded", ExchangeName = $"vh.{_entityNamePrefix}.exchange.video", RoutingKey = "video_uploaded" });
            //_rabbitMQProducerConfigs.Add(new RabbitMQProducerConfig() { ProjectName = "VideoHub", Key = "Transcode.Completed", ExchangeName = $"vh.{_entityNamePrefix}.exchange.transcode", RoutingKey = "transcode_completed" });

            foreach (var config in _rabbitMQProducerConfigs.DistinctBy(c => new { c.ExchangeName }))
            {
                _channel.ExchangeDeclare(exchange: config.ExchangeName, type: "direct");

                var rabbitMQConsumerConfigs = RabbitMQConsumerConfig.ToRabbitMQConsumerConfigList(config, config.ConsumerConfigs, _prefix);

                foreach (var consumerConfig in rabbitMQConsumerConfigs.DistinctBy(c => new { c.QueueName }))
                {
                    _channel.QueueDeclare(consumerConfig.QueueName, durable: true, exclusive: false, autoDelete: true);
                }

                foreach (var consumerConfig in rabbitMQConsumerConfigs.DistinctBy(c => new { c.ExchangeName, c.QueueName, c.RoutingKey }))
                {
                    _channel.QueueBind(queue: consumerConfig.QueueName, exchange: config.ExchangeName, routingKey: config.RoutingKey);
                }
            }

            // Consumer Config
            //ConsumerConfigs.Add(new RabbitMQConsumerConfig() { ProjectName = "VideoHub", Key = "Transcode.Listen", ExchangeName = $"vh.{_entityNamePrefix}.exchange.video", QueueName = $"vh.{_entityNamePrefix}.queue.transcode", RoutingKey = "video_uploaded" });
            //ConsumerConfigs.Add(new RabbitMQConsumerConfig() { ProjectName = "VideoHub", Key = "Notification.Listen", ExchangeName = $"vh.{_entityNamePrefix}.exchange.video", QueueName = $"vh.{_entityNamePrefix}.queue.notification", RoutingKey = "video_uploaded" });
            //ConsumerConfigs.Add(new RabbitMQConsumerConfig() { ProjectName = "VideoHub", Key = "Video.Listen", ExchangeName = $"vh.{_entityNamePrefix}.exchange.transcode", QueueName = $"vh.{_entityNamePrefix}.queue.video", RoutingKey = "transcode_completed" });

            
        }

        public override void Send<T>(string key, T entity) 
        {
            var message = JsonSerializer.Serialize<T>(entity);
            Send(key, message);
        }

        public override void Send(string key, string payload)
        {
            RefreshConnection();

            //ProducerConfigs.Select(p => new RabbitMQProducerConfig()).Where(c => c.ProjectName == ProjectName && c.ProducerKey == key).ToList().ForEach(c =>
            _rabbitMQProducerConfigs.ForEach(c =>
            {
                var body = Encoding.UTF8.GetBytes(payload);
                _channel.BasicPublish(exchange: c.ExchangeName,
                                     routingKey: c.RoutingKey,
                                     basicProperties: null,
                                     body: body);
            });
        }

        public override void Receive<T>(string producerKey, string consumerKey, ReceivedFunc<T> func)
        {
            RefreshConnection();

            //var producer = ProducerConfigs.Select(p => p as RabbitMQProducerConfig).Where(p => p.ProducerKey == producerKey && p.ProjectName == ProjectName).FirstOrDefault();
            var producer = _rabbitMQProducerConfigs.Where(p => p.ProducerKey == producerKey && p.ProjectName == ProjectName).FirstOrDefault();

            var config = producer.ConsumerConfigs.Select(c => c as RabbitMQConsumerConfig).Where(c => c.ConsumerKey == consumerKey).ToList().FirstOrDefault();

            var rmqc = new RabbitMQConsumer<T>(_channel, producerKey, consumerKey, config.QueueName, func);
            rmqc.Consume();

            ActiveConsumers.Add(rmqc);


            //func.Method.Name
        }

        private void RefreshConnection()
        {
            if (!_connection.IsOpen || _channel.IsClosed)
            {
                Initialize();
            }
        }

        //Dispose pattern in the MS recommended way:
        #region Dispose 
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    if (_channel.IsOpen) _channel.Close();
                    if (_connection.IsOpen) _connection.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RabbitMQManager()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion Dispose 

    }
}
