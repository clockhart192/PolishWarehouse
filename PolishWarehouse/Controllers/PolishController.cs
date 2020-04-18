using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PolishWarehouse.Models;
using PolishWarehouseData;

namespace PolishWarehouse.Controllers
{
    public class PolishController : Controller
    {

        #region Main
        public ActionResult Index(PolishFilterModel model)
        {
            if (model == null)
            {
                model = new PolishFilterModel();
            }

            //Check for session Object
            if (Session["AdvancedSearch"] != null && (model.isEmpty))

                model = (PolishFilterModel)Session["AdvancedSearch"];
            else
                Session["AdvancedSearch"] = model;

            ViewBag.AdvancedSearch = model;
            ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = BrandModel.getBrands().OrderBy(c => c.Name);
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);

            Server.ScriptTimeout = 600;
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Where(p => p.Polishes_DestashInfo == null || (p.Polishes_DestashInfo != null && p.Polishes_DestashInfo.Qty != p.Quantity))
                    .Select(p => new
                    {
                        ID = p.ID,
                        BrandID = p.BrandID,
                        ColorID = p.ColorID,
                        BrandName = p.Brand.Name,
                        PolishName = p.Name,
                        ColorName = p.Color.Name,
                        ColorNumber = p.ColorNumber,
                        Description = p.Polishes_AdditionalInfo.Description,
                        Label = p.Label,
                        Coats = p.Coats,
                        Quantity = p.Quantity,
                        HasBeenTried = p.HasBeenTried,
                        WasGift = p.WasGift,
                        GiftFromName = p.Polishes_AdditionalInfo.GiftFromName,
                        Notes = p.Polishes_AdditionalInfo.Notes,
                        Location = p.Location,

                        Types = p.Polishes_PolishTypes.Select(pec => new PolishTypeModel()
                        {
                            ID = pec.PolishType.ID,
                            Name = pec.PolishType.Name,
                            Description = pec.PolishType.Description
                        }),

                        SecondaryColors = p.Polishes_Secondary_Colors.Select(c => new ColorModel()
                        {
                            ID = c.Color.ID,
                            Name = c.Color.Name,
                            Description = c.Color.Description
                        }),

                        GlitterColors = p.Polishes_Glitter_Colors.Select(c => new ColorModel()
                        {
                            ID = c.Color.ID,
                            Name = c.Color.Name,
                            Description = c.Color.Description
                        }),

                    }).ToList()
                .Select(p => new PolishModel
                {
                    ID = p.ID,
                    BrandID = p.BrandID,
                    ColorID = p.ColorID,
                    BrandName = p.BrandName,
                    PolishName = p.PolishName,
                    ColorName = p.ColorName,
                    ColorNumber = p.ColorNumber,
                    Description = p.Description,
                    Label = p.Label,
                    Coats = p.Coats,
                    Quantity = p.Quantity,
                    HasBeenTried = p.HasBeenTried,
                    WasGift = p.WasGift,
                    GiftFromName = p.GiftFromName,
                    Notes = p.Notes,
                    Location = p.Location,
                    TypeModels = p.Types.ToArray(),
                    SecondaryColorModels = p.SecondaryColors.ToArray(),
                    GlitterColorModels = p.GlitterColors.ToArray()
                }).ToArray();

