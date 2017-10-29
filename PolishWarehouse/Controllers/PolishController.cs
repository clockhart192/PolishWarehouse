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

        public ActionResult Index(string cheatCode)
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

                }).OrderBy(p => p.BrandName).ToArray();



                if (cheatCode == "upupdowndownleftrightleftrightbastart")
                {
                    if (Convert.ToBoolean(Utilities.GetConfigurationValue("Konami Code Active")))
                    {
                        Utilities.ReSaveAllImages();
                        TempData["Messages"] = "Konami Code Activated!";
                    }
                }

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
            ViewBag.Brands = PolishModel.getBrands().OrderBy(c => c.Name);
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
                    TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving polish info", "There was an error saving your polish", ex);
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
        public JsonResult GetPolishQuickInfo(string colorName,bool onlyPrimary = true)
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
                    PolishModel.processCSV(file);
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
            ViewBag.Brands = PolishModel.getBrands().OrderBy(c => c.Name);
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
    }
}