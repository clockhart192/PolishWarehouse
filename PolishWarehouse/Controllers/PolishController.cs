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
        public ActionResult Index()
        {
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
                return View(polishes);
            }
        }

        public ActionResult Public()
        {
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);
            return Index();
        }

        public JsonResult DetailsAsync(int id)
        {
            var model = new PolishModel(id, false, true, true);
            return Json(model);
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
                    TempData["Errors"] = "Error: " + ex.Message;
                }
                return RedirectToAction(action);
            }
        }

        public ActionResult ManageImages(int id)
        {
            using (var db = new PolishWarehouseEntities())
            {
                var images = db.Polishes_Images.Where(p => p.PolishID == id).Select(p => new PolishImageModel
                {
                    ID = p.ID,
                    PolishID = p.PolishID,
                    Image = p.Image,
                    MimeType = p.MIMEType,
                    ImageForHTML = "data:" + p.MIMEType + ";base64," + p.Image,
                    Description = p.Description,
                    Notes = p.Notes,
                    MakerImage = p.MakerImage.HasValue ? p.MakerImage.Value : false,
                    PublicImage = p.PublicImage,
                    DisplayImage = p.DisplayImage.HasValue ? p.DisplayImage.Value : false
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
                    TempData["Errors"] = "Error: " + ex.Message;
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
                return Json(ex.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetPolishQuickInfo(string colorName)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    var c = db.Colors.Where(b => b.Name == colorName).SingleOrDefault();
                    if (c == null)
                        throw new Exception("Color Doesn't exist!");

                    return Json(new { id = c.ID, number = PolishModel.getNextColorNumber(c.ID) });
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
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
                    TempData["Errors"] = "Error: " + ex.Message;
                    return RedirectToAction("Import");
                }
            }

            TempData["Messages"] = "File Uploaded!";
            return RedirectToAction("Import");
        }
    }
}