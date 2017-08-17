using PolishWarehouse.Models;
using PolishWarehouseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolishWarehouse.Controllers
{
    public class SettingsController : Controller
    {
        public ActionResult Colors()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Select(p => new PolishModel
                {
                    BrandName = p.Brand.Name,
                    PolishName = p.Name,
                    ColorName = p.Color.Name,
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

        public ActionResult Brands()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Select(p => new PolishModel
                {
                    BrandName = p.Brand.Name,
                    PolishName = p.Name,
                    ColorName = p.Color.Name,
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

        public ActionResult PolishTypes()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Select(p => new PolishModel
                {
                    BrandName = p.Brand.Name,
                    PolishName = p.Name,
                    ColorName = p.Color.Name,
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
    }
}