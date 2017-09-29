using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace PolishWarehouse.Models
{
    public class Utilities
    {
        public static string ReduceImage(string image, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            byte[] bytes = Convert.FromBase64String(image);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (Image img = Image.FromStream(ms))
                {
                    using (MemoryStream target = new MemoryStream())
                    {
                        img.Save(target, jpegCodec, encoderParams);
                        byte[] data = target.ToArray();
                        var fileBase64 = Convert.ToBase64String(data);

                        return fileBase64;
                    }
                }
            }
        }

        public static string ResizeImage(string image, int maxWidth, int MaxHeight)
        {
            byte[] bytes = Convert.FromBase64String(image);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (Image img = Image.FromStream(ms))
                {
                    using (Image resized = new Bitmap(img, ResizeKeepAspect(img.Size, maxWidth, MaxHeight)))
                    {
                        using (MemoryStream target = new MemoryStream())
                        {
                            resized.Save(target, ImageFormat.Jpeg);
                            byte[] data = target.ToArray();
                            var fileBase64 = Convert.ToBase64String(data);

                            return fileBase64;
                        }
                    }
                }
            }
        }

        public static string ReduceAndResizeImage(string image, int quality, int maxWidth, int MaxHeight)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            byte[] bytes = Convert.FromBase64String(image);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (Image img = Image.FromStream(ms))
                {
                    using (Image resized = new Bitmap(img, ResizeKeepAspect(img.Size, maxWidth, MaxHeight)))
                    {
                        using (MemoryStream target = new MemoryStream())
                        {
                            resized.Save(target, jpegCodec, encoderParams);
                            byte[] data = target.ToArray();
                            var fileBase64 = Convert.ToBase64String(data);

                            return fileBase64;
                        }
                    }
                }
            }
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }

        public static Size ResizeKeepAspect(Size src, int maxWidth, int maxHeight)
        {
            decimal rnd = Math.Min(maxWidth / (decimal)src.Width, maxHeight / (decimal)src.Height);
            return new Size((int)Math.Round(src.Width * rnd), (int)Math.Round(src.Height * rnd));
        }
    }
}