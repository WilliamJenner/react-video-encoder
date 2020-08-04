using FFMpegCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VideoEncoderReact.Extensions
{
    public static class FormFileExtensions
    {
        // Source: https://stackoverflow.com/questions/36432028/how-to-convert-a-file-into-byte-array-in-memory#answer-59359240
        public static async Task<byte[]> GetBytesAsync(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static byte[] GetBytes(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        //public static async Task<byte[]> ReadStream (this IFormFile formFile)
        //{
        //    const int twoKb = 2048;

        //    using (var input = formFile.OpenReadStream())
        //    using (Stream file = File.Create(filename))
        //    {
        //        CopyStream(input, file);
        //    }

        //    return new byte[] { };
        //}

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}
