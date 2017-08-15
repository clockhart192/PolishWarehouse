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
        // GET: Polish
        public ActionResult Index()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Select(p=> new PolishModel {
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