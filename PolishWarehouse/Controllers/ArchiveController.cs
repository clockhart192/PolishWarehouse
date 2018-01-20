using PolishWarehouse.Models;
using PolishWarehouseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolishWarehouse.Controllers
{
    public class ArchiveController : Controller
    {
        // GET: Archive
        public ActionResult Index()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishesFromDB = db.Polishes_ARCHIVE.ToArray();
                var polishes = new List<PolishArchiveModel>();

                foreach(var p in polishesFromDB)
                {
                    polishes.Add(new PolishArchiveModel(p));
                }

                return View(polishes.OrderBy(p=> p.BrandName).ToArray());
            }
        }

        public ActionResult Details(int? id)
        {
            ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = BrandModel.getBrands().OrderBy(c => c.Name);
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);

            if (id.HasValue)
                return View(new PolishArchiveModel(id.Value, returnimages: true));
            else
                return View(new PolishArchiveModel());
        }
    }
}