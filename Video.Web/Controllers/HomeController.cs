using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VideoHub.Web.Models;

namespace Video.Web
{
    public class HomeController : Controller
    {
        private readonly VideoHubWebSettings _settings;
        private readonly HttpClient _client;

        public HomeController(IOptions<VideoHubWebSettings> settings)
        {
            _settings = settings.Value;
            _client = new HttpClient();
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index() 
        {
            return View();
            //return Problem()
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
