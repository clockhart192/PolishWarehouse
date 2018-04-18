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
    public class PolishModel
    {
        public long? ID { get; set; }
        public string PolishName { get; set; }
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public int ColorNumber { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public int? Coats { get; set; }
        public int? Quantity { get; set; }
        public bool HasBeenTried { get; set; } = false;
        public bool WasGift { get; set; } = false;
        public string GiftFromName { get; set; }
        public string Notes { get; set; }
        public Color[] SecondaryColors { get; set; }
        public Color[] GlitterColors { get; set; }
        public PolishType[] Types { get; set; }
        public int[] SecondaryColorsIDs { get; set; }
        public int[] GlitterColorsIDs { get; set; }
        public int[] TypesIDs { get; set; }
        public PolishImageModel[] Images { get; set; }

        public PolishModel() { }
        public PolishModel(int? id, bool colors = true, bool returnimages = false, bool forPublicView = true)
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

                var p = db.Polishes.Where(po => po.ID == id).SingleOrDefault();

                ID = p.ID;
                BrandID = p.BrandID;
                ColorID = p.ColorID;
                BrandName = p.Brand.Name;
                PolishName = p.Name;
                ColorName = p.Color.Name;
                ColorNumber = p.ColorNumber;
                Description = p.Polishes_AdditionalInfo.Description;
                Label = p.Label;
                Coats = p.Coats;
                Quantity = p.Quantity;
                HasBeenTried = p.HasBeenTried;
                WasGift = p.WasGift;
                GiftFromName = p.Polishes_AdditionalInfo.GiftFromName;
                Notes = p.Polishes_AdditionalInfo.Notes;
                SecondaryColors = colors ? p.Polishes_Secondary_Colors.Select(pec => pec.Color).ToArray() : null;
                GlitterColors = colors ? p.Polishes_Glitter_Colors.Select(pec => pec.Color).ToArray() : null;
                Types = colors ? p.Polishes_PolishTypes.Select(ppt => ppt.PolishType).ToArray() : null;

                if (returnimages)
                {
                    Images = db.Polishes_Images.Where(i => i.PolishID == id && (forPublicView ? i.PublicImage : true)).Select(i => new PolishImageModel()
                    {
                        ID = i.ID,
                        PolishID = i.PolishID,
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
        public PolishModel(Polish p, bool colors = false, bool returnimages = false, bool forPublicView = true)
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
                ColorID = p.ColorID;
                BrandName = p.Brand.Name;
                PolishName = p.Name;
                ColorName = p.Color.Name;
                ColorNumber = p.ColorNumber;
                Description = p.Polishes_AdditionalInfo.Description;
                Label = p.Label;
                Coats = p.Coats;
                Quantity = p.Quantity;
                HasBeenTried = p.HasBeenTried;
                WasGift = p.WasGift;
                GiftFromName = p.Polishes_AdditionalInfo.GiftFromName;
                Notes = p.Polishes_AdditionalInfo.Notes;
                SecondaryColors = colors ? p.Polishes_Secondary_Colors.Select(pec => pec.Color).ToArray() : null;
                GlitterColors = colors ? p.Polishes_Glitter_Colors.Select(pec => pec.Color).ToArray() : null;
                Types = colors ? p.Polishes_PolishTypes.Select(ppt => ppt.PolishType).ToArray() : null;

                if (returnimages)
                {
                    Images = db.Polishes_Images.Where(i => i.PolishID == ID && (forPublicView ? i.PublicImage : true)).Select(i => new PolishImageModel()
                    {
                        ID = i.ID,
                        PolishID = i.PolishID,
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

        //TODO: MOVE TO COLORMODEL
        public static Color[] getPrimaryColors()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.Colors.Where(c => c.IsPrimary).ToArray();
            }
        }
        public static Color[] getSecondaryColors()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.Colors.Where(c => c.IsSecondary).ToArray();
            }
        }
        public static Color[] getGlitterColors()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.Colors.Where(c => c.IsGlitter).ToArray();
            }
        }

        public static int getNextColorNumber(int colorID)
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Where(p => p.ColorID == colorID).OrderBy(p => p.ColorNumber).ToArray();

                //This is our first one for this color clearly.
                if (polishes.Length <= 0)
                    return 1;

                //loop through the polishes until you find one where the i and the color number does not align,
                //this should be because there is a gap in the color numbers and we want to fill that gap.
                for (int i = 0; i < polishes.Length; i++)
                {
                    if (polishes[i].ColorNumber != (i + 1))
                    {
                        return i + 1;
                    }
                }

                //If we got this far, we are going to the next number in line.
                return polishes.Length + 1;

                //throw new Exception("A color number could not be generated.");
            }
        }
        public static PolishModel GetRandom(string color, string brand, bool includeTried)
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Where(p => p.Polishes_DestashInfo == null).ToArray();//Get all non-destash polish.

                //filter colors.
                if (!string.IsNullOrWhiteSpace(color))
                {
                    var colorFilter = polishes.Where(p => p.Color.Name == color).ToArray();
                    polishes = colorFilter;
                }

                //filter brands.
                if (!string.IsNullOrWhiteSpace(brand))
                {
                    var brandFilter = polishes.Where(p => p.Brand.Name == brand).ToArray();
                    polishes = brandFilter;
                }

                //include tried check
                if (!includeTried)
                {
                    var triedFilter = polishes.Where(p => p.HasBeenTried == false).ToArray();
                    polishes = triedFilter;
                }

                Random rnd = new Random();
                int count = rnd.Next(polishes.Count());

                var final = (Polish)polishes.GetValue(count);

                return new PolishModel(final);

            }

        }

        //TODO:MOVE TO POLISHTYPEMODEL
        public static PolishType[] getPolishTypes()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.PolishTypes.ToArray();
            }
        }

        public Response ArchivePolish()
        {
            using (var db = new PolishWarehouseEntities())
            {
                //Add the polish
                var polish = db.Polishes_ARCHIVE.Where(p => p.ID == ID).SingleOrDefault();
                if (polish == null)
                {
                    polish = new Polishes_ARCHIVE();
                    db.Polishes_ARCHIVE.Add(polish);

                }

                var colornum = 0;
                try
                {
                    colornum = ColorNumber;
                }
                catch (Exception ex)
                {
                    throw new Exception(Logging.LogEvent(LogTypes.Error, string.Format("Swatch #{0} did not register as a number.", ColorNumber), string.Format("Swatch #{0} did not register as a number.", ColorNumber), ex));
                }

                ColorName = db.Colors.Where(c => c.ID == ColorID).Select(c => c.Name).SingleOrDefault();
                Label = string.Format("{0} {1}", ColorName, colornum.ToString());
                Types = TypesIDs == null ? null : db.PolishTypes.Where(pt => TypesIDs.Contains(pt.ID)).ToArray();
                SecondaryColors = SecondaryColorsIDs == null ? null : db.Colors.Where(c => SecondaryColorsIDs.Contains(c.ID)).ToArray();
                GlitterColors = GlitterColorsIDs == null ? null : db.Colors.Where(c => GlitterColorsIDs.Contains(c.ID)).ToArray();


                polish.ColorID = ColorID;
                polish.BrandID = BrandID;
                polish.CreatedOn = DateTime.UtcNow;
                polish.Name = PolishName;
                polish.ColorNumber = colornum;
                polish.Quantity = 1;
                polish.Coats = Coats.HasValue ? Coats.Value : 1;
                polish.Label = Label;
                polish.HasBeenTried = HasBeenTried;
                polish.WasGift = WasGift;
                polish.ID = ID.Value;



                db.SaveChanges();

                //Add the additional info
                var polishAdditional = db.Polishes_AdditionalInfo_ARCHIVE.Where(p => p.PolishID == polish.ID).SingleOrDefault();
                if (polishAdditional == null)
                {
                    polishAdditional = new Polishes_AdditionalInfo_ARCHIVE();
                    db.Polishes_AdditionalInfo_ARCHIVE.Add(polishAdditional);
                }
                polishAdditional.PolishID = polish.ID;
                polishAdditional.Description = Description;
                polishAdditional.Notes = Notes;
                polishAdditional.GiftFromName = GiftFromName;


                db.SaveChanges();

                if (Images != null)
                {
                    foreach (var imageModel in Images)
                    {
                        var image = db.Polishes_Images.Where(im => im.ID == imageModel.ID).SingleOrDefault();
                        var i = new Polishes_Images_ARCHIVE();
                        i.PolishID = image.PolishID;
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

                        db.Polishes_Images_ARCHIVE.Add(i);
                        db.SaveChanges();
                    }
                }

                //Polish Types
                //If this is a new add, this should be empty.
                //If it is not, we need to purge all of these so that we can refresh the table.
                //var oldPtypes = db.Polishes_PolishTypes_ARCHIVE.Where(p => p.PolishID == polish.ID).ToArray();
                //db.Polishes_PolishTypes_ARCHIVE.RemoveRange(oldPtypes);
                //db.SaveChanges();

                if (Types != null)
                {
                    foreach (var ptype in Types)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var polishType = db.Polishes_PolishTypes_ARCHIVE.Where(p => p.PolishID == polish.ID && p.PolishTypeID == ptype.ID).SingleOrDefault();
                        if (polishType == null)//Add this type/polish combo if it did not already exist.
                        {
                            polishType = new Polishes_PolishTypes_ARCHIVE()
                            {
                                PolishID = polish.ID,
                                PolishTypeID = ptype.ID
                            };
                            db.Polishes_PolishTypes_ARCHIVE.Add(polishType);
                            db.SaveChanges();
                        }
                    }
                }


                if (SecondaryColors != null)
                {
                    //Secondary Colors
                    //If this is a new add, this should be empty.
                    //If it is not, we need to purge all of these so that we can refresh the table.
                    //var oldSColors = db.Polishes_Secondary_Colors.Where(p => p.PolishID == polish.ID).ToArray();
                    //db.Polishes_Secondary_Colors.RemoveRange(oldSColors);
                    //db.SaveChanges();

                    foreach (var color in SecondaryColors)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var secondaryColor = db.Polishes_Secondary_Colors_ARCHIVE.Where(sc => sc.ColorID == color.ID && sc.PolishID == polish.ID).SingleOrDefault();
                        if (secondaryColor == null)
                        {
                            secondaryColor = new Polishes_Secondary_Colors_ARCHIVE()
                            {
                                PolishID = polish.ID,
                                ColorID = color.ID
                            };
                            db.Polishes_Secondary_Colors_ARCHIVE.Add(secondaryColor);
                            db.SaveChanges();
                        }
                    }
                }

                //Glitter Colors
                //If this is a new add, this should be empty.
                //If it is not, we need to purge all of these so that we can refresh the table.
                //var oldGColors = db.Polishes_Glitter_Colors.Where(p => p.PolishID == polish.ID).ToArray();
                //db.Polishes_Glitter_Colors.RemoveRange(oldGColors);
                //db.SaveChanges();

                if (GlitterColors != null)
                {
                    foreach (var color in GlitterColors)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var glitterColor = db.Polishes_Glitter_Colors_ARCHIVE.Where(gc => gc.ColorID == color.ID && gc.PolishID == polish.ID).SingleOrDefault();
                        if (glitterColor == null)
                        {
                            glitterColor = new Polishes_Glitter_Colors_ARCHIVE()
                            {
                                PolishID = polish.ID,
                                ColorID = color.ID
                            };
                            db.Polishes_Glitter_Colors_ARCHIVE.Add(glitterColor);
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

                if (ColorID <= 0)
                    throw new Exception("Primary color not valid");

                if (!Coats.HasValue)
                    throw new Exception("Coats amount not valid");

                if (Coats.Value <= 0)
                    throw new Exception("Coats amount not valid");

                if (!Quantity.HasValue)
                    throw new Exception("Quantity amount not valid");

                if (Quantity.Value <= 0)
                    throw new Exception("Quantity amount not valid");

                //Add the polish
                var polish = db.Polishes.Where(p => p.ID == ID).SingleOrDefault();
                if (polish == null)
                {
                    polish = new Polish();
                    db.Polishes.Add(polish);
                }

                var colornum = 0;
                try
                {
                    colornum = ColorNumber;
                }
                catch (Exception ex)
                {
                    throw new Exception(Logging.LogEvent(LogTypes.Error, string.Format("Swatch #{0} did not register as a number.", ColorNumber), string.Format("Swatch #{0} did not register as a number.", ColorNumber), ex));
                }

                ColorName = db.Colors.Where(c => c.ID == ColorID).Select(c => c.Name).SingleOrDefault();

                Label = string.Format("{0} {1}", ColorName, colornum.ToString());
                if (db.Polishes.Any(p => p.Label == Label && !Label.ToLower().Contains("Destash") && ID == null))
                    throw new Exception("Label already exists");

                Types = TypesIDs == null ? null : db.PolishTypes.Where(pt => TypesIDs.Contains(pt.ID)).ToArray();
                SecondaryColors = SecondaryColorsIDs == null ? null : db.Colors.Where(c => SecondaryColorsIDs.Contains(c.ID)).ToArray();
                GlitterColors = GlitterColorsIDs == null ? null : db.Colors.Where(c => GlitterColorsIDs.Contains(c.ID)).ToArray();


                polish.ColorID = ColorID;
                polish.BrandID = BrandID;
                polish.CreatedOn = DateTime.UtcNow;
                polish.Name = PolishName;
                polish.ColorNumber = colornum;
                polish.Quantity = Quantity.Value;
                polish.Coats = Coats.HasValue ? Coats.Value : 1;
                polish.Label = Label;
                polish.HasBeenTried = HasBeenTried;
                polish.WasGift = WasGift;

                db.SaveChanges();
                ID = polish.ID;

                //Add the additional info
                var polishAdditional = db.Polishes_AdditionalInfo.Where(p => p.PolishID == polish.ID).SingleOrDefault();
                if (polishAdditional == null)
                {
                    polishAdditional = new Polishes_AdditionalInfo();
                    db.Polishes_AdditionalInfo.Add(polishAdditional);
                }
                polishAdditional.PolishID = polish.ID;
                polishAdditional.Description = Description;
                polishAdditional.Notes = Notes;
                polishAdditional.GiftFromName = GiftFromName;


                db.SaveChanges();

                //Polish Types
                //If this is a new add, this should be empty.
                //If it is not, we need to purge all of these so that we can refresh the table.
                var oldPtypes = db.Polishes_PolishTypes.Where(p => p.PolishID == polish.ID).ToArray();
                db.Polishes_PolishTypes.RemoveRange(oldPtypes);
                db.SaveChanges();

                if (Types != null)
                {
                    foreach (var ptype in Types)
                    {
                        //Given the above delete, this should always be null, sanity check though.
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


                if (SecondaryColors != null)
                {
                    //Secondary Colors
                    //If this is a new add, this should be empty.
                    //If it is not, we need to purge all of these so that we can refresh the table.
                    var oldSColors = db.Polishes_Secondary_Colors.Where(p => p.PolishID == polish.ID).ToArray();
                    db.Polishes_Secondary_Colors.RemoveRange(oldSColors);
                    db.SaveChanges();

                    foreach (var color in SecondaryColors)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var secondaryColor = db.Polishes_Secondary_Colors.Where(sc => sc.ColorID == color.ID && sc.PolishID == polish.ID).SingleOrDefault();
                        if (secondaryColor == null)
                        {
                            secondaryColor = new Polishes_Secondary_Colors()
                            {
                                PolishID = polish.ID,
                                ColorID = color.ID
                            };
                            db.Polishes_Secondary_Colors.Add(secondaryColor);
                            db.SaveChanges();
                        }
                    }
                }

                //Glitter Colors
                //If this is a new add, this should be empty.
                //If it is not, we need to purge all of these so that we can refresh the table.
                var oldGColors = db.Polishes_Glitter_Colors.Where(p => p.PolishID == polish.ID).ToArray();
                db.Polishes_Glitter_Colors.RemoveRange(oldGColors);
                db.SaveChanges();

                if (GlitterColors != null)
                {
                    foreach (var color in GlitterColors)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var glitterColor = db.Polishes_Glitter_Colors.Where(gc => gc.ColorID == color.ID && gc.PolishID == polish.ID).SingleOrDefault();
                        if (glitterColor == null)
                        {
                            glitterColor = new Polishes_Glitter_Colors()
                            {
                                PolishID = polish.ID,
                                ColorID = color.ID
                            };
                            db.Polishes_Glitter_Colors.Add(glitterColor);
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
                var images = db.Polishes_Images.Where(i => i.PolishID == ID.Value).ToArray();
                var maxWidth = Convert.ToInt32(Utilities.GetConfigurationValue("Image max width"));
                var maxHeight = Convert.ToInt32(Utilities.GetConfigurationValue("Image max height"));

                bool first = images == null ? true : !(images.Any(i => i.DisplayImage.Value));
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var model = new PolishImageModel()
                        {
                            PolishID = ID.Value,
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
                var additional = db.Polishes_AdditionalInfo.Where(a => a.PolishID == ID).SingleOrDefault();
                if (additional != null)
                    db.Polishes_AdditionalInfo.Remove(additional);

                var secondary = db.Polishes_Secondary_Colors.Where(a => a.PolishID == ID).ToArray();
                if (secondary != null)
                    db.Polishes_Secondary_Colors.RemoveRange(secondary);

                var glitter = db.Polishes_Glitter_Colors.Where(a => a.PolishID == ID).ToArray();
                if (glitter != null)
                    db.Polishes_Glitter_Colors.RemoveRange(glitter);

                var types = db.Polishes_PolishTypes.Where(a => a.PolishID == ID).ToArray();
                if (types != null)
                    db.Polishes_PolishTypes.RemoveRange(types);

                var images = db.Polishes_Images.Where(a => a.PolishID == ID).ToArray();
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

                    db.Polishes_Images.RemoveRange(images);
                }


                //Remove the polish
                var polish = db.Polishes.Where(p => p.ID == ID.Value).SingleOrDefault();

                db.Polishes.Remove(polish);
                db.SaveChanges();

                return new Response(true);
                //return new Response(false, "Polishes can't be removed yet like this because your husband didn't do it right.");
            }
        }


    }

    public class PolishDestashModel : PolishModel
    {
        public int SellQty { get; set; } = 1;
        public string BuyerName { get; set; }
        public decimal? AskingPrice { get; set; }
        public decimal? SoldPrice { get; set; }
        public string TrackingNumber { get; set; }
        public string DestashNotes { get; set; }
        public string InternalDestashNotes { get; set; }
        public string SaleStatus { get; set; }

        public PolishDestashModel() { }
        public PolishDestashModel(long? id, bool colors = true, bool returnimages = false, bool forPublicView = true)
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

                var polish = db.Polishes.Join(db.Polishes_DestashInfo,
                                            p => p.ID,
                                            pdi => pdi.PolishID,
                                            (p, pdi) => new { Polish = p, Polishes_DestashInfo = pdi }).Where(po => po.Polish.ID == id).SingleOrDefault();

                ID = polish.Polish.ID;
                BrandID = polish.Polish.BrandID;
                ColorID = polish.Polish.ColorID;
                BrandName = polish.Polish.Brand.Name;
                PolishName = polish.Polish.Name;
                ColorName = polish.Polish.Color.Name;
                ColorNumber = polish.Polish.ColorNumber;
                Description = polish.Polish.Polishes_AdditionalInfo.Description;
                Label = polish.Polish.Label;
                Coats = polish.Polish.Coats;
                Quantity = polish.Polish.Quantity;
                HasBeenTried = polish.Polish.HasBeenTried;
                WasGift = polish.Polish.WasGift;
                GiftFromName = polish.Polish.Polishes_AdditionalInfo.GiftFromName;
                Notes = polish.Polish.Polishes_AdditionalInfo.Notes;
                SecondaryColors = polish.Polish.Polishes_Secondary_Colors.Select(pec => pec.Color).ToArray();
                GlitterColors = polish.Polish.Polishes_Glitter_Colors.Select(pec => pec.Color).ToArray();
                Types = polish.Polish.Polishes_PolishTypes.Select(ppt => ppt.PolishType).ToArray();

                if (returnimages)
                {
                    Images = db.Polishes_Images.Where(i => i.PolishID == id && (forPublicView ? i.PublicImage : true)).Select(i => new PolishImageModel()
                    {
                        ID = i.ID,
                        PolishID = i.PolishID,
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

                SellQty = polish.Polishes_DestashInfo.Qty;
                BuyerName = polish.Polishes_DestashInfo.BuyerName;
                AskingPrice = polish.Polishes_DestashInfo.AskingPrice;
                SoldPrice = polish.Polishes_DestashInfo.SoldPrice;
                TrackingNumber = polish.Polishes_DestashInfo.TrackingNumber;
                DestashNotes = polish.Polishes_DestashInfo.Notes;
                InternalDestashNotes = polish.Polishes_DestashInfo.InternalNotes;
                SaleStatus = polish.Polishes_DestashInfo.SaleStatus;
            }
        }
        public Response DestashPolish()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polish = db.Polishes.Where(p => p.ID == ID).SingleOrDefault();
                if (polish == null)
                    return new Response(false, "Polish not found.");

                var destash = db.Polishes_DestashInfo.Where(p => p.PolishID == ID).SingleOrDefault();
                if (destash == null)
                {
                    destash = new Polishes_DestashInfo();
                    destash.PolishID = ID.Value;
                    db.Polishes_DestashInfo.Add(destash);
                }

                destash.Qty = SellQty;
                destash.BuyerName = BuyerName;
                destash.AskingPrice = AskingPrice;
                destash.SoldPrice = SoldPrice;
                destash.TrackingNumber = TrackingNumber;
                destash.Notes = DestashNotes;
                destash.InternalNotes = InternalDestashNotes;
                destash.SaleStatus = SaleStatus;

                var color = db.Colors.Where(c => c.Name.ToLower() == "destash").SingleOrDefault();
                if (color != null)
                {
                    polish.ColorID = color.ID;
                    polish.ColorNumber = 0;
                }

                db.SaveChanges();

                return new Response(true);
            }
        }
        public Response ArchiveDestash()
        {
            var resp = ArchivePolish();
            if (resp.WasSuccessful)
            {
                using (var db = new PolishWarehouseEntities())
                {
                    //_ARCHIVE
                    var polish = db.Polishes_ARCHIVE.Where(p => p.ID == ID).SingleOrDefault();
                    if (polish == null)
                        return new Response(false, "Polish not found.");

                    var destash = db.Polishes_DestashInfo_ARCHIVE.Where(p => p.PolishID == ID).SingleOrDefault();
                    if (destash == null)
                    {
                        destash = new Polishes_DestashInfo_ARCHIVE();
                        destash.PolishID = ID.Value;
                        db.Polishes_DestashInfo_ARCHIVE.Add(destash);
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
                    var d = db.Polishes_DestashInfo.Where(a => a.PolishID == ID).SingleOrDefault();
                    if (d != null)
                        db.Polishes_DestashInfo.Remove(d);

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
                var polishes = db.Polishes_DestashInfo.Where(p => p.SaleStatus == "P").ToArray();

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
                var polishes = db.Polishes_DestashInfo.Where(p => p.SaleStatus == "S").Select(p => p.PolishID).ToArray();
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

        public static Response UpdateTotalQty(int id,int qty)
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polish = db.Polishes.Where(p => p.ID == id).SingleOrDefault();
                polish.Quantity = qty;
                db.SaveChanges();
                return new Response();
            }
        }
    }

    public class PolishArchiveModel : PolishModel
    {
        public PolishArchiveModel() { }
        public PolishArchiveModel(int? id, bool colors = true, bool returnimages = false, bool forPublicView = true)
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

                var p = db.Polishes_ARCHIVE.Where(po => po.ID == id).SingleOrDefault();
                var brand = db.Brands.Where(z => z.ID == p.BrandID).SingleOrDefault();
                var color = db.Colors.Where(z => z.ID == p.ColorID).SingleOrDefault();
                var add = db.Polishes_AdditionalInfo_ARCHIVE.Where(z => z.PolishID == p.ID).SingleOrDefault();
                var secColor = db.Polishes_Secondary_Colors_ARCHIVE.Join(db.Colors,
                                            sc => sc.ColorID,
                                            c => c.ID,
                                            (sc, c) => new { Polishes_Secondary_Colors_ARCHIVE = sc, Color = c }).Where(z => z.Polishes_Secondary_Colors_ARCHIVE.PolishID == p.ID).ToArray();
                var glitColor = db.Polishes_Glitter_Colors_ARCHIVE.Join(db.Colors,
                                            sc => sc.ColorID,
                                            c => c.ID,
                                            (sc, c) => new { Polishes_Glitter_Colors_ARCHIVE = sc, Color = c }).Where(z => z.Polishes_Glitter_Colors_ARCHIVE.PolishID == p.ID).ToArray();
                var pptypes = db.Polishes_PolishTypes_ARCHIVE.Join(db.PolishTypes,
                                            sc => sc.PolishTypeID,
                                            c => c.ID,
                                            (sc, c) => new { Polishes_PolishTypes_ARCHIVE = sc, PolishType = c }).Where(z => z.Polishes_PolishTypes_ARCHIVE.PolishID == p.ID).ToArray();


                ID = p.ID;
                BrandID = p.BrandID;
                ColorID = p.ColorID;
                BrandName = brand.Name;
                PolishName = p.Name;
                ColorName = color.Name;
                ColorNumber = p.ColorNumber;
                Description = add.Description;
                Label = p.Label;
                Coats = p.Coats;
                Quantity = p.Quantity;
                HasBeenTried = p.HasBeenTried;
                WasGift = p.WasGift;
                GiftFromName = add.GiftFromName;
                Notes = add.Notes;
                SecondaryColors = (colors && secColor != null) ? secColor.Select(pec => pec.Color).ToArray() : null;
                GlitterColors = (colors && glitColor != null) ? glitColor.Select(pec => pec.Color).ToArray() : null;
                Types = colors ? pptypes.Select(ppt => ppt.PolishType).ToArray() : null;

                if (returnimages)
                {
                    Images = db.Polishes_Images_ARCHIVE.Where(i => i.PolishID == id && (forPublicView ? i.PublicImage : true)).Select(i => new PolishImageModel()
                    {
                        ID = i.ID,
                        PolishID = i.PolishID,
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
        public PolishArchiveModel(Polishes_ARCHIVE p, bool colors = false, bool returnimages = false, bool forPublicView = true)
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
                //var p = db.Polishes_ARCHIVE.Where(po => po.ID == id).SingleOrDefault();
                var brand = db.Brands.Where(z => z.ID == p.BrandID).SingleOrDefault();
                var color = db.Colors.Where(z => z.ID == p.ColorID).SingleOrDefault();
                var add = db.Polishes_AdditionalInfo_ARCHIVE.Where(z => z.PolishID == p.ID).SingleOrDefault();
                var secColor = db.Polishes_Secondary_Colors_ARCHIVE.Join(db.Colors,
                                            sc => sc.ColorID,
                                            c => c.ID,
                                            (sc, c) => new { Polishes_Secondary_Colors_ARCHIVE = sc, Color = c }).Where(z => z.Polishes_Secondary_Colors_ARCHIVE.PolishID == p.ID).ToArray();
                var glitColor = db.Polishes_Glitter_Colors_ARCHIVE.Join(db.Colors,
                                            sc => sc.ColorID,
                                            c => c.ID,
                                            (sc, c) => new { Polishes_Glitter_Colors_ARCHIVE = sc, Color = c }).Where(z => z.Polishes_Glitter_Colors_ARCHIVE.PolishID == p.ID).ToArray();
                var pptypes = db.Polishes_PolishTypes_ARCHIVE.Join(db.PolishTypes,
                                            sc => sc.PolishTypeID,
                                            c => c.ID,
                                            (sc, c) => new { Polishes_PolishTypes_ARCHIVE = sc, PolishType = c }).Where(z => z.Polishes_PolishTypes_ARCHIVE.PolishID == p.ID).ToArray();


                ID = p.ID;
                BrandID = p.BrandID;
                ColorID = p.ColorID;
                BrandName = brand.Name;
                PolishName = p.Name;
                ColorName = color.Name;
                ColorNumber = p.ColorNumber;
                Description = add.Description;
                Label = p.Label;
                Coats = p.Coats;
                Quantity = p.Quantity;
                HasBeenTried = p.HasBeenTried;
                WasGift = p.WasGift;
                GiftFromName = add.GiftFromName;
                Notes = add.Notes;
                SecondaryColors = (colors && secColor != null) ? secColor.Select(pec => pec.Color).ToArray() : null;
                GlitterColors = (colors && glitColor != null) ? glitColor.Select(pec => pec.Color).ToArray() : null;
                Types = colors ? pptypes.Select(ppt => ppt.PolishType).ToArray() : null;

                if (returnimages)
                {
                    Images = db.Polishes_Images_ARCHIVE.Where(i => i.PolishID == ID && (forPublicView ? i.PublicImage : true)).Select(i => new PolishImageModel()
                    {
                        ID = i.ID,
                        PolishID = i.PolishID,
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

    public class PolishImageModel
    {
        public long? ID { get; set; }
        public long PolishID { get; set; }
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
                var polish = db.Polishes.Where(p => p.ID == PolishID).SingleOrDefault();
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
                    image.PolishID = PolishID;
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
                    var otherImages = db.Polishes_Images.Where(i => i.PolishID == PolishID && i.ID != ID).ToArray();
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
                    var polish = db.Polishes.Where(p => p.ID == PolishID).SingleOrDefault();
                    if (polish == null)
                        return new Response(false, "Polish not found");

                    string basePath = $"/Content/PolishImages/{polish.Brand.ID.ToString()}/{polish.ID.ToString()}/{ID.ToString()}/";

                    var image = db.Polishes_Images.Where(i => i.ID == ID).SingleOrDefault();
                    var imgCount = 0;

                    if (image == null)
                    {
                        image = new Polishes_Images();
                        image.PolishID = PolishID;
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
                        var otherImages = db.Polishes_Images.Where(i => i.PolishID == PolishID && i.ID != ID).ToArray();
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
                var polish = db.Polishes.Where(p => p.ID == PolishID).SingleOrDefault();
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