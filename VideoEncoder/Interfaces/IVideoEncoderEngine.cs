
using FFMpegCore.Arguments;
using System.Threading.Tasks;

namespace VideoEncoderReact.VideoEncoder.Interfaces
{
    public interface IVideoEncoderEngine
    {
        public Task<bool> SetInputFile(byte[] inputFile);
        public Task<bool> WriteVideo();
        public Task<byte[]> GetVideo();


    }
}
