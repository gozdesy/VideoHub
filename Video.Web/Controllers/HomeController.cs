using Common.UpLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VideoHub.Web.Models;

namespace Video.Web
{
    public class HomeController : Controller
    {
        private readonly VideoHubWebSettings _settings;
        private readonly HttpClient _client;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IOptions<VideoHubWebSettings> settings, ILogger<HomeController> logger)
        {
            _settings = settings.Value;
            _client = new HttpClient();
            _logger = logger;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index() 
        {
            throw new ArgumentException("test new mongo user");

            await Task.Delay(100);
            return Ok("hello");
        }

        [HttpGet]
        [Route("/upload")]
        public async Task<IActionResult> UploadVideo()
        {

            return View("UploadVideo");
        }

        [HttpPost]
        [Route("/upload")]
        public async Task<IActionResult> PostVideo(IFormFile file)
        {
            var result = false;

            using (MemoryStream stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;

                using (var multipartFormContent = new MultipartFormDataContent()) 
                {
                    multipartFormContent.Add(new StreamContent(stream), "file", "file");

                    var response = await _client.PostAsync(_settings.ApiUrl + "/upload", multipartFormContent);
                }
            }

            if (!result) return BadRequest();

            return Ok();
        }
    }
}
