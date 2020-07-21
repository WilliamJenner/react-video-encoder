using Microsoft.AspNetCore.Http;
using System;
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
    }
}