                if (model != null)
                {
                    try
                    {
                        ///////////////ADVANED SEARCH SHENANIGANS////////////////////////////
                        if (!string.IsNullOrWhiteSpace(model.PolishName))
                        {
                            polishes = polishes.Where(polish => polish.PolishName.ToUpperInvariant().Contains(model.PolishName.ToUpperInvariant())).ToArray();
                        }
                        if (!string.IsNullOrWhiteSpace(model.Description))
                        {
                            polishes = polishes.Where(polish => polish.Description != null && polish.Description.ToUpperInvariant().Contains(model.Description.ToUpperInvariant())).ToArray();
                        }
                        if (model.BrandNames != null && model.BrandNames.Length > 0)
                        {
                            polishes = polishes.Where(polish => model.BrandNames.ToUpperInvariant().Contains(polish.BrandName.ToUpperInvariant())).ToArray();
                        }
                        if (model.ColorNames != null && model.ColorNames.Length > 0)
                        {
                            polishes = polishes.Where(polish => model.ColorNames.ToUpperInvariant().Contains(polish.ColorName.ToUpperInvariant())).ToArray();
                        }
                        if (!string.IsNullOrWhiteSpace(model.Types))
                        {
                            var type = new PolishTypeModel(PolishModel.getPolishTypes().SingleOrDefault(p => p.Name.ToUpperInvariant().Contains(model.Types.ToUpperInvariant())).ID);
                            polishes = polishes.Where(polish => polish.TypeModels != null && polish.TypeModels.Any(t=> t.Name == type.Name)).ToArray();
                        }
                        if (!string.IsNullOrWhiteSpace(model.SecondaryColors))
                        {
                            var color = new ColorModel(PolishModel.getSecondaryColors().SingleOrDefault(p => p.Name.ToUpperInvariant().Contains(model.SecondaryColors.ToUpperInvariant())).ID);
                            polishes = polishes.Where(polish => polish.SecondaryColorModels != null && polish.SecondaryColorModels.Any(t => t.Name == color.Name)).ToArray();
                        }
                        if (!string.IsNullOrWhiteSpace(model.GlitterColors))
                        {
                            var color = new ColorModel(PolishModel.getGlitterColors().SingleOrDefault(p => p.Name.ToUpperInvariant().Contains(model.GlitterColors.ToUpperInvariant())).ID);
                            polishes = polishes.Where(polish => polish.GlitterColorModels != null && polish.GlitterColorModels.Any(t => t.Name == color.Name)).ToArray();
                        }
                        if (!string.IsNullOrWhiteSpace(model.Location))
                        {
                            polishes = polishes.Where(polish => polish.Location != null && polish.Location.ToUpperInvariant().Contains(model.Location.ToUpperInvariant())).ToArray();
                        }
                        if (model.ColorNumbers.HasValue)
                        {
                            polishes = polishes.Where(polish => polish.ColorNumber == model.ColorNumbers.Value).ToArray();
                        }
                        //if (model.Types != null && model.Types.Length > 0)
                        //{
                        //    polishes = polishes.Where(polish => model.Types.Intersect(polish.Types).Any()).ToArray();
                        //}
                        //if (model.SecondaryColors != null && model.SecondaryColors.Length > 0)
                        //{
                        //    polishes = polishes.Where(polish => model.SecondaryColors.Intersect(polish.SecondaryColors).Any()).ToArray();
                        //}
                        //if (model.GlitterColors != null && model.GlitterColors.Length > 0)
                        //{
                        //    polishes = polishes.Where(polish => model.GlitterColors.Intersect(polish.GlitterColors).Any()).ToArray();
                        //}
                        //if (model.ColorNumbers != null && model.ColorNumbers.Length > 0)
                        //{
                        //    polishes = polishes.Where(polish => model.ColorNumbers.Contains(polish.ColorNumber)).ToArray();
                        //}
                        if (model.FilterByHasBeenTried)
                        {
                            polishes = polishes.Where(polish => polish.HasBeenTried == model.HasBeenTried).ToArray();
                        }
                        if (model.FilterByWasGift)
                        {
                            polishes = polishes.Where(polish => polish.WasGift == model.WasGift).ToArray();
                        }
                        if (model.Coats.HasValue)
                        {
                            polishes = polishes.Where(polish => polish.Coats == model.Coats.Value).ToArray();
                        }

                        polishes = polishes.OrderBy(p => p.BrandName).ThenBy(p => p.PolishName).ToArray();
                        ///////////////END ADVANED SEARCH SHENANIGANS////////////////////////////
                    }
                    catch (Exception ex)
                    {
                        Session["AdvancedSearch"] = null;
                        ViewBag.AdvancedSearch = null;
                        TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error searching for polish", $"There was an error searching for polish: {ex.Message}", ex);
                    }


                    //if (cheatCode == "upupdowndownleftrightleftrightbastart")
                    //{
                    //    if (Convert.ToBoolean(Utilities.GetConfigurationValue("Konami Code Active")))
                    //    {
                    //        Utilities.ReSaveAllImages();
                    //        TempData["Messages"] = "Konami Code Activated!";
                    //    }
                    //}
                }


                string dispConfig = Utilities.GetConfigurationValue("Private List Display Configuration");
                var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, bool>>(dispConfig);
                ViewBag.DisplayConfiguration = dict;

                return View(polishes);
            }
        }

        public ActionResult ClearSearch()
        {
            Session["AdvancedSearch"] = null;
            return RedirectToAction("Index", new PolishFilterModel());
        }

        public ActionResult IndexStampingPlates(string cheatCode)
        {
            Server.ScriptTimeout = 600;
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Where(p => p.Polishes_DestashInfo == null).Select(p => new PolishModel
                {
                    ID = p.ID,
                    BrandID = p.BrandID,
                    ColorID = p.ColorID,
                    BrandName = p.Brand.Name,
                    PolishName = p.Name,
                    ColorName = p.Color.Name,
                    ColorNumber = p.ColorNumber,
                    Description = p.Polishes_AdditionalInfo.Description,
                    Label = p.Label,
                    Coats = p.Coats,
                    Quantity = p.Quantity,
                    HasBeenTried = p.HasBeenTried,
                    WasGift = p.WasGift,
                    GiftFromName = p.Polishes_AdditionalInfo.GiftFromName,
                    Notes = p.Polishes_AdditionalInfo.Notes,

                }).OrderBy(p => p.BrandName).ThenBy(p => p.PolishName).ToArray();



                //if (cheatCode == "upupdowndownleftrightleftrightbastart")
                //{
                //    if (Convert.ToBoolean(Utilities.GetConfigurationValue("Konami Code Active")))
                //    {
                //        Utilities.ReSaveAllImages();
                //        TempData["Messages"] = "Konami Code Activated!";
                //    }
                //}

                string dispConfig = Utilities.GetConfigurationValue("Private List Display Configuration");
                var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, bool>>(dispConfig);

                //var displayConfig = new List<KeyValuePair<string, bool>>();
                //foreach(var pair in dict)
                //{
                //    displayConfig.Add(new KeyValuePair<string, bool>(pair.Key,pair.Value));
                //}
                ViewBag.DisplayConfiguration = dict;

                return View(polishes);
            }
        }

        public ActionResult DetailsStampingPlates(int? id)
        {
            ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = BrandModel.getBrands().OrderBy(c => c.Name);
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);

            if (id.HasValue)
                return View(new PolishModel(id.Value, returnimages: true));
            else
                return View(new PolishModel());
        }

        public ActionResult Public()
        {
            ViewBag.ShowPublicList = true;
            try
            {
                ViewBag.ShowPublicList = Convert.ToBoolean(Utilities.GetConfigurationValue("Show Public List"));
            }
            catch { };
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);
            return Index(null);
        }

        public JsonResult DetailsAsync(int id)
        {
            var model = new PolishModel(id, false, true, true);
            var jsonResult = Json(model, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult Details(int? id)
        {
            ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = BrandModel.getBrands().OrderBy(c => c.Name);
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);

            if (id.HasValue)
                return View(new PolishModel(id.Value, returnimages: true));
            else
                return View(new PolishModel());
        }

        public ActionResult PrintDetails(int? id)
        {
            ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = BrandModel.getBrands().OrderBy(c => c.Name);
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);

            if (id.HasValue)
                return View(new PolishModel(id.Value, returnimages: true));
            else
                return View(new PolishModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(PolishModel polish, IEnumerable<HttpPostedFileBase> files)
        {
            using (var db = new PolishWarehouseEntities())
            {
                var action = "Details";
                try
                {
                    if (polish.ID.HasValue)
                        action = "Index";

                    polish.Save();
                    if (files != null)
                        polish.SaveImages(files);

                    TempData["Messages"] = "Polish Saved!";
                }
                catch (Exception ex)
                {
                    TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving polish info", $"There was an error saving your polish: {ex.Message}", ex);

                    ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
                    ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
                    ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
                    ViewBag.Brands = BrandModel.getBrands().OrderBy(c => c.Name);
                    ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);
                    return View(polish);
                }
                return RedirectToAction(action);
            }
        }

        public ActionResult ManageImages(int id)
        {
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

                var images = db.Polishes_Images.Where(p => p.PolishID == id).Select(i => new PolishImageModel
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
                    //ID = p.ID,
                    //PolishID = p.PolishID,
                    //Image = p.Image,
                    //MimeType = p.MIMEType,
                    //ImageForHTML = "data:" + p.MIMEType + ";base64," + p.Image,
                    //Description = p.Description,
                    //Notes = p.Notes,
                    //MakerImage = p.MakerImage.HasValue ? p.MakerImage.Value : false,
                    //PublicImage = p.PublicImage,
                    //DisplayImage = p.DisplayImage.HasValue ? p.DisplayImage.Value : false
                }).ToArray();

                var polish = db.Polishes.Where(p => p.ID == id).SingleOrDefault();
                if (polish != null)
                    ViewBag.PolishName = polish.Name;
                var destash = db.Polishes_DestashInfo.Where(p => p.PolishID == id).SingleOrDefault();
                if (destash != null)
                    ViewBag.RedirectController = "Destash";
                else
                    ViewBag.RedirectController = "Polish";

                return View(images);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImageDetails(PolishImageModel model, HttpPostedFileBase file)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    var resp = model.Save(file);
                    if (resp.WasSuccessful)
                        TempData["Messages"] = "Image Saved!";
                    else
                        TempData["Errors"] = resp.Message;
                }
                catch (Exception ex)
                {
                    TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving images", "There was an error saving your images", ex);
                }
                return RedirectToAction("ManageImages", new { id = model.PolishID });

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetNextColorNumber(int colorID)
        {
            try
            {
                return Json(PolishModel.getNextColorNumber(colorID));
            }
            catch (Exception ex)
            {
                return Json(Logging.LogEvent(LogTypes.Error, "Error getting next color number", "The server failed to get a color number", ex));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetPolishQuickInfo(string colorName, bool onlyPrimary = true)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    var c = db.Colors.Where(b => b.Name == colorName && (onlyPrimary ? b.IsPrimary : true)).SingleOrDefault();
                    if (c == null)
                        return Json(new { id = 0, number = 0 });

                    return Json(new { id = c.ID, number = PolishModel.getNextColorNumber(c.ID) });
                }

            }
            catch (Exception ex)
            {
                return Json(Logging.LogEvent(LogTypes.Error, $"Error getting polish quick info for {colorName}", "The server failed to return polish info", ex));
            }

        }

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                try
                {
                    //Do stuff with the file
                    Utilities.processCSV(file);
                }
                catch (Exception ex)
                {
                    TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error with import", "There was an error importing your polish", ex);
                    return RedirectToAction("Import");
                }
            }

            TempData["Messages"] = "File Uploaded!";
            return RedirectToAction("Import");
        }

        public ActionResult Random()
        {
            ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            //ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            //ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = BrandModel.getBrands().OrderBy(c => c.Name);
            //ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Random(string color, string brand, bool includeTried = false)
        {
            var p = PolishModel.GetRandom(color, brand, includeTried);
            return RedirectToAction("Details", new { id = p.ID });
        }

        #endregion  

        #region Destash

        public ActionResult Destash(bool pub = false)
        {
            using (var db = new PolishWarehouseEntities())
            {

                var polishes = db.Polishes.Join(db.Polishes_DestashInfo,
                    p => p.ID,
                    pdi => pdi.PolishID,
                    (p, pdi) => new { Polish = p, Polishes_DestashInfo = pdi }).Select(p => new PolishDestashModel()
                    {
                        ID = p.Polish.ID,
                        BrandID = p.Polish.BrandID,
                        ColorID = p.Polish.ColorID,
                        BrandName = p.Polish.Brand.Name,
                        PolishName = p.Polish.Name,
                        ColorName = p.Polish.Color.Name,
                        ColorNumber = p.Polish.ColorNumber,
                        Description = p.Polish.Polishes_AdditionalInfo.Description,
                        Label = p.Polish.Label,
                        Coats = p.Polish.Coats,
                        Quantity = p.Polish.Quantity,
                        HasBeenTried = p.Polish.HasBeenTried,
                        WasGift = p.Polish.WasGift,
                        GiftFromName = p.Polish.Polishes_AdditionalInfo.GiftFromName,
                        Notes = p.Polish.Polishes_AdditionalInfo.Notes,

                        SellQty = p.Polishes_DestashInfo.Qty,
                        BuyerName = p.Polishes_DestashInfo.BuyerName,
                        AskingPrice = p.Polishes_DestashInfo.AskingPrice,
                        SoldPrice = p.Polishes_DestashInfo.SoldPrice,
                        TrackingNumber = p.Polishes_DestashInfo.TrackingNumber,
                        DestashNotes = p.Polishes_DestashInfo.Notes,
                        InternalDestashNotes = p.Polishes_DestashInfo.InternalNotes,
                        SaleStatus = p.Polishes_DestashInfo.SaleStatus

                    }).Where(p => pub ? (p.SaleStatus != "S") : true).OrderBy(p => p.BrandName).ToArray();
                return View(polishes);
            }
        }

        public ActionResult DestashPublic()
        {
            ViewBag.ShowPublicDestash = true;
            try
            {
                ViewBag.ShowPublicDestash = Convert.ToBoolean(Utilities.GetConfigurationValue("Show Public Destash"));
            }
            catch { };
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);
            return Destash(true);
        }

        public JsonResult DestashDetailsAsync(int id)
        {
            var jsonResult = Json(new PolishDestashModel(id, false, true, true), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult DestashDetails(int? id)
        {
            ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = BrandModel.getBrands().OrderBy(c => c.Name);
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);

            if (id.HasValue)
                return View(new PolishDestashModel(id.Value, returnimages: true));
            else
                return View(new PolishDestashModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DestashDetails(PolishDestashModel polish, IEnumerable<HttpPostedFileBase> files)
        {
            var action = "DestashDetails";
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    if (polish.ID.HasValue)
                        action = "Destash";

                    var dcolor = db.Colors.Where(c => c.Name.ToLower() == "destash").SingleOrDefault();

                    if (polish.ColorID == 0)
                    {
                        polish.ColorID = dcolor.ID;
                        polish.ColorName = dcolor.Name;
                    }

                    if (!polish.Coats.HasValue)
                        polish.Coats = 1;

                    polish.Save();

                    if (files != null)
                        polish.SaveImages(files);

                    polish.DestashPolish();
                    TempData["Messages"] = "Polish Saved!";
                }
                catch (Exception ex)
                {
                    TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving destash info", "There was an error saving your polish", ex);

                    ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
                    ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
                    ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
                    ViewBag.Brands = BrandModel.getBrands().OrderBy(c => c.Name);
                    ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);

                    return View(polish);
                }
                return RedirectToAction(action);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DestashPolish(PolishDestashModel model)
        {
            try
            {
                var resp = model.DestashPolish();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Polish destashed!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error destash polish", "There was an error saving your polish's destash", ex);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BulkDestashPolish(string ID, Decimal AskingPrice, int SellQty, string BuyerName, string SaleStatus)
        {
            try
            {
                var ids = ID.Split(',');
                bool wasSuccessful = true;
                foreach (var id in ids)
                {
                    using (var db = new PolishWarehouseEntities())
                    {
                        var dbID = Convert.ToInt32(id);
                        var polish = db.Polishes.Where(p => p.ID == dbID).Select(p => new PolishDestashModel()
                        {
                            ID = p.ID,
                            AskingPrice = AskingPrice,
                            SoldPrice = AskingPrice,
                            SellQty = SellQty,
                            BuyerName = BuyerName,
                            SaleStatus = SaleStatus
                        }).SingleOrDefault();
                        wasSuccessful = polish.DestashPolish().WasSuccessful;
                    }
                }
                if (wasSuccessful)
                    TempData["Messages"] = "Polish destashed!";
                else
                    TempData["Errors"] = "This is an error message that isn't going to tell you enough data about your mass destash and you need to yell at the site creator to log better errors.";
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error bulk destash polish", "There was an error bulk saving your polish's destash", ex);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkAllPendingAsSold()
        {
            try
            {
                var resp = PolishDestashModel.MarkAllPendingAsSold();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "All pending marked as sold!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error pending marked as sold", "There was an error marking pending as sold", ex);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveAllSold()
        {
            try
            {
                var resp = PolishDestashModel.ArchiveAllSold();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Polish Archived!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error archiving destasah polish", "There was an error archiving your sold destash", ex);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateTotalQty(int id, int qty)
        {
            try
            {
                var resp = PolishDestashModel.UpdateTotalQty(id, qty);
                return Json(resp);
            }
            catch (Exception ex)
            {
                return Json(new Response(false, Logging.LogEvent(LogTypes.Error, "Error polish updating total quantity", "Update failed", ex)));
            }

        }
        #endregion
    }
}