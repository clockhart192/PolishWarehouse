using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PolishWarehouse.Models;
using PolishWarehouseData;

namespace PolishWarehouse.Controllers
{
    public class StampingPlateController : Controller
    {
        #region Main
        public ActionResult Index()
        {
            Server.ScriptTimeout = 600;
            using (var db = new PolishWarehouseEntities())
            {
                var stampingPlates = db.StampingPlates.Where(p => p.StampingPlates_DestashInfo == null).Select(p => new StampingPlateModel
                {
                    ID = p.ID,
                    BrandID = p.BrandID,
                    BrandName = p.Brand.Name,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    WasGift = p.WasGift,
                    GiftFromName = p.StampingPlates_AdditionalInfo.GiftFromName,
                    Notes = p.StampingPlates_AdditionalInfo.Notes,

                }).OrderBy(p => p.BrandName).ThenBy(p => p.BrandName).ToArray();

                string dispConfig = Utilities.GetConfigurationValue("Private Stamping List Display Configuration");
                var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, bool>>(dispConfig);

                ViewBag.DisplayConfiguration = dict;

                return View(stampingPlates);
            }
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
            return Index();
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
            ViewBag.Brands = BrandModel.getBrands(BrandModel.BrandFor.stampingPlate).OrderBy(c => c.Name);
            ViewBag.StampingPlateShapes = StampingPlateModel.GetPlateShapes().OrderBy(sps => sps.Name);
            ViewBag.StampingPlateDesigns = StampingPlateModel.GetPlateDesigns().OrderBy(sps => sps.Name);
            ViewBag.StampingPlateThemes = StampingPlateModel.GetPlateThemes().OrderBy(sps => sps.Name);

            if (id.HasValue)
                return View(new StampingPlateModel(id.Value, returnimages: true));
            else
                return View(new StampingPlateModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(StampingPlateModel plate, IEnumerable<HttpPostedFileBase> files)
        {
            using (var db = new PolishWarehouseEntities())
            {
                var action = "Details";
                try
                {
                    if (plate.ID.HasValue)
                        action = "Index";

                    plate.Save();
                    if (files != null)
                        plate.SaveImages(files);

                    TempData["Messages"] = "Plate Saved!";
                }
                catch (Exception ex)
                {
                    TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving plate info", $"There was an error saving your plate: {ex.Message}", ex);

                    ViewBag.Brands = BrandModel.getBrands(BrandModel.BrandFor.stampingPlate).OrderBy(c => c.Name);
                    ViewBag.StampingPlateShapes = StampingPlateModel.GetPlateShapes().OrderBy(sps => sps.Name);
                    ViewBag.StampingPlateDesigns = StampingPlateModel.GetPlateDesigns().OrderBy(sps => sps.Name);
                    ViewBag.StampingPlateThemes = StampingPlateModel.GetPlateThemes().OrderBy(sps => sps.Name);
                    return View(plate);
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

                var images = db.StampingPlates_Images.Where(p => p.StampingPlateID == id).Select(i => new StampingPlateImageModel
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

                var polish = db.StampingPlates.Where(p => p.ID == id).SingleOrDefault();
                if (polish != null)
                    ViewBag.PolishName = polish.Name;
                var destash = db.StampingPlates_DestashInfo.Where(p => p.StampingPlateID == id).SingleOrDefault();
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
            //ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            //ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            //ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = BrandModel.getBrands(BrandModel.BrandFor.stampingPlate).OrderBy(c => c.Name);
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

                var stampingPlates = db.StampingPlates.Join(db.StampingPlates_DestashInfo,
                    p => p.ID,
                    pdi => pdi.StampingPlateID,
                    (p, pdi) => new { StampingPlate = p, StampingPlates_DestashInfo = pdi }).Select(p => new StampingPlateDestashModel()
                    {
                        ID = p.StampingPlate.ID,
                        BrandID = p.StampingPlate.BrandID,
                        BrandName = p.StampingPlate.Brand.Name,
                        Name = p.StampingPlate.Name,
                        Quantity = p.StampingPlate.Quantity,
                        WasGift = p.StampingPlate.WasGift,
                        GiftFromName = p.StampingPlate.StampingPlates_AdditionalInfo.GiftFromName,
                        Notes = p.StampingPlate.StampingPlates_AdditionalInfo.Notes,

                        SellQty = p.StampingPlates_DestashInfo.Qty,
                        BuyerName = p.StampingPlates_DestashInfo.BuyerName,
                        AskingPrice = p.StampingPlates_DestashInfo.AskingPrice,
                        SoldPrice = p.StampingPlates_DestashInfo.SoldPrice,
                        TrackingNumber = p.StampingPlates_DestashInfo.TrackingNumber,
                        DestashNotes = p.StampingPlates_DestashInfo.Notes,
                        InternalDestashNotes = p.StampingPlates_DestashInfo.InternalNotes,
                        SaleStatus = p.StampingPlates_DestashInfo.SaleStatus

                    }).Where(p => pub ? (p.SaleStatus != "S") : true).OrderBy(p => p.BrandName).ToArray();
                return View(stampingPlates);
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
            var jsonResult = Json(new StampingPlateDestashModel(id, false, true, true), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult DestashDetails(int? id)
        {
            ViewBag.Brands = BrandModel.getBrands(BrandModel.BrandFor.stampingPlate).OrderBy(c => c.Name);
            ViewBag.StampingPlateShapes = StampingPlateModel.GetPlateShapes().OrderBy(sps => sps.Name);
            ViewBag.StampingPlateDesigns = StampingPlateModel.GetPlateDesigns().OrderBy(sps => sps.Name);
            ViewBag.StampingPlateThemes = StampingPlateModel.GetPlateThemes().OrderBy(sps => sps.Name);

            if (id.HasValue)
                return View(new StampingPlateDestashModel(id.Value, returnimages: true));
            else
                return View(new StampingPlateDestashModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DestashDetails(StampingPlateDestashModel polish, IEnumerable<HttpPostedFileBase> files)
        {
            var action = "DestashDetails";
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    if (polish.ID.HasValue)
                        action = "Destash";

                    var dcolor = db.Colors.Where(c => c.Name.ToLower() == "destash").SingleOrDefault();

                    polish.Save();

                    if (files != null)
                        polish.SaveImages(files);

                    polish.DestashPolish();
                    TempData["Messages"] = "Polish Saved!";
                }
                catch (Exception ex)
                {
                    TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving destash info", "There was an error saving your polish", ex);

                    ViewBag.Brands = BrandModel.getBrands(BrandModel.BrandFor.stampingPlate).OrderBy(c => c.Name);
                    ViewBag.StampingPlateShapes = StampingPlateModel.GetPlateShapes().OrderBy(sps => sps.Name);
                    ViewBag.StampingPlateDesigns = StampingPlateModel.GetPlateDesigns().OrderBy(sps => sps.Name);
                    ViewBag.StampingPlateThemes = StampingPlateModel.GetPlateThemes().OrderBy(sps => sps.Name);

                    return View(polish);
                }
                return RedirectToAction(action);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DestashPlate(StampingPlateDestashModel model)
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
        public ActionResult MarkAllPendingAsSold()
        {
            try
            {
                var resp = StampingPlateDestashModel.MarkAllPendingAsSold();
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
                var resp = StampingPlateDestashModel.ArchiveAllSold();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Polish Archived!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error archiving destasah plates", "There was an error archiving your sold destash", ex);
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