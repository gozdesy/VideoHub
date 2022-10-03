using Common.MessageQueueManager;
using Imagekit;
using Microsoft.Extensions.Logging;
using VideoHub.Message.Models;

namespace VideoHub.API.Services
{
    public class VideoService : IVideoService
    {
        private readonly HttpClient _client;
        private readonly ILogger<VideoService> _logger;
        private readonly IMessageQueueManager _messageQueueManager;

        public VideoService(ILogger<VideoService> logger, IMessageQueueManager messageQueueManager)
        {
            _client = new HttpClient();
            _logger = logger;   
            _messageQueueManager = messageQueueManager;
        }

        public async Task<ServiceResult> UploadVideo(byte[] video) 
        {
            var result = new ServiceResult();
            
            try
            {
                var imagekit = new ServerImagekit("public_Iptr3N3sgz6LTtwdCA4KReJY4VE=", "private_/aaei3EWQK7lGILjnUZGM7ONn1s=", "https://ik.imagekit.io/qqyj0ycll", "path");
                imagekit.FileName("test2.mov");
                var response = await imagekit.UploadAsync(video);

                _messageQueueManager.Send(ProducerKey.VideoUploaded, new MessageVideoUploaded() { VideoUrl = response.URL });
            }
            catch (Exception ex)
            {
                result.Valid = false;
                _logger.LogError(ex, null);
            }
            
            return result;
        }

        public async Task<ServiceResult> ValidateVideo(byte[] video) 
        {
            var result = new ServiceResult();
            
            try
            {
                
            }
            catch (Exception)
            {
                result.Valid = false;
            }
            
            return result;
        }
    }
}