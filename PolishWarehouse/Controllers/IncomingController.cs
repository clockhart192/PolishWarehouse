using PolishWarehouse.Models;
using PolishWarehouseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolishWarehouse.Controllers
{
    public class IncomingController : Controller
    {
        public ActionResult Index(bool viewComplete = false)
        {
            ViewBag.ViewingComplete = viewComplete;
            if (viewComplete)
                return View(IncomingOrderModel.GetIncomingOrderModelList().Where(o => o.OrderComplete).ToArray());
            else
                return View(IncomingOrderModel.GetIncomingOrderModelList().Where(o=> !o.OrderComplete).ToArray());
        }

        public ActionResult Details(int? id)
        {
            ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = PolishModel.getBrands().OrderBy(c => c.Name);
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);
            ViewBag.LineTypes = IncomingOrderLineTypeModel.GetLineTypes().OrderBy(c => c.Name);
            ViewBag.ShippingProviders = ShippingProviderModel.GetShippingProviders().OrderBy(c => c.ID);
            ViewBag.IsNew = !id.HasValue;

            if (id.HasValue)
                return View(new IncomingOrderModel(id.Value));
            else
                return View(new IncomingOrderModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(IncomingOrderModel order)
        {
            var action = "Details";
            try
            {
                var resp = order.Save();

                if (resp.WasSuccessful)
                    TempData["Messages"] = "Order Saved!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving order info", "There was an error saving your order", ex);
            }
            return RedirectToAction(action, new { id = order.ID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveLine(IncomingOrderLineModel model)
        {
            try
            {
                return Json(model.Save());
            }
            catch (Exception ex)
            {
                return Json(Logging.LogEvent(LogTypes.Error, "Error saving orderline", "Error saving the order line", ex));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SavePolishLine(IncomingOrderLinePolishModel model)
        {
            try
            {
                return Json(model.Save());
            }
            catch (Exception ex)
            {
                return Json(Logging.LogEvent(LogTypes.Error, "Error saving orderline polish", "Error saving the polish to the order.", ex));
            }

        }
    }
}