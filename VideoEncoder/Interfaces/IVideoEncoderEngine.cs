
using FFMpegCore.Arguments;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace VideoEncoderReact.VideoEncoder.Interfaces
{
    public interface IVideoEncoderEngine
    {
        public Task<bool> SetInputFile(IFormFile input);
        public Task<bool> WriteVideo();
        public Task<byte[]> GetVideo();


    }
}
