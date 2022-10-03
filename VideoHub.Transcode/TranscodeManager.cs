using Common.MessageQueueManager;
using VideoHub.Message.Models;

namespace VideoHub.Transcode
{
    public class TranscodeManager : ITranscodeManager
    {
        private readonly IMessageQueueManager _messageQueueManager;

        public TranscodeManager(IMessageQueueManager messageQueueManager)
        {
            _messageQueueManager = messageQueueManager;
        }

        internal void SetMessageReceivers() 
        {
            _messageQueueManager.Receive<MessageVideoUploaded>(ProducerKey.VideoUploaded, ConsumerKey.TranscodeStart, TranscodeStart);
        } 

        private void TranscodeStart(ReceivedMessage<MessageVideoUploaded> receivedMessage) 
        {
            Console.WriteLine("Transcode Started : " + receivedMessage.Data.VideoUrl);

            Transcode(receivedMessage.Data.VideoUrl);

            Console.WriteLine("Transcode Ended");
        }

        public void Transcode(string videoUrl) 
        {
            _messageQueueManager.Send(ProducerKey.TranscodeCompleted, new MessageTranscodeCompleted() { VideoUrl = videoUrl });
        }
        
    }

    
    
}