using FFMpegCore.Arguments;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace VideoEncoderReact.VideoEncoder.Interfaces
{
    public interface IVideoEncoderEngine
    {
        Task<bool> SetInputFile(IFormFile input);
        Task<bool> WriteVideo();
        Task<byte[]> GetVideo();
    }
}