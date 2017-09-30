using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using PolishWarehouseData;

namespace PolishWarehouse.Models
{
    public class Utilities
    {
        #region Image Manipulation
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

        public static string ConvertFileToBase64(HttpPostedFileBase file)
        {
            try
            {
                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] data = target.ToArray();
                var fileBase64 = Convert.ToBase64String(data);
                return fileBase64;
            }
            catch (Exception ex)
            {
                return Logging.LogEvent(LogTypes.Error, "Error Converting File to Base64", "Error occured converting file", ex);
            }
        }

        public static string ConvertImageToBase64(Image image)
        {
            try
            {
                using (MemoryStream target = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(image);
                    bmp.Save(target, ImageFormat.Jpeg);
                    byte[] data = target.ToArray();
                    var fileBase64 = Convert.ToBase64String(data);

                    return fileBase64;
                }

            }
            catch (Exception ex)
            {
                return Logging.LogEvent(LogTypes.Error, "Error Converting Image to Base64", "Error occured converting file", ex);
            }
        }

        public static Image ConvertBase64ToImage(string image)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(image);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    Image img = Image.FromStream(ms);
                    return img;
                }
            }
            catch (Exception ex)
            {
                Logging.LogEvent(LogTypes.Error, "Error Converting Image to Base64", "Error occured converting file", ex);
                return null;
            }
        }

        public static bool ConvertBase64ToImageAndSave(string image, string path)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(image);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (Image img = Image.FromStream(ms))
                    {
                        img.Save(path, ImageFormat.Jpeg);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogEvent(LogTypes.Error, "Error Converting Image to Base64", "Error occured converting file", ex);
                return false;
            }
        }

        public static string MakeImageForHtml(string image, string mimeType)
        {
            return $"data:{mimeType} ;base64,{image}";
        }

        public static Response ReSaveAllImages()
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    var maxWidth = Convert.ToInt32(Utilities.GetConfigurationValue("Image max width"));
                    var maxHeight = Convert.ToInt32(Utilities.GetConfigurationValue("Image max height"));

                    var images = db.Polishes_Images.Select(i => new PolishImageModel()
                    {
                        ID = i.ID,
                        PolishID = i.PolishID,
                        Image = i.Image,
                        MimeType = i.MIMEType,
                        //ImagePath = i.ImagePath,
                        //CompressedImage =i.CompressedImage,
                        //CompressedMIMEType =i.CompressedMIMEType,
                        //CompressedImagePath =i.CompressedImagePath,
                        Description = i.Description,
                        Notes = i.Notes,
                        MakerImage = i.MakerImage.HasValue ? i.MakerImage.Value : false,
                        PublicImage = i.PublicImage,
                        DisplayImage = i.DisplayImage.HasValue ? i.DisplayImage.Value : false,
                        MaxWidth = maxWidth,
                        MaxHeight = maxHeight
                    }).ToArray();

                    foreach (var image in images)
                    {
                        var i = ConvertBase64ToImage(image.Image);
                        image.Save(i);
                    }
                }
                catch (Exception ex)
                {
                    return new Response(false, Logging.LogEvent(LogTypes.Error, $"Error resaving all images", $"Error resaving all images", ex));
                }
                return new Response(true);
            }
        }
        #endregion

        #region Configuration
        public static string GetConfigurationValue(string key)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    return db.Settings.Where(s => s.KeyName == key).Select(s => s.KeyValue).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    return Logging.LogEvent(LogTypes.Error, $"Error getting configuration: {key}", $"Error getting configuration: {key}", ex);
                }
            }
        }
        #endregion

        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}