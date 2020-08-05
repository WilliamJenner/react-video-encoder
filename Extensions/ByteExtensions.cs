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
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    await fs.WriteAsync(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in ToFile", ex);
                return false;
            }
        }

        public static MemoryStream ToMemoryStream(this byte[] byteArray)
        {
            try
            {
                // Probably add a comment here stating that the lack of using statements
                // is deliberate.
                MemoryStream stream = new MemoryStream();

                StreamWriter writer = new StreamWriter(stream);
                // Code that writes stuff to the memorystream via streamwriter
                writer.Write(byteArray);

                writer.Flush();
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
