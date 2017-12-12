using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Common.Infrastructure.Utilities
{
    public static class ImageUtil
    {
        public static Byte[] ConvertImageFileToBytes(string fileFullName)
        {
            FileUtil.CheckFileEixsts(fileFullName);
            return File.ReadAllBytes(fileFullName);
        }

        public static Byte[] ConvertImageToBytes(this BitmapImage bitMap)
        {
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitMap));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        public static BitmapImage ConvertBytesToBitMapImage(this byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();

                return image;
            }
        }

        public static string ConvertByteToString(this byte[] bytes)
        {
            return bytes != null ? Encoding.UTF8.GetString(bytes) : null;
        }
    }
}
