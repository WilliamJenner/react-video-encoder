using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using VideoEncoderReact.Extensions;
using VideoEncoderReact.VideoEncoder.Interfaces;

namespace VideoEncoderReact.Controllers
{
    [ApiController]
    [Route("api/VideoEncoder")]
    public class VideoEncoderController : ControllerBase
    {
        private readonly IVideoEncoderEngine _videoEncoderEngine;

        public VideoEncoderController( IVideoEncoderEngine videoEncoderEngine)
        {
            _videoEncoderEngine = videoEncoderEngine;
        }

        [HttpPost("SetInputVideo")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> SetInputVideo()
        {
            try
            {
                await _videoEncoderEngine.SetInputFile(HttpContext.Request.Form.Files.First());
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error("Exception bubbled up to controller, SetInputVideo(byte[] input)", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("StartWrite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> StartWrite()
        {
            try
            {
                return Ok(await _videoEncoderEngine.WriteVideo());
            }
            catch (Exception ex)
            {
                Log.Error("Exception bubbled up to controller, StartWrite()", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
