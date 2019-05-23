using System.IO;
using System.Text;

namespace LOT.Core
{
    public static class StreamHelper
    {

        // http://stackoverflow.com/questions/17801761/converting-stream-to-string-and-back-what-are-we-missing
        public static string StreamToString (Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader (stream, Encoding.UTF8)) {
                return reader.ReadToEnd ();
            }
        }

        public static Stream StringToStream (string src)
        {
            byte [] byteArray = Encoding.UTF8.GetBytes (src);
            return new MemoryStream (byteArray);
        }

        // Copied from: http://stackoverflow.com/questions/5730863/how-to-use-stream-copyto-on-net-framework-3-5
        public static void CopyTo (Stream input , Stream output)
        {
            byte [] buffer = new byte[16 * 1024];
            int bytesRead;
            while ((bytesRead = input.Read (buffer, 0, buffer.Length)) > 0) {
                output.Write (buffer, 0, bytesRead);
            }
        }

        public static byte[] ToArray (Stream stream)
        {
            using (var memoryStream = new MemoryStream ()) {
                stream.Seek (0, SeekOrigin.Begin);
                CopyTo (stream, memoryStream);
                return memoryStream.ToArray ();
            }
            return null;
        }
    }
}
