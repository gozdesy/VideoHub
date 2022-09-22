using Microsoft.AspNetCore.Mvc;
using VideoHub.API.Services;

namespace VideoHub.API.Controllers
{
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly ILogger<VideoController> _logger;
        private readonly IVideoService _videoService;
        public VideoController(ILogger<VideoController> logger, IVideoService videoService)
        {
            _logger = logger;
            _videoService = videoService;
        }

        [HttpGet]
        [Route("/videos")]
        public async Task<IActionResult> GetVideos()
        {
            //var items = await _vr.GetItems();
            //return Ok(String.Join(", ", items.Select(v => JsonSerializer.Serialize<Video>(v))));

            await Task.Delay(100);
            return Ok();
        }

        [HttpPost]
        [Route("/upload")]
        public async Task<IActionResult> PostVideo([FromForm] IFormFile file)
        {
            //var file = Request.Form.Files[0];
            var result = new ServiceResult();

            //file.ContentType

            using (MemoryStream stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                result = await _videoService.UploadVideo(stream.ToArray());
            }

            if (!result.Valid) return BadRequest();

            return Ok();
        }
    }
}