using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VideoEncoderReact.Extensions
{
    public static class ByteExtensions
    {
        public static async Task<bool> ToFileAsync(this byte[] byteArray, string fileName)
        {
            try
            {
                await using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                await fs.WriteAsync(byteArray, 0, byteArray.Length);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in ToFile", ex);
                return false;
            }
        }

        public static async Task<MemoryStream> ToMemoryStreamAsync(this byte[] byteArray)
        {
            try
            {
                // src https://stackoverflow.com/a/11528766
                MemoryStream stream = new MemoryStream();

                await stream.WriteAsync(byteArray);
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in ToMemoryStreamAsync", ex);
                return null;
            }
        }
    }
}
