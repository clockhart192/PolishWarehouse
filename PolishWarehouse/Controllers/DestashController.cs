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
        public ActionResult Index()
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
            return Json(new PolishDestashModel(id));
        }
        public ActionResult Details(int? id)
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
        public ActionResult Details(PolishDestashModel polish)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    polish.Save();
                    polish.DestashPolish();
                    TempData["Messages"] = "Polish Saved!";
                }
                catch (Exception ex)
                {
                    TempData["Errors"] = "Error: " + ex.Message;
                }
                return RedirectToAction("DestashDetails");
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
    }
}