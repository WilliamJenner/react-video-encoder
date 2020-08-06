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
        private string _outputFileName;
        private string _fileType; //shut up ReSharper this won't be readonly forever
        private byte[] _inputVideo;

        public VideoEncoderEngine()
        {
            _fileType = "mp4";
            GenerateNewOutputFileName();
        }

        public async Task<bool> SetInputFile(IFormFile input)
        {
            try
            {
                _inputVideo = await input.GetBytesAsync();
            }
            catch (Exception ex)
            {
               Log.Error(ex,"Error in SetInputFile()");
            }

            return false;
        }

        public async Task<bool> WriteVideo()
        {

            if (_outputFileName == string.Empty)
            {
                throw new Exception($"OutputFileName was {_outputFileName}");
            }

            try
            {
                
                {
                    var pipe = new StreamPipeSource(await _inputVideo.ToMemoryStreamAsync());
                    var stopWatch = new Stopwatch();
                  Log.Information($"Starting writing input video to {this._outputFileName}");
                    
                    stopWatch.Start();
                    await FFMpegArguments
                                    .FromPipe(pipe)
                                    .WithVideoCodec(VideoCodec.LibX264)
                                    .WithConstantRateFactor(21)
                                    .WithAudioCodec(AudioCodec.Aac)
                                    .WithVariableBitrate(4)
                                    .WithFastStart()
                                    .Scale(VideoSize.Hd)
                                    .OutputToFile(this._outputFileName)
                                    .ProcessAsynchronously();

                    Log.Information($"Finished writing video to {this._outputFileName} in " +
                        $"{stopWatch.ElapsedMilliseconds:n3}ms");
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
            if (_inputVideo == null)
            {
                throw new NullReferenceException("An exception occured at GetVideo(), the video was null.");
            }

            return Task.FromResult(_inputVideo);
        }

        #region Private Methods

        private void ResetOutputFile()
        {
            File.Delete(this._outputFileName);
            this.GenerateNewOutputFileName();
        }

        private void GenerateNewOutputFileName()
        {
            if (this._fileType == null)
            {
                throw new NullReferenceException("this.FileType was null in VideoEncoderEngine.GenerateNewOutputFileName()");
            }

            this._outputFileName = $"output/{Guid.NewGuid()}.{this._fileType}";
        }

        #endregion

    }
}
