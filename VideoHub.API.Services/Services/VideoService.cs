using Imagekit;

namespace VideoHub.API.Services
{
    public class VideoService : IVideoService
    {
        private readonly HttpClient _client;  

        public VideoService()
        {
            _client = new HttpClient();
        }

        public async Task<ServiceResult> UploadVideo(byte[] video) 
        {
            var result = new ServiceResult();
            
            try
            {
                var imagekit = new ServerImagekit(
                "public_Iptr3N3sgz6LTtwdCA4KReJY4VE=",
                "private_/aaei3EWQK7lGILjnUZGM7ONn1s=",
                "https://ik.imagekit.io/qqyj0ycll",
                "path"
            );
                imagekit.FileName("test2.mov");
                var response = await imagekit.UploadAsync(video);
            }
            catch (Exception ex)
            {
                result.Valid = false;
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