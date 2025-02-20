//using System.Net;
//using BackendYourList.Data;
//using BackendYourList.Models;
//using BackendYourList.Models.Entities;
//using BackendYourList.Services.Interface;
//using Microsoft.AspNetCore.Mvc;
//using YoutubeExplode.Videos.Streams;
//using YoutubeExplode;
//using Swashbuckle.AspNetCore.Annotations;

//namespace BackendYourList.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class VideoDownloadController : ControllerBase
//    {
//        private readonly YoutubeClient _youtubeClient;
//        private readonly ILogger<VideoDownloadController> _logger;

//        public VideoDownloadController(ILogger<VideoDownloadController> logger)
//        {
//            _youtubeClient = new YoutubeClient();
//            _logger = logger;
//        }

//        [HttpPost("download")]
//        [SwaggerOperation(
//            Summary = "Download YouTube Video",
//            Description = "Download a YouTube video in 720p quality")]
//        [SwaggerResponse((int)HttpStatusCode.OK, "Video downloaded successfully", typeof(FileResult))]
//        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Server error during download")]
//        public async Task<IActionResult> DownloadVideo([FromBody] VideoDownloadRequest request)
//        {
//            try
//            {
//                _logger.LogInformation($"Download request for URL: {request.Url}");

//                // Get video metadata
//                var video = await _youtubeClient.Videos.GetAsync(request.Url);

//                // Get available streams
//                var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(request.Url);

//                // Select 720p video stream
//                // Select 720p video stream
//                var videoStream = streamManifest.GetVideoStreams()
//                    .Where(s => s.Container == Container.Mp4 && s.VideoQuality == VideoQuality.High720)
//                    .FirstOrDefault();

//                if (videoStream == null)
//                {
//                    _logger.LogWarning($"No 720p stream available for video: {request.Url}");
//                    return BadRequest(new { message = "720p stream not available" });
//                }

//                // Download the stream
//                using var stream = await _youtubeClient.Videos.Streams.GetAsync(videoStream);

//                // Generate filename
//                string filename = $"{video.Title}_720p.mp4"
//                    .Replace(" ", "_")
//                    .Replace("|", "_")
//                    .Replace("/", "_")
//                    .Replace("\\", "_");

//                _logger.LogInformation($"Preparing to download video: {filename}");

//                // Return file
//                return File(stream, "video/mp4", filename);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error during video download");
//                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
//            }
//        }
//    }

//    /// <summary>
//    /// Video Download Request Model
//    /// </summary>
//    public class VideoDownloadRequest
//    {
//        /// <summary>
//        /// YouTube Video URL
//        /// </summary>
//        [SwaggerSchema("YouTube video URL to download")]
//        public string Url { get; set; }
//    }
//}
