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
    public class StampingPlateModel
    {
        public long? ID { get; set; }
        public string Name { get; set; }
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public int? Quantity { get; set; } = 1;
        public bool HasBeenTried { get; set; } = false;
        public bool WasGift { get; set; } = false;
        public string GiftFromName { get; set; }
        public string Notes { get; set; }
        public StampingPlateShape StampingPlateShape { get; set; }
        public StampingPlateDesign[] StampingPlateDesigns { get; set; }
        public StampingPlateTheme[] StampingPlateThemes { get; set; }
        public int StampingPlateShapeID { get; set; }
        public string StampingPlateShapeName { get; set; }
        public int[] StampingPlateDesignIDs { get; set; }
        public int[] StampingPlateThemeIDs { get; set; }
        public StampingPlateImageModel[] Images { get; set; }

        public StampingPlateModel() { }
        public StampingPlateModel(int? id, bool colors = true, bool returnimages = false, bool forPublicView = true)
        {
            if (!id.HasValue)
                return;

            using (var db = new PolishWarehouseEntities())
            {
                var useOriginal = false;
                var useDatabase = false;
                try
                {
                    useOriginal = Convert.ToBoolean(Utilities.GetConfigurationValue("Use original quality image"));
                    useDatabase = Convert.ToBoolean(Utilities.GetConfigurationValue("Use database image"));
                }
                catch (Exception ex)
                {
                    Logging.LogEvent(LogTypes.Error, "Error getting image config settings", "Error getting image config settings", ex);
                }

                var p = db.StampingPlates.Where(po => po.ID == id).SingleOrDefault();

                ID = p.ID;
                BrandID = p.BrandID;
                Name = p.Name;
                BrandName = p.Brand.Name;
                Quantity = p.Quantity;
                WasGift = p.WasGift;
                GiftFromName = p.StampingPlates_AdditionalInfo.GiftFromName;
                Notes = p.StampingPlates_AdditionalInfo.Notes;
                StampingPlateShapeID = p.ShapeID;
                StampingPlateShapeName = p.StampingPlateShape.Name;
                StampingPlateDesigns = colors ? p.StampingPlates_StampingPlateDesigns.Select(ppt => ppt.StampingPlateDesign).ToArray() : null;
                StampingPlateThemes = colors ? p.StampingPlates_StampingPlateThemes.Select(ppt => ppt.StampingPlateTheme).ToArray() : null;
                //Types = colors ? p.StampingPlates_PolishTypes.Select(ppt => ppt.PolishType).ToArray() : null;

                if (returnimages)
                {
                    Images = db.StampingPlates_Images.Where(i => i.StampingPlateID == id && (forPublicView ? i.PublicImage : true)).Select(i => new StampingPlateImageModel()
                    {
                        ID = i.ID,
                        StampingPlateID = i.StampingPlateID,
                        //Image = i.Image,
                        //MimeType = i.MIMEType,
                        ImageForHTML = (useDatabase ? (useOriginal ? "data:" + i.MIMEType + ";base64," + i.Image : "data:" + i.CompressedMIMEType + ";base64," + i.CompressedImage) : (useOriginal ? i.ImagePath : i.CompressedImagePath)),
                        Description = i.Description,
                        Notes = i.Notes,
                        MakerImage = i.MakerImage.HasValue ? i.MakerImage.Value : false,
                        PublicImage = i.PublicImage,
                        DisplayImage = i.DisplayImage.HasValue ? i.DisplayImage.Value : false
                    }).ToArray();
                }
            }
        }
        public StampingPlateModel(StampingPlate p, bool colors = false, bool returnimages = false, bool forPublicView = true)
        {
            var useOriginal = false;
            var useDatabase = false;
            try
            {
                useOriginal = Convert.ToBoolean(Utilities.GetConfigurationValue("Use original quality image"));
                useDatabase = Convert.ToBoolean(Utilities.GetConfigurationValue("Use database image"));
            }
            catch (Exception ex)
            {
                Logging.LogEvent(LogTypes.Error, "Error getting image config settings", "Error getting image config settings", ex);
            }
            using (var db = new PolishWarehouseEntities())
            {
                ID = p.ID;
                BrandID = p.BrandID;
                Name = p.Name;
                BrandName = p.Brand.Name;
                Quantity = p.Quantity;
                WasGift = p.WasGift;
                GiftFromName = p.StampingPlates_AdditionalInfo.GiftFromName;
                Notes = p.StampingPlates_AdditionalInfo.Notes;
                //Types = colors ? p.StampingPlates_PolishTypes.Select(ppt => ppt.PolishType).ToArray() : null;

                if (returnimages)
                {
                    Images = db.StampingPlates_Images.Where(i => i.StampingPlateID == ID && (forPublicView ? i.PublicImage : true)).Select(i => new StampingPlateImageModel()
                    {
                        ID = i.ID,
                        StampingPlateID = i.StampingPlateID,
                        //Image = i.Image,
                        //MimeType = i.MIMEType,
                        ImageForHTML = (useDatabase ? (useOriginal ? "data:" + i.MIMEType + ";base64," + i.Image : "data:" + i.CompressedMIMEType + ";base64," + i.CompressedImage) : (useOriginal ? i.ImagePath : i.CompressedImagePath)),
                        Description = i.Description,
                        Notes = i.Notes,
                        MakerImage = i.MakerImage.HasValue ? i.MakerImage.Value : false,
                        PublicImage = i.PublicImage,
                        DisplayImage = i.DisplayImage.HasValue ? i.DisplayImage.Value : false
                    }).ToArray();
                }
            }
        }

        public static StampingPlateModel GetRandom(string brand)
        {
            using (var db = new PolishWarehouseEntities())
            {
                var stampingPlates = db.StampingPlates.Where(p => p.StampingPlates_DestashInfo == null).ToArray();//Get all non-destash polish.

                //filter brands.
                if (!string.IsNullOrWhiteSpace(brand))
                {
                    var brandFilter = stampingPlates.Where(p => p.Brand.Name == brand).ToArray();
                    stampingPlates = brandFilter;
                }

                Random rnd = new Random();
                int count = rnd.Next(stampingPlates.Count());

                var final = (StampingPlate)stampingPlates.GetValue(count);

                return new StampingPlateModel(final);

            }

        }

        public static StampingPlateShape[] GetPlateShapes()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.StampingPlateShapes.ToArray();
            }
        }

        public static StampingPlateDesign[] GetPlateDesigns()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.StampingPlateDesigns.ToArray();
            }
        }

        public static StampingPlateTheme[] GetPlateThemes()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.StampingPlateThemes.ToArray();
            }
        }

        public Response Archive()
        {
            using (var db = new PolishWarehouseEntities())
            {
                //Add the polish
                var stampingPlate = db.StampingPlates_ARCHIVE.Where(p => p.ID == ID).SingleOrDefault();
                if (stampingPlate == null)
                {
                    stampingPlate = new StampingPlates_ARCHIVE();
                    db.StampingPlates_ARCHIVE.Add(stampingPlate);

                }

                stampingPlate.BrandID = BrandID;
                stampingPlate.CreatedOn = DateTime.UtcNow;
                stampingPlate.Quantity = 1;
                stampingPlate.WasGift = WasGift;
                stampingPlate.ID = ID.Value;

                db.SaveChanges();

                //Add the additional info
                var polishAdditional = db.StampingPlates_AdditionalInfo_ARCHIVE.Where(p => p.StampingPlateID == stampingPlate.ID).SingleOrDefault();
                if (polishAdditional == null)
                {
                    polishAdditional = new StampingPlates_AdditionalInfo_ARCHIVE();
                    db.StampingPlates_AdditionalInfo_ARCHIVE.Add(polishAdditional);
                }
                polishAdditional.StampingPlateID = stampingPlate.ID;
                polishAdditional.Notes = Notes;
                polishAdditional.GiftFromName = GiftFromName;


                db.SaveChanges();

                if (Images != null)
                {
                    foreach (var imageModel in Images)
                    {
                        var image = db.StampingPlates_Images.Where(im => im.ID == imageModel.ID).SingleOrDefault();
                        var i = new StampingPlates_Images_ARCHIVE();
                        i.StampingPlateID = image.StampingPlateID;
                        i.Image = image.Image;
                        i.MIMEType = image.MIMEType;
                        i.PublicImage = image.PublicImage;
                        i.DisplayImage = image.DisplayImage;
                        i.MakerImage = image.MakerImage;
                        i.ID = image.ID;
                        i.Description = image.Description;
                        i.Notes = image.Notes;
                        i.MakerImage = image.MakerImage;
                        i.PublicImage = image.PublicImage;
                        i.DisplayImage = image.DisplayImage;

                        db.StampingPlates_Images_ARCHIVE.Add(i);
                        db.SaveChanges();
                    }
                }

                //If this is a new add, this should be empty.
                //If it is not, we need to purge all of these so that we can refresh the table.
                var oldStampingPlateDesigns = db.StampingPlates_StampingPlateDesigns.Where(p => p.ID == stampingPlate.ID).ToArray();
                db.StampingPlates_StampingPlateDesigns.RemoveRange(oldStampingPlateDesigns);
                db.SaveChanges();

                if (StampingPlateDesigns != null)
                {
                    foreach (var plateDesign in StampingPlateDesigns)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var design = db.StampingPlates_StampingPlateDesigns.Where(gc => gc.DesignID == plateDesign.ID && gc.StampingPlateID == stampingPlate.ID).SingleOrDefault();
                        if (design == null)
                        {
                            design = new StampingPlates_StampingPlateDesigns()
                            {
                                StampingPlateID = stampingPlate.ID,
                                DesignID = plateDesign.ID
                            };
                            db.StampingPlates_StampingPlateDesigns.Add(design);
                            db.SaveChanges();
                        }
                    }
                }

                //If this is a new add, this should be empty.
                //If it is not, we need to purge all of these so that we can refresh the table.
                var oldStampingPlateThemes = db.StampingPlates_StampingPlateThemes.Where(p => p.ID == stampingPlate.ID).ToArray();
                db.StampingPlates_StampingPlateThemes.RemoveRange(oldStampingPlateThemes);
                db.SaveChanges();

                if (StampingPlateThemes != null)
                {
                    foreach (var plateTheme in StampingPlateThemes)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var plate = db.StampingPlates_StampingPlateThemes.Where(gc => gc.ThemeID == plateTheme.ID && gc.StampingPlateID == stampingPlate.ID).SingleOrDefault();
                        if (plate == null)
                        {
                            plate = new StampingPlates_StampingPlateThemes()
                            {
                                StampingPlateID = stampingPlate.ID,
                                ThemeID = plateTheme.ID
                            };
                            db.StampingPlates_StampingPlateThemes.Add(plate);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return new Response(true);
        }
        public bool Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                if (!Quantity.HasValue)
                    throw new Exception("Quantity amount not valid");

                if (Quantity.Value <= 0)
                    throw new Exception("Quantity amount not valid");

                //Add the stamping plate
                var stampingPlate = db.StampingPlates.Where(p => p.ID == ID).SingleOrDefault();
                if (stampingPlate == null)
                {
                    stampingPlate = new StampingPlate();
                    db.StampingPlates.Add(stampingPlate);
                }

                stampingPlate.BrandID = BrandID;
                stampingPlate.CreatedOn = DateTime.UtcNow;
                stampingPlate.Quantity = Quantity.Value;
                stampingPlate.WasGift = WasGift;
                stampingPlate.Name = Name;
                stampingPlate.ShapeID = StampingPlateShapeID;
                

                db.SaveChanges();
                ID = stampingPlate.ID;

                //Add the additional info
                var polishAdditional = db.StampingPlates_AdditionalInfo.Where(p => p.StampingPlateID == stampingPlate.ID).SingleOrDefault();
                if (polishAdditional == null)
                {
                    polishAdditional = new StampingPlates_AdditionalInfo();
                    db.StampingPlates_AdditionalInfo.Add(polishAdditional);
                }
                polishAdditional.StampingPlateID = stampingPlate.ID;
                polishAdditional.Notes = Notes;
                polishAdditional.GiftFromName = GiftFromName;


                db.SaveChanges();

                StampingPlateShape = db.StampingPlateShapes.Where(s=> s.ID == StampingPlateShapeID).SingleOrDefault();
                StampingPlateDesigns = StampingPlateDesignIDs == null ? null : db.StampingPlateDesigns.Where(pt => StampingPlateDesignIDs.Contains(pt.ID)).ToArray();
                StampingPlateThemes = StampingPlateThemeIDs == null ? null : db.StampingPlateThemes.Where(pt => StampingPlateThemeIDs.Contains(pt.ID)).ToArray();


                //If this is a new add, this should be empty.
                //If it is not, we need to purge all of these so that we can refresh the table.
                var oldStampingPlateDesigns = db.StampingPlates_StampingPlateDesigns.Where(p => p.ID == stampingPlate.ID).ToArray();
                db.StampingPlates_StampingPlateDesigns.RemoveRange(oldStampingPlateDesigns);
                db.SaveChanges();

                if (StampingPlateDesigns != null)
                {
                    foreach (var plateDesign in StampingPlateDesigns)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var design = db.StampingPlates_StampingPlateDesigns.Where(gc => gc.DesignID == plateDesign.ID && gc.StampingPlateID == stampingPlate.ID).SingleOrDefault();
                        if (design == null)
                        {
                            design = new StampingPlates_StampingPlateDesigns()
                            {
                                StampingPlateID = stampingPlate.ID,
                                DesignID = plateDesign.ID
                            };
                            db.StampingPlates_StampingPlateDesigns.Add(design);
                            db.SaveChanges();
                        }
                    }
                }

                //If this is a new add, this should be empty.
                //If it is not, we need to purge all of these so that we can refresh the table.
                var oldStampingPlateThemes = db.StampingPlates_StampingPlateThemes.Where(p => p.ID == stampingPlate.ID).ToArray();
                db.StampingPlates_StampingPlateThemes.RemoveRange(oldStampingPlateThemes);
                db.SaveChanges();

                if (StampingPlateThemes != null)
                {
                    foreach (var plateTheme in StampingPlateThemes)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var plate = db.StampingPlates_StampingPlateThemes.Where(gc => gc.ThemeID == plateTheme.ID && gc.StampingPlateID == stampingPlate.ID).SingleOrDefault();
                        if (plate == null)
                        {
                            plate = new StampingPlates_StampingPlateThemes()
                            {
                                StampingPlateID = stampingPlate.ID,
                                ThemeID = plateTheme.ID
                            };
                            db.StampingPlates_StampingPlateThemes.Add(plate);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return true;
        }
        public Response SaveImages(IEnumerable<HttpPostedFileBase> files)
        {
            if (!ID.HasValue)
                return new Response(false, "Polish is not saved, save polish before uploading images");

            using (var db = new PolishWarehouseEntities())
            {
                var images = db.StampingPlates_Images.Where(i => i.StampingPlateID == ID.Value).ToArray();
                var maxWidth = Convert.ToInt32(Utilities.GetConfigurationValue("Image max width"));
                var maxHeight = Convert.ToInt32(Utilities.GetConfigurationValue("Image max height"));

                bool first = images == null ? true : !(images.Any(i => i.DisplayImage.Value));
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var model = new StampingPlateImageModel()
                        {
                            StampingPlateID = ID.Value,
                            MaxHeight = maxHeight,
                            MaxWidth = maxWidth,
                            DisplayImage = first
                        };

                        first = false;
                        model.Save(file);
                    }
                }

                return new Response(true);
            }
        }
        public Response Delete()
        {
            using (var db = new PolishWarehouseEntities())
            {
                //string basePath = HttpContext.Current.Server.MapPath($"/Content/PolishImages/{BrandName.Replace(" ", "").Replace("&", "AND")}/{PolishName.Replace(" ", "")}/");
                //Remove the dependants
                var additional = db.StampingPlates_AdditionalInfo.Where(a => a.StampingPlateID == ID).SingleOrDefault();
                if (additional != null)
                    db.StampingPlates_AdditionalInfo.Remove(additional);

               

                //var types = db.StampingPlates_PolishTypes.Where(a => a.StampingPlateID == ID).ToArray();
                //if (types != null)
                //    db.StampingPlates_PolishTypes.RemoveRange(types);

                var images = db.StampingPlates_Images.Where(a => a.StampingPlateID == ID).ToArray();
                if (images != null)
                {
                    ////Kill current images
                    //if (Directory.Exists(basePath))
                    //{
                    //    DirectoryInfo di = new DirectoryInfo(basePath);
                    //    foreach (FileInfo f in di.GetFiles())
                    //    {
                    //        f.Delete();
                    //    }
                    //    foreach (DirectoryInfo dir in di.GetDirectories())
                    //    {
                    //        dir.Delete(true);
                    //    }
                    //}

                    db.StampingPlates_Images.RemoveRange(images);
                }


                //Remove the polish
                var polish = db.StampingPlates.Where(p => p.ID == ID.Value).SingleOrDefault();

                db.StampingPlates.Remove(polish);
                db.SaveChanges();

                return new Response(true);
                //return new Response(false, "StampingPlates can't be removed yet like this because your husband didn't do it right.");
            }
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

    public class StampingPlateDestashModel : StampingPlateModel
    {
        public int SellQty { get; set; } = 1;
        public string BuyerName { get; set; }
        public decimal? AskingPrice { get; set; }
        public decimal? SoldPrice { get; set; }
        public string TrackingNumber { get; set; }
        public string DestashNotes { get; set; }
        public string InternalDestashNotes { get; set; }
        public string SaleStatus { get; set; }

        public StampingPlateDestashModel() { }
        public StampingPlateDestashModel(long? id, bool colors = true, bool returnimages = false, bool forPublicView = true)
        {
            if (!id.HasValue)
                return;

            using (var db = new PolishWarehouseEntities())
            {
                var useOriginal = false;
                var useDatabase = false;
                try
                {
                    useOriginal = Convert.ToBoolean(Utilities.GetConfigurationValue("Use original quality image"));
                    useDatabase = Convert.ToBoolean(Utilities.GetConfigurationValue("Use database image"));
                }
                catch (Exception ex)
                {
                    Logging.LogEvent(LogTypes.Error, "Error getting image config settings", "Error getting image config settings", ex);
                }

                var stampingPlate = db.StampingPlates.Join(db.StampingPlates_DestashInfo,
                                            p => p.ID,
                                            pdi => pdi.StampingPlateID,
                                            (p, pdi) => new { Polish = p, StampingPlates_DestashInfo = pdi }).Where(po => po.Polish.ID == id).SingleOrDefault();

                ID = stampingPlate.Polish.ID;
                BrandID = stampingPlate.Polish.BrandID;
                BrandName = stampingPlate.Polish.Brand.Name;
                Quantity = stampingPlate.Polish.Quantity;
                WasGift = stampingPlate.Polish.WasGift;
                GiftFromName = stampingPlate.Polish.StampingPlates_AdditionalInfo.GiftFromName;
                Notes = stampingPlate.Polish.StampingPlates_AdditionalInfo.Notes;
                //Types = stampingPlate.Polish.StampingPlates_PolishTypes.Select(ppt => ppt.PolishType).ToArray();

                if (returnimages)
                {
                    Images = db.StampingPlates_Images.Where(i => i.StampingPlateID == id && (forPublicView ? i.PublicImage : true)).Select(i => new StampingPlateImageModel()
                    {
                        ID = i.ID,
                        StampingPlateID = i.StampingPlateID,
                        //Image = i.Image,
                        //MimeType = i.MIMEType,
                        ImageForHTML = (useDatabase ? (useOriginal ? "data:" + i.MIMEType + ";base64," + i.Image : "data:" + i.CompressedMIMEType + ";base64," + i.CompressedImage) : (useOriginal ? i.ImagePath : i.CompressedImagePath)),
                        Description = i.Description,
                        Notes = i.Notes,
                        MakerImage = i.MakerImage.HasValue ? i.MakerImage.Value : false,
                        PublicImage = i.PublicImage,
                        DisplayImage = i.DisplayImage.HasValue ? i.DisplayImage.Value : false
                    }).ToArray();
                }

                SellQty = stampingPlate.StampingPlates_DestashInfo.Qty;
                BuyerName = stampingPlate.StampingPlates_DestashInfo.BuyerName;
                AskingPrice = stampingPlate.StampingPlates_DestashInfo.AskingPrice;
                SoldPrice = stampingPlate.StampingPlates_DestashInfo.SoldPrice;
                TrackingNumber = stampingPlate.StampingPlates_DestashInfo.TrackingNumber;
                DestashNotes = stampingPlate.StampingPlates_DestashInfo.Notes;
                InternalDestashNotes = stampingPlate.StampingPlates_DestashInfo.InternalNotes;
                SaleStatus = stampingPlate.StampingPlates_DestashInfo.SaleStatus;
            }
        }
        public Response DestashPolish()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polish = db.StampingPlates.Where(p => p.ID == ID).SingleOrDefault();
                if (polish == null)
                    return new Response(false, "Polish not found.");

                var destash = db.StampingPlates_DestashInfo.Where(p => p.StampingPlateID == ID).SingleOrDefault();
                if (destash == null)
                {
                    destash = new StampingPlates_DestashInfo();
                    destash.StampingPlateID = ID.Value;
                    db.StampingPlates_DestashInfo.Add(destash);
                }

                destash.Qty = SellQty;
                destash.BuyerName = BuyerName;
                destash.AskingPrice = AskingPrice;
                destash.SoldPrice = SoldPrice;
                destash.TrackingNumber = TrackingNumber;
                destash.Notes = DestashNotes;
                destash.InternalNotes = InternalDestashNotes;
                destash.SaleStatus = SaleStatus;

                db.SaveChanges();

                return new Response(true);
            }
        }
        public Response ArchiveDestash()
        {
            var resp = Archive();
            if (resp.WasSuccessful)
            {
                using (var db = new PolishWarehouseEntities())
                {
                    //_ARCHIVE
                    var polish = db.StampingPlates_ARCHIVE.Where(p => p.ID == ID).SingleOrDefault();
                    if (polish == null)
                        return new Response(false, "Polish not found.");

                    var destash = db.StampingPlates_DestashInfo_ARCHIVE.Where(p => p.StampingPlateID == ID).SingleOrDefault();
                    if (destash == null)
                    {
                        destash = new StampingPlates_DestashInfo_ARCHIVE();
                        destash.StampingPlateID = ID.Value;
                        db.StampingPlates_DestashInfo_ARCHIVE.Add(destash);
                    }

                    destash.Qty = SellQty;
                    destash.BuyerName = BuyerName;
                    destash.AskingPrice = AskingPrice;
                    destash.SoldPrice = SoldPrice;
                    destash.TrackingNumber = TrackingNumber;
                    destash.Notes = DestashNotes;
                    destash.InternalNotes = InternalDestashNotes;
                    destash.SaleStatus = SaleStatus;

                    //Remove record from DB.
                    var d = db.StampingPlates_DestashInfo.Where(a => a.StampingPlateID == ID).SingleOrDefault();
                    if (d != null)
                        db.StampingPlates_DestashInfo.Remove(d);

                    db.SaveChanges();

                    //blow away everything else.
                    var del = Delete();
                    if (del.WasSuccessful)
                        return new Response(true);
                    else
                        return del;
                }
            }
            else
                return resp;

        }
        public static Response MarkAllPendingAsSold()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.StampingPlates_DestashInfo.Where(p => p.SaleStatus == "P").ToArray();

                foreach (var polish in polishes)
                {
                    polish.SaleStatus = "S";
                }

                db.SaveChanges();
                return new Response(true);
            }
        }

        public static Response ArchiveAllSold()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.StampingPlates_DestashInfo.Where(p => p.SaleStatus == "S").Select(p => p.StampingPlateID).ToArray();
                var errors = new List<string>();
                foreach (var polish in polishes)
                {
                    var model = new PolishDestashModel(polish, true, true);
                    var resp = model.ArchiveDestash();
                    if (!resp.WasSuccessful)
                        errors.Add($"Error Archiving {model.PolishName} : {resp.Message}");
                }

                if (errors.Count > 0)
                    return new Response(false, string.Join("\r\n", errors.ToArray()));
                else
                    return new Response(true);
            }
        }
    }

    public class StampingPlateArchiveModel : StampingPlateModel
    {
        public StampingPlateArchiveModel() { }
        public StampingPlateArchiveModel(int? id, bool colors = true, bool returnimages = false, bool forPublicView = true)
        {
            if (!id.HasValue)
                return;

            using (var db = new PolishWarehouseEntities())
            {
                var useOriginal = false;
                var useDatabase = false;
                try
                {
                    useOriginal = Convert.ToBoolean(Utilities.GetConfigurationValue("Use original quality image"));
                    useDatabase = Convert.ToBoolean(Utilities.GetConfigurationValue("Use database image"));
                }
                catch (Exception ex)
                {
                    Logging.LogEvent(LogTypes.Error, "Error getting image config settings", "Error getting image config settings", ex);
                }

                var p = db.StampingPlates_ARCHIVE.Where(po => po.ID == id).SingleOrDefault();
                var brand = db.Brands.Where(z => z.ID == p.BrandID).SingleOrDefault();
                //var color = db.Colors.Where(z => z.ID == p.ColorID).SingleOrDefault();
                var add = db.StampingPlates_AdditionalInfo_ARCHIVE.Where(z => z.StampingPlateID == p.ID).SingleOrDefault();
                //var secColor = db.StampingPlates_Secondary_Colors_ARCHIVE.Join(db.Colors,
                //                            sc => sc.ColorID,
                //                            c => c.ID,
                //                            (sc, c) => new { StampingPlates_Secondary_Colors_ARCHIVE = sc, Color = c }).Where(z => z.StampingPlates_Secondary_Colors_ARCHIVE.StampingPlateID == p.ID).ToArray();
                //var glitColor = db.StampingPlates_Glitter_Colors_ARCHIVE.Join(db.Colors,
                //                            sc => sc.ColorID,
                //                            c => c.ID,
                //                            (sc, c) => new { StampingPlates_Glitter_Colors_ARCHIVE = sc, Color = c }).Where(z => z.StampingPlates_Glitter_Colors_ARCHIVE.StampingPlateID == p.ID).ToArray();
                //var pptypes = db.StampingPlates_PolishTypes_ARCHIVE.Join(db.PolishTypes,
                //                            sc => sc.PolishTypeID,
                //                            c => c.ID,
                //                            (sc, c) => new { StampingPlates_PolishTypes_ARCHIVE = sc, PolishType = c }).Where(z => z.StampingPlates_PolishTypes_ARCHIVE.StampingPlateID == p.ID).ToArray();


                ID = p.ID;
                BrandID = p.BrandID.Value;
                BrandName = brand.Name;
                Quantity = p.Quantity;
                GiftFromName = add.GiftFromName;
                Notes = add.Notes;
                //SecondaryColors = (colors && secColor != null) ? secColor.Select(pec => pec.Color).ToArray() : null;
                //GlitterColors = (colors && glitColor != null) ? glitColor.Select(pec => pec.Color).ToArray() : null;
                //Types = colors ? pptypes.Select(ppt => ppt.PolishType).ToArray() : null;

                if (returnimages)
                {
                    Images = db.StampingPlates_Images_ARCHIVE.Where(i => i.StampingPlateID == id && (forPublicView ? i.PublicImage : true)).Select(i => new StampingPlateImageModel()
                    {
                        ID = i.ID,
                        StampingPlateID = i.StampingPlateID,
                        //Image = i.Image,
                        //MimeType = i.MIMEType,
                        ImageForHTML = (useDatabase ? (useOriginal ? "data:" + i.MIMEType + ";base64," + i.Image : "data:" + i.CompressedMIMEType + ";base64," + i.CompressedImage) : (useOriginal ? i.ImagePath : i.CompressedImagePath)),
                        Description = i.Description,
                        Notes = i.Notes,
                        MakerImage = i.MakerImage.HasValue ? i.MakerImage.Value : false,
                        PublicImage = i.PublicImage,
                        DisplayImage = i.DisplayImage.HasValue ? i.DisplayImage.Value : false
                    }).ToArray();
                }
            }
        }
        public StampingPlateArchiveModel(StampingPlates_ARCHIVE p, bool colors = false, bool returnimages = false, bool forPublicView = true)
        {
            var useOriginal = false;
            var useDatabase = false;
            try
            {
                useOriginal = Convert.ToBoolean(Utilities.GetConfigurationValue("Use original quality image"));
                useDatabase = Convert.ToBoolean(Utilities.GetConfigurationValue("Use database image"));
            }
            catch (Exception ex)
            {
                Logging.LogEvent(LogTypes.Error, "Error getting image config settings", "Error getting image config settings", ex);
            }
            using (var db = new PolishWarehouseEntities())
            {
                //var p = db.StampingPlates_ARCHIVE.Where(po => po.ID == id).SingleOrDefault();
                var brand = db.Brands.Where(z => z.ID == p.BrandID).SingleOrDefault();
                //var color = db.Colors.Where(z => z.ID == p.ColorID).SingleOrDefault();
                var add = db.StampingPlates_AdditionalInfo_ARCHIVE.Where(z => z.StampingPlateID == p.ID).SingleOrDefault();
                //var secColor = db.StampingPlates_Secondary_Colors_ARCHIVE.Join(db.Colors,
                //                            sc => sc.ColorID,
                //                            c => c.ID,
                //                            (sc, c) => new { StampingPlates_Secondary_Colors_ARCHIVE = sc, Color = c }).Where(z => z.StampingPlates_Secondary_Colors_ARCHIVE.StampingPlateID == p.ID).ToArray();
                //var glitColor = db.StampingPlates_Glitter_Colors_ARCHIVE.Join(db.Colors,
                //                            sc => sc.ColorID,
                //                            c => c.ID,
                //                            (sc, c) => new { StampingPlates_Glitter_Colors_ARCHIVE = sc, Color = c }).Where(z => z.StampingPlates_Glitter_Colors_ARCHIVE.StampingPlateID == p.ID).ToArray();
                //var pptypes = db.StampingPlates_PolishTypes_ARCHIVE.Join(db.PolishTypes,
                //                            sc => sc.PolishTypeID,
                //                            c => c.ID,
                //                            (sc, c) => new { StampingPlates_PolishTypes_ARCHIVE = sc, PolishType = c }).Where(z => z.StampingPlates_PolishTypes_ARCHIVE.StampingPlateID == p.ID).ToArray();


                ID = p.ID;
                BrandID = p.BrandID.Value;
                BrandName = brand.Name;
                Quantity = p.Quantity;
                WasGift = p.WasGift.HasValue ? p.WasGift.Value : false;
                GiftFromName = add.GiftFromName;
                Notes = add.Notes;
                //SecondaryColors = (colors && secColor != null) ? secColor.Select(pec => pec.Color).ToArray() : null;
                //GlitterColors = (colors && glitColor != null) ? glitColor.Select(pec => pec.Color).ToArray() : null;
                //Types = colors ? pptypes.Select(ppt => ppt.PolishType).ToArray() : null;

                if (returnimages)
                {
                    Images = db.StampingPlates_Images_ARCHIVE.Where(i => i.StampingPlateID == ID && (forPublicView ? i.PublicImage : true)).Select(i => new StampingPlateImageModel()
                    {
                        ID = i.ID,
                        StampingPlateID = i.StampingPlateID,
                        //Image = i.Image,
                        //MimeType = i.MIMEType,
                        ImageForHTML = (useDatabase ? (useOriginal ? "data:" + i.MIMEType + ";base64," + i.Image : "data:" + i.CompressedMIMEType + ";base64," + i.CompressedImage) : (useOriginal ? i.ImagePath : i.CompressedImagePath)),
                        Description = i.Description,
                        Notes = i.Notes,
                        MakerImage = i.MakerImage.HasValue ? i.MakerImage.Value : false,
                        PublicImage = i.PublicImage,
                        DisplayImage = i.DisplayImage.HasValue ? i.DisplayImage.Value : false
                    }).ToArray();
                }
            }
        }
    }

    public class StampingPlateImageModel
    {
        public long? ID { get; set; }
        public long StampingPlateID { get; set; }
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
                var polish = db.StampingPlates.Where(p => p.ID == StampingPlateID).SingleOrDefault();
                if (polish == null)
                    return new Response(false, "Polish not found");

                MaxWidth = Convert.ToInt32(Utilities.GetConfigurationValue("Image max width"));
                MaxHeight = Convert.ToInt32(Utilities.GetConfigurationValue("Image max height"));

                //string basePath = HttpContext.Current.Server.MapPath($"/Content/PolishImages/{polish.Brand.Name.Replace(" ", "").Replace("&", "AND")}/{polish.Name.Replace(" ", "")}/{ID.ToString()}/");
                string basePath = $"/Content/PolishImages/{polish.Brand.ID.ToString()}/{polish.ID.ToString()}/{ID.ToString()}/";

                var image = db.StampingPlates_Images.Where(i => i.ID == ID).SingleOrDefault();
                var imgCount = 0;

                if (image == null)
                {
                    image = new StampingPlates_Images();
                    image.StampingPlateID = StampingPlateID;
                    image.Image = "";
                    image.MIMEType = file.ContentType;
                    image.PublicImage = PublicImage;
                    image.DisplayImage = DisplayImage;
                    image.MakerImage = MakerImage;
                    db.StampingPlates_Images.Add(image);
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
                    var otherImages = db.StampingPlates_Images.Where(i => i.StampingPlateID == StampingPlateID && i.ID != ID).ToArray();
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
                    var polish = db.StampingPlates.Where(p => p.ID == StampingPlateID).SingleOrDefault();
                    if (polish == null)
                        return new Response(false, "Polish not found");

                    string basePath = $"/Content/PolishImages/{polish.Brand.ID.ToString()}/{polish.ID.ToString()}/{ID.ToString()}/";

                    var image = db.StampingPlates_Images.Where(i => i.ID == ID).SingleOrDefault();
                    var imgCount = 0;

                    if (image == null)
                    {
                        image = new StampingPlates_Images();
                        image.StampingPlateID = StampingPlateID;
                        image.Image = "";
                        image.MIMEType = "image/jpeg";
                        image.PublicImage = PublicImage;
                        image.DisplayImage = DisplayImage;
                        image.MakerImage = MakerImage;
                        db.StampingPlates_Images.Add(image);
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
                        var otherImages = db.StampingPlates_Images.Where(i => i.StampingPlateID == StampingPlateID && i.ID != ID).ToArray();
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
                var polish = db.StampingPlates.Where(p => p.ID == StampingPlateID).SingleOrDefault();
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

                var image = db.StampingPlates_Images.Where(p => p.ID == ID).SingleOrDefault();
                db.StampingPlates_Images.Remove(image);
                db.SaveChanges();
                return new Response(true);
            }
        }
    }
}