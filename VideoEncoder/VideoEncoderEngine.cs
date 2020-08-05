using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.IO.Pipes;
using System.Threading.Tasks;
using VideoEncoderReact.Extensions;
using VideoEncoderReact.VideoEncoder.Interfaces;

namespace VideoEncoderReact.VideoEncoder
{
    public class VideoEncoderEngine : IVideoEncoderEngine
    {
        private string OutputFileName;
        private string FileType;
        private byte[] InputVideo;
        private readonly ILogger<VideoEncoderEngine> _logger;

        public VideoEncoderEngine(ILogger<VideoEncoderEngine> logger)
        {
            this._logger = logger;
            this.FileType = "mp4";
            GenerateNewOutputFileName();
        }

        public async Task<bool> SetInputFile(IFormFile input)
        {
            try
            {
                this.InputVideo = await input.GetBytesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SetInputFile()", ex);
            }

            return false;
        }

        public async Task<bool> WriteVideo()
        {

            if (this.OutputFileName == string.Empty)
            {
                throw new Exception($"OutputFileName was {OutputFileName}");
            }

            try
            {
                
                {
                    var pipe = new StreamPipeSource(InputVideo.ToMemoryStream());
                    var stopWatch = new Stopwatch();
                    _logger.LogInformation($"Starting writing input video to {this.OutputFileName}");
                    
                    stopWatch.Start();
                    await FFMpegArguments
                                    .FromPipe(pipe)
                                    .WithVideoCodec(VideoCodec.LibX264)
                                    .WithConstantRateFactor(21)
                                    .WithAudioCodec(AudioCodec.Aac)
                                    .WithVariableBitrate(4)
                                    .WithFastStart()
                                    .Scale(VideoSize.Hd)
                                    .OutputToFile(this.OutputFileName)
                                    .ProcessAsynchronously();

                    _logger.LogInformation($"Finished writing video to {this.OutputFileName} in " +
                        $"{stopWatch.ElapsedMilliseconds.ToString("n3")}ms");
                    stopWatch.Stop();
                    return true;
                } 
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in VideoEncoderEngine.WriteVideo()", ex);
                
                return false;
            }   
        }

        public async Task<byte[]> GetVideo()
        {
            if (this.InputVideo == null)
            {
                throw new NullReferenceException("An exception occured at GetVideo(), the video was null.");
            }

            return this.InputVideo;
        }

        #region Private Methods

        private void ResetOutputFile()
        {
            File.Delete(this.OutputFileName);
            this.GenerateNewOutputFileName();
        }

        private void GenerateNewOutputFileName()
        {
            if (this.FileType == null)
            {
                throw new NullReferenceException("this.FileType was null in VideoEncoderEngine.GenerateNewOutputFileName()");
            }

            this.OutputFileName = $"output/{Guid.NewGuid()}.{this.FileType}";
        }

        #endregion

    }
}
