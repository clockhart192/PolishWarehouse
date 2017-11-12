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
            ViewBag.PrimaryColors = PolishModel.getPrimaryColors().OrderBy(c => c.Name);
            ViewBag.SecondaryColors = PolishModel.getSecondaryColors().OrderBy(c => c.Name);
            ViewBag.GlitterColors = PolishModel.getGlitterColors().OrderBy(c => c.Name);
            ViewBag.Brands = PolishModel.getBrands().OrderBy(c => c.Name);
            ViewBag.PolishTypes = PolishModel.getPolishTypes().OrderBy(c => c.Name);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ConvertToPolish(long id, int dupeAction = 0 )
        {
            try
            {
                IncomingOrderLinePolishModel.DupeAction action = (IncomingOrderLinePolishModel.DupeAction)dupeAction;
                
                using (var db = new PolishWarehouseEntities())
                {
                    var incomingPolish = db.IncomingOrderLines_Polishes.Where(p => p.ID == id).SingleOrDefault();
                    if (incomingPolish == null)
                        throw new Exception("Record not found.");

                    var model = new IncomingOrderLinePolishModel(incomingPolish);
                    var resp = model.ConvertToPolish(action);
                    return Json(resp);
                }
               
            }
            catch (Exception ex)
            {
                return Json(new Response(false,Logging.LogEvent(LogTypes.Error, "Error converting orderline to polish", "Error converting orderline to polish", ex)));
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult FinalizeConvert(long PolishLineID, PolishModel polish, IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    polish.Save();
                    if (files != null)
                        polish.SaveImages(files);

                    var incomingPolish = db.IncomingOrderLines_Polishes.Where(p => p.ID == PolishLineID).SingleOrDefault();
                    if (incomingPolish == null)
                        throw new Exception("Record not found.");

                    incomingPolish.Converted = true;
                    db.SaveChanges();

                    return Json(new Response());
                }

            }
            catch (Exception ex)
            {
                return Json(new Response(false, Logging.LogEvent(LogTypes.Error, "Error converting orderline to polish", ex.Message, ex)));
            }

        }
    }
}