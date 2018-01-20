using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using PolishWarehouseData;
using Microsoft.VisualBasic.FileIO;

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

        public static bool processCSV(HttpPostedFileBase file, bool overwriteIfExists = false)
        {
            //db.Configuration.AutoDetectChangesEnabled = false;
            using (var reader = new TextFieldParser(file.InputStream))
            {
                reader.TextFieldType = FieldType.Delimited;
                reader.SetDelimiters(",");
                reader.ReadFields(); //Skip the header
                while (!reader.EndOfData)
                {
                    using (var db = new PolishWarehouseEntities())
                    {
                        string[] fields = reader.ReadFields();
                        var swatchWheel = fields[(int)Column.swatchWheel];

                        if (!db.Polishes.Any(p => p.Label == swatchWheel) || overwriteIfExists)//The swatch number doesn't exist
                        {
                            var swatchColor = fields[(int)Column.swatchColor];
                            var brand = fields[(int)Column.brand];
                            var colorID = db.Colors.Where(c => c.Name == swatchColor).Select(c => c.ID).SingleOrDefault();
                            var brandID = db.Brands.Where(b => b.Name == brand).Select(b => b.ID).SingleOrDefault();

                            //Add the polish
                            var polish = db.Polishes.Where(p => p.Label == swatchWheel).SingleOrDefault();
                            if (polish == null)
                            {
                                polish = new Polish();
                                db.Polishes.Add(polish);

                            }

                            var colornum = 0;
                            try
                            {
                                colornum = Convert.ToInt32(fields[(int)Column.swatchNum].Trim());
                            }
                            catch (Exception ex)
                            {

                                throw new Exception(Logging.LogEvent(LogTypes.Error, $"Swatch #{fields[(int)Column.swatchNum]} did not register as a number", $"Swatch #{fields[(int)Column.swatchNum]} did not register as a number", ex));
                            }

                            var coats = 0;
                            try
                            {
                                coats = Convert.ToInt32(fields[(int)Column.coats].Trim());
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(Logging.LogEvent(LogTypes.Error, $"Coat Value for swatch label { fields[(int)Column.swatchWheel]} did not register as a number.", $"Coat Value for swatch label { fields[(int)Column.swatchWheel]} did not register as a number.", ex));
                            }

                            polish.ColorID = colorID;
                            polish.BrandID = brandID;
                            polish.CreatedOn = DateTime.UtcNow;
                            polish.Name = fields[(int)Column.polishName];
                            polish.ColorNumber = colornum;
                            polish.Quantity = 1;
                            polish.Coats = coats;
                            polish.Label = fields[(int)Column.swatchWheel];
                            polish.HasBeenTried = !string.IsNullOrWhiteSpace(fields[(int)Column.tried]);
                            polish.WasGift = !string.IsNullOrWhiteSpace(fields[(int)Column.gift]);

                            db.SaveChanges();

                            //Add the additional info
                            var polishAdditional = db.Polishes_AdditionalInfo.Where(p => p.PolishID == polish.ID).SingleOrDefault();
                            if (polishAdditional == null)
                            {
                                polishAdditional = new Polishes_AdditionalInfo();
                                db.Polishes_AdditionalInfo.Add(polishAdditional);
                            }
                            polishAdditional.PolishID = polish.ID;
                            polishAdditional.Description = fields[(int)Column.desc];

                            db.SaveChanges();

                            //Check to see if we have a valid polish type and add it.
                            var type = fields[(int)Column.type];
                            var ptype = db.PolishTypes.Where(p => p.Name == type).SingleOrDefault();
                            if (ptype != null)
                            {
                                var polishType = db.Polishes_PolishTypes.Where(p => p.PolishID == polish.ID && p.PolishTypeID == ptype.ID).SingleOrDefault();
                                if (polishType == null)//Add this type/polish combo if it did not already exist.
                                {
                                    polishType = new Polishes_PolishTypes()
                                    {
                                        PolishID = polish.ID,
                                        PolishTypeID = ptype.ID
                                    };
                                    db.Polishes_PolishTypes.Add(polishType);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public enum Column
        {
            brand = 0,
            polishName = 1,
            pintrest = 2,
            desc = 3,
            type = 4,
            swatchWheel = 5,
            swatchColor = 6,
            swatchNum = 7,
            coats = 8,
            tried = 9,
            gift = 10,
            notes = 11
        }
    }
}