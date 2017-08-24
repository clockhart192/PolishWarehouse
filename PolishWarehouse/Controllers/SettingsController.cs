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
                var colors = db.Colors.OrderBy(p => p.Name).ToArray();
                return View(colors);
            }
        }

        public ActionResult Brands()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var brands = db.Brands.OrderBy(p => p.Name).ToArray();
                return View(brands);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetBrandID(string BrandName)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    int? brandID = db.Brands.Where(b => b.Name == BrandName).Select(b => b.ID).SingleOrDefault();
                    if (brandID.HasValue)
                        return Json(brandID);
                    else
                        return Json(0);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddBrand(string BrandName)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    var br = db.Brands.Where(b => b.Name == BrandName).SingleOrDefault();
                    if (br != null)
                        throw new Exception("Brand Name already exists!");

                    var brand = new Brand() {
                        Name = BrandName,
                    };

                    db.Brands.Add(brand);
                    db.SaveChanges();
                    return Json(brand.ID);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
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

        public ActionResult BrandCategories()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var BrandCategories = db.BrandCategories.OrderBy(p => p.Name).ToArray();
                return View(BrandCategories);
            }
        }
    }
}