using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PolishWarehouse.Models;
using PolishWarehouseData;

namespace PolishWarehouse.Controllers
{
    public class DestashController : Controller
    {
        public ActionResult Index(bool pub = false)
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

        public ActionResult Public()
        {
            ViewBag.ShowPublicDestash = true;
            try
            {
                ViewBag.ShowPublicDestash = Convert.ToBoolean(Utilities.GetConfigurationValue("Show Public Destash"));
            }
            catch { };
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);
            return Index(true);
        }

        public JsonResult DetailsAsync(int id)
        {
            var jsonResult = Json(new PolishDestashModel(id, false, true, true), JsonRequestBehavior.AllowGet);
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
                return View(new PolishDestashModel(id.Value,returnimages: true));
            else
                return View(new PolishDestashModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(PolishDestashModel polish, IEnumerable<HttpPostedFileBase> files)
        {
            var action = "Details";
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    if (polish.ID.HasValue)
                        action = "Index";

                    polish.Save();

                    if (files != null)
                        polish.SaveImages(files);

                    polish.DestashPolish();
                    TempData["Messages"] = "Polish Saved!";
                }
                catch(Exception ex)
                {
                    TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving destash info", "There was an error saving your polish",ex);
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
    }
}