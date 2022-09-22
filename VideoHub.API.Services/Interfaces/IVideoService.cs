namespace VideoHub.API.Services
{
    public interface IVideoService
    {
        Task<ServiceResult> UploadVideo(byte[] video);

        Task<ServiceResult> ValidateVideo(byte[] video);

    }
}