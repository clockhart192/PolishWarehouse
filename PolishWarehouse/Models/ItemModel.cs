using PolishWarehouseData;
using System;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PolishWarehouse.Models
{
    public class ItemModel
    {
        public long? ID { get; set; }
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public Brand Brand { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public bool HasBeenTried { get; set; } = false;
        public bool WasGift { get; set; } = false;
        public string GiftFromName { get; set; }
        public string Notes { get; set; }

        public ItemModel() { }
    }

    public class ImageModel
    {
        public long? ID { get; set; }
        public long ItemID { get; set; }
        public string Image { get; set; }
        public string MimeType { get; set; }
        public string ImageForHTML { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public bool MakerImage { get; set; }
        public bool PublicImage { get; set; } = true;
        public bool DisplayImage { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }


        public Response Save(HttpPostedFileBase file)
        {

            using (var db = new PolishWarehouseEntities())
            {
                var polish = db.Polishes.Where(p => p.ID == ItemID).SingleOrDefault();
                if (polish == null)
                    return new Response(false, "Polish not found");

                MaxWidth = Convert.ToInt32(Utilities.GetConfigurationValue("Image max width"));
                MaxHeight = Convert.ToInt32(Utilities.GetConfigurationValue("Image max height"));

                //string basePath = HttpContext.Current.Server.MapPath($"/Content/PolishImages/{polish.Brand.Name.Replace(" ", "").Replace("&", "AND")}/{polish.Name.Replace(" ", "")}/{ID.ToString()}/");
                string basePath = $"/Content/PolishImages/{polish.Brand.ID.ToString()}/{polish.ID.ToString()}/{ID.ToString()}/";

                var image = db.Polishes_Images.Where(i => i.ID == ID).SingleOrDefault();
                var imgCount = 0;

                if (image == null)
                {
                    image = new Polishes_Images();
                    image.PolishID = ItemID;
                    image.Image = "";
                    image.MIMEType = file.ContentType;
                    image.PublicImage = PublicImage;
                    image.DisplayImage = DisplayImage;
                    image.MakerImage = MakerImage;
                    db.Polishes_Images.Add(image);
                    db.SaveChanges();
                }

                ID = image.ID;
                image.Description = Description;
                image.Notes = Notes;
                image.MakerImage = MakerImage;
                image.PublicImage = PublicImage;
                image.DisplayImage = DisplayImage;

                if (DisplayImage)//Kill the rest of the Display Images for this polish if this is the primary.
                {
                    var otherImages = db.Polishes_Images.Where(i => i.PolishID == ItemID && i.ID != ID).ToArray();
                    foreach (var otherImage in otherImages)
                    {
                        otherImage.DisplayImage = false;
                        imgCount++;
                    }
                }
                if (file != null && file.ContentLength > 0)
                {
                    //do image magic
                    var imageBase64 = Utilities.ConvertFileToBase64(file);
                    var compressedBase64 = Utilities.ResizeImage(imageBase64, MaxWidth, MaxHeight);
                    //var compressedImage = Utilities.ConvertBase64ToImage(compressedBase64);

                    //set up dirs
                    var path = $"{basePath}\\original\\";
                    var compressedPath = $"{basePath}\\compressed\\";

                    //Kill current images
                    if (Directory.Exists(HttpContext.Current.Server.MapPath(path)))
                    {
                        DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
                        foreach (FileInfo f in di.GetFiles())
                        {
                            f.Delete();
                        }
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }
                    }

                    if (Directory.Exists(HttpContext.Current.Server.MapPath(path)))
                    {
                        DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(compressedPath));
                        foreach (FileInfo f in di.GetFiles())
                        {
                            f.Delete();
                        }
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }
                    }

                    //Remake the folders
                    Directory.CreateDirectory((HttpContext.Current.Server.MapPath(path)));
                    Directory.CreateDirectory((HttpContext.Current.Server.MapPath(compressedPath)));

                    //Set up the paths
                    var imagePath = $"{path}{ID.Value.ToString()}_{imgCount.ToString()}_{Utilities.ToUnixTime(DateTime.Now).ToString()}{Path.GetExtension(file.FileName)}";
                    var compressedImagePath = $"{compressedPath}{ID.Value.ToString()}_{imgCount.ToString()}_{Utilities.ToUnixTime(DateTime.Now).ToString()}.jpeg";

                    //save
                    file.SaveAs(HttpContext.Current.Server.MapPath(imagePath));
                    var resp = Utilities.ConvertBase64ToImageAndSave(compressedBase64, HttpContext.Current.Server.MapPath(compressedImagePath));
                    //compressedImage.Save(compressedImagePath, ImageFormat.Jpeg);

                    //dump into db fields
                    image.Image = imageBase64;
                    image.MIMEType = file.ContentType;
                    image.CompressedImage = compressedBase64;
                    image.CompressedMIMEType = "image/jpeg";
                    image.ImagePath = imagePath;
                    image.CompressedImagePath = compressedImagePath;
                }

                db.SaveChanges();

                return new Response(true);
            }

        }

        public Response Save(System.Drawing.Image img)
        {
            if (img != null)
            {
                using (var db = new PolishWarehouseEntities())
                {
                    var polish = db.Polishes.Where(p => p.ID == ItemID).SingleOrDefault();
                    if (polish == null)
                        return new Response(false, "Polish not found");

                    string basePath = $"/Content/PolishImages/{polish.Brand.ID.ToString()}/{polish.ID.ToString()}/{ID.ToString()}/";

                    var image = db.Polishes_Images.Where(i => i.ID == ID).SingleOrDefault();
                    var imgCount = 0;

                    if (image == null)
                    {
                        image = new Polishes_Images();
                        image.PolishID = ItemID;
                        image.Image = "";
                        image.MIMEType = "image/jpeg";
                        image.PublicImage = PublicImage;
                        image.DisplayImage = DisplayImage;
                        image.MakerImage = MakerImage;
                        db.Polishes_Images.Add(image);
                        db.SaveChanges();
                    }

                    ID = image.ID;
                    image.Description = Description;
                    image.Notes = Notes;
                    image.MakerImage = MakerImage;
                    image.PublicImage = PublicImage;
                    image.DisplayImage = DisplayImage;

                    if (DisplayImage)//Kill the rest of the Display Images for this polish if this is the primary.
                    {
                        var otherImages = db.Polishes_Images.Where(i => i.PolishID == ItemID && i.ID != ID).ToArray();
                        foreach (var otherImage in otherImages)
                        {
                            otherImage.DisplayImage = false;
                            imgCount++;
                        }
                    }

                    //do image magic
                    var imageBase64 = Utilities.ConvertImageToBase64(img);
                    var compressedBase64 = Utilities.ResizeImage(imageBase64, MaxWidth, MaxHeight);
                    //var compressedImage = Utilities.ConvertBase64ToImage(compressedBase64);

                    //set up dirs
                    var path = $"{basePath}\\original\\";
                    var compressedPath = $"{basePath}\\compressed\\";

                    //Kill current images
                    if (Directory.Exists(HttpContext.Current.Server.MapPath(path)))
                    {
                        DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
                        foreach (FileInfo f in di.GetFiles())
                        {
                            f.Delete();
                        }
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }
                    }

                    if (Directory.Exists(HttpContext.Current.Server.MapPath(compressedPath)))
                    {
                        DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(compressedPath));
                        foreach (FileInfo f in di.GetFiles())
                        {
                            f.Delete();
                        }
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }
                    }

                    //Remake the folders
                    Directory.CreateDirectory((HttpContext.Current.Server.MapPath(path)));
                    Directory.CreateDirectory((HttpContext.Current.Server.MapPath(compressedPath)));

                    //Set up the paths
                    var imagePath = $"{path}{ID.Value.ToString()}_{imgCount.ToString()}_{Utilities.ToUnixTime(DateTime.Now).ToString()}.jpeg";
                    var compressedImagePath = $"{compressedPath}{ID.Value.ToString()}_{imgCount.ToString()}_{Utilities.ToUnixTime(DateTime.Now).ToString()}.jpeg";

                    //save
                    Utilities.ConvertBase64ToImageAndSave(imageBase64, HttpContext.Current.Server.MapPath(imagePath));
                    var resp = Utilities.ConvertBase64ToImageAndSave(compressedBase64, HttpContext.Current.Server.MapPath(compressedImagePath));
                    //compressedImage.Save(compressedImagePath, ImageFormat.Jpeg);

                    //dump into db fields
                    image.Image = imageBase64;
                    image.MIMEType = "image/jpeg";
                    image.CompressedImage = compressedBase64;
                    image.CompressedMIMEType = "image/jpeg";
                    image.ImagePath = imagePath;
                    image.CompressedImagePath = compressedImagePath;

                    db.SaveChanges();

                    return new Response(true);
                }
            }
            return new Response(false, "Image not found");
        }
        public Response Delete()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polish = db.Polishes.Where(p => p.ID == ItemID).SingleOrDefault();
                string basePath = HttpContext.Current.Server.MapPath($"/Content/PolishImages/{polish.Brand.Name.Replace(" ", "").Replace("&", "AND")}/{polish.Name.Replace(" ", "")}/{ID.ToString()}/");

                //Kill current images
                if (Directory.Exists(basePath))
                {
                    DirectoryInfo di = new DirectoryInfo(basePath);
                    foreach (FileInfo f in di.GetFiles())
                    {
                        f.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }

                var image = db.Polishes_Images.Where(p => p.ID == ID).SingleOrDefault();
                db.Polishes_Images.Remove(image);
                db.SaveChanges();
                return new Response(true);
            }
        }
    }
}