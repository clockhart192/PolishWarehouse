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
                var polishes = db.Polishes.Where(p=> p.Polishes_DestashInfo == null).Select(p => new PolishModel
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

        public ActionResult PublicList()
        {
            return Index();
        }

        public ActionResult Destash()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Join(db.Polishes_DestashInfo,
                    p=> p.ID,
                    pdi=> pdi.PolishID,
                    (p,pdi) => new {Polish = p, Polishes_DestashInfo = pdi }).Select(p => new PolishDestashModel()
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

                    }).OrderBy(p => p.BrandName).ToArray();
                return View(polishes);
            }
        }

        public ActionResult PublicDestash()
        {
            return Destash();
        }
        public ActionResult DestashDetails(int? id)
        {
            ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = PolishModel.getBrands().OrderBy(c => c.Name);
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);

            if (id.HasValue)
                return View(new PolishDestashModel(id.Value));
            else
                return View(new PolishDestashModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DestashDetails(PolishDestashModel polish)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    polish.Save();
                    polish.DestashPolish();
                    TempData["Messages"] = "Polish Saved!";
                }
                catch(Exception ex)
                {
                    TempData["Errors"] = "Error: " + ex.Message;
                }
                return RedirectToAction("DestashDetails");
            }
        }

        public ActionResult Details(int? id)
        {
            ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = PolishModel.getBrands().OrderBy(c => c.Name);
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);

            if (id.HasValue)
                return View(new PolishModel(id.Value));
            else
                return View(new PolishModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(PolishModel polish)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    polish.Save();
                    TempData["Messages"] = "Polish Saved!";
                }
                catch (Exception ex)
                {
                    TempData["Errors"] = "Error: " + ex.Message;
                }
                return RedirectToAction("Details");
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
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetNextColorNumber(int colorID)
        {
            try
            {
                return Json(PolishModel.getNextColorNumber(colorID));
            }
            catch(Exception ex)
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