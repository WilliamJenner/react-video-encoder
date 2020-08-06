using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
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

        public VideoEncoderEngine()
        {
            FileType = "mp4";
            GenerateNewOutputFileName();
        }

        public async Task<bool> SetInputFile(IFormFile input)
        {
            try
            {
                InputVideo = await input.GetBytesAsync();
            }
            catch (Exception ex)
            {
               Log.Error(ex,"Error in SetInputFile()");
            }

            return false;
        }

        public async Task<bool> WriteVideo()
        {

            if (OutputFileName == string.Empty)
            {
                throw new Exception($"OutputFileName was {OutputFileName}");
            }

            try
            {
                
                {
                    var pipe = new StreamPipeSource(await InputVideo.ToMemoryStreamAsync());
                    var stopWatch = new Stopwatch();
                  Log.Information($"Starting writing input video to {this.OutputFileName}");
                    
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

                    Log.Information($"Finished writing video to {this.OutputFileName} in " +
                        $"{stopWatch.ElapsedMilliseconds.ToString("n3")}ms");
                    stopWatch.Stop();
                    return true;
                } 
            }
            catch (Exception ex)
            {
               Log.Error(ex,"Error in VideoEncoderEngine.WriteVideo()");
                
                return false;
            }   
        }

        public Task<byte[]> GetVideo()
        {
            if (InputVideo == null)
            {
                throw new NullReferenceException("An exception occured at GetVideo(), the video was null.");
            }

            return Task.FromResult(InputVideo);
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
