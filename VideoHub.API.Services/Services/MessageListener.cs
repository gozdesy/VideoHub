using Common.MessageQueueManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VideoHub.Message.Models;

namespace VideoHub.API.Services
{
    public class MessageListener : IMessageListener
    {

        private readonly ILogger<VideoService> _logger;
        private readonly IMessageQueueManager _messageQueueManager;
        private readonly IServiceProvider _serviceProvider;
        public MessageListener(ILogger<VideoService> logger, IMessageQueueManager messageQueueManager, IServiceProvider serviceProvider)
        {

            _logger = logger;   
            _messageQueueManager = messageQueueManager;
            _serviceProvider = serviceProvider;
        }

        public void SetMessageReceivers()
        {
            _messageQueueManager.Receive<MessageTranscodeCompleted>(ProducerKey.TranscodeCompleted, ConsumerKey.VideoUpdate, VideoUpdate);
            //_messageQueueManager.Receive("Notification.Listen", NotificationListen);
        }

        private void VideoUpdate(ReceivedMessage<MessageTranscodeCompleted> receivedMessage)
        {
            using (var scope = _serviceProvider.CreateScope()) {

                var videoService = scope.ServiceProvider.GetRequiredService<IVideoService>();
                //videoService.UpdateVideo
                Console.WriteLine("UpdateVideo Started : " + receivedMessage.Data.VideoUrl);
            }
            Console.WriteLine("UpdateVideo Ended");
        }
    }
}