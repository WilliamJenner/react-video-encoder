﻿using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.IO.Pipes;
using System.Threading.Tasks;
using VideoEncoderReact.VideoEncoder.Interfaces;

namespace VideoEncoderReact.VideoEncoder
{
    public class VideoEncoderEngine : IVideoEncoderEngine
    {
        private string InputFileName;
        private string OutputFileName;
        private string FileType;
        private MediaAnalysis mediaAnalysis;
        private byte[] video;
        private readonly ILogger<VideoEncoderEngine> _logger;

        public VideoEncoderEngine(ILogger<VideoEncoderEngine> logger)
        {
            this._logger = logger;
            this.FileType = "mp4";

            GenerateNewInputFileName(); 
            GenerateNewOutputFileName();
        }

        public async Task<bool> SetInputFile(byte[] inputFile)
        {
            var stream = new FileStream(this.InputFileName, FileMode.Create);
            stream.Write(inputFile);

            var mediaInfo = await FFProbe.AnalyseAsync(stream);

            this.mediaAnalysis = mediaInfo;

            return mediaInfo != null;
        }

        public async Task<bool> WriteVideo()
        {

            if (this.InputFileName == string.Empty || this.OutputFileName == string.Empty)
            {
                throw new Exception($"InputFileName was {InputFileName} and OutputFileName was {OutputFileName}");
            }

            try
            {
                if (File.Exists(this.InputFileName))
                {
                    var stopWatch = new Stopwatch();
                    _logger.LogInformation($"Starting writing input video {this.InputFileName} to {this.OutputFileName}");
                    var fileInfo = new FileInfo(this.InputFileName);
                    stopWatch.Start();
                    await FFMpegArguments
                                    .FromInputFiles(fileInfo)
                                    .WithVideoCodec(VideoCodec.LibX264)
                                    .WithConstantRateFactor(21)
                                    .WithAudioCodec(AudioCodec.Aac)
                                    .WithVariableBitrate(4)
                                    .WithFastStart()
                                    .Scale(VideoSize.Hd)
                                    .OutputToFile(this.OutputFileName)
                                    .ProcessAsynchronously();

                    _logger.LogInformation($"Finished writing video to {this.OutputFileName} in {stopWatch.ElapsedMilliseconds.ToString("n3")}ms");
                    stopWatch.Stop();
                    return true;
                } else
                {
                    throw new FileNotFoundException($"File not found: {this.InputFileName}");
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
            if (this.video == null)
            {
                throw new NullReferenceException("An exception occured at GetVideo(), the video was null.");
            }

            return this.video;
        }

        #region Private Methods

        private void ResetInputFile()
        {
            File.Delete(this.InputFileName);
            this.GenerateNewInputFileName();
        }

        private void GenerateNewInputFileName()
        {
            this.InputFileName = Path.Combine("C:\\Users\\Will\\Documents", "Wildlife.avi");
        }

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

            this.OutputFileName = $"{Guid.NewGuid()}.{this.FileType}";
        }

        #endregion

    }
}
