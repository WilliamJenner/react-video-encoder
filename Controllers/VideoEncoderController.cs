using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VideoEncoderReact.VideoEncoder.Interfaces;

namespace VideoEncoderReact.Controllers
{
    [ApiController]
    [Route("api/VideoEncoder")]
    public class VideoEncoderController : ControllerBase
    {
        private readonly ILogger<VideoEncoderController> _logger;
        private readonly IVideoEncoderEngine _videoEncoderEngine;

        public VideoEncoderController(ILogger<VideoEncoderController> logger, IVideoEncoderEngine videoEncoderEngine)
        {
            _logger = logger;
            _videoEncoderEngine = videoEncoderEngine;
        }

        [HttpPost("SetInputVideo")]
        public async Task<IActionResult> SetInputVideo([FromBody] byte[] input)
        {
            try
            {
                return Ok(await _videoEncoderEngine.SetInputFile(input));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception bubbled up to controller, SetInputVideo(byte[] input)", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("WriteVideo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> StartWrite()
        {
            try
            {
                return Ok(await _videoEncoderEngine.WriteVideo());
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception bubbled up to controller, StartWrite()", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
