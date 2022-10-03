using Common.MessageQueueManager;
using VideoHub.Message.Models;

namespace VideoHub.API.Services
{
    public class VideoHubContext
    {
        public List<IProducerConfig> ProducerConfigs { get; set; }

        public VideoHubContext(string mqProjectName)
        {
            SetMessageQueueContext(mqProjectName);
        }

        private void SetMessageQueueContext(string mqProjectName) 
        {
            ProducerConfigs = new List<IProducerConfig>();

            ProducerConfigs.Add(new ProducerConfig()
            {
                ProjectName = mqProjectName,
                ProducerKey = ProducerKey.VideoUploaded,
                ConsumerConfigs = {
                    new ConsumerConfig() { ConsumerKey = ConsumerKey.TranscodeStart }   
                } 
            });

            ProducerConfigs.Add(new ProducerConfig()
            {
                ProjectName = mqProjectName,
                ProducerKey = ProducerKey.TranscodeCompleted,
                ConsumerConfigs = {
                    new ConsumerConfig() { ConsumerKey = ConsumerKey.VideoUpdate }
                }
            });
        }
    }
}
