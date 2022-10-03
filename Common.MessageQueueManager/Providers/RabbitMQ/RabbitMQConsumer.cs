using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Common.MessageQueueManager.Providers.RabbitMQ
{
    public class RabbitMQConsumer<T> : DefaultBasicConsumer, IConsumer<T>
    {
        public string ProducerKey { get; set; }

        public string ConsumerKey { get; set; }
        
        public ReceivedFunc<T> ReceivedFunc { get; set; }

        private IModel _model;
        private string _queueName;

        public RabbitMQConsumer(IModel model, string producerKey, string consumerKey, string queueName, ReceivedFunc<T> receivedFunc) : base(model)
        {
            ProducerKey = producerKey;
            ConsumerKey = consumerKey;
            ReceivedFunc = receivedFunc;

            _model = model;
            _queueName = queueName;

            //model.BasicConsume(queueName, true, this);
        }

        public void Consume()
        {
            _model.BasicConsume(_queueName, false, this);
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            base.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);

            var message = Encoding.UTF8.GetString(body.ToArray());

            var data = JsonSerializer.Deserialize<T>(message);
            var receivedMessage = new ReceivedMessage<T>() { Data = data };

            ReceivedFunc(receivedMessage);
        }

       
    }

}
