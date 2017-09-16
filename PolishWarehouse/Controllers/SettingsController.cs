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
        #region Colors
        public ActionResult Colors()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var colors = db.Colors.Select(b => new ColorModel()
                {
                    ID = b.ID,
                    Name = b.Name,
                    Description = b.Description,
                    IsPrimary = b.IsPrimary,
                    IsSecondary = b.IsSecondary,
                    IsGlitter = b.IsGlitter
                }).OrderBy(p => p.Name).ToArray();
                return View(colors);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetColorID(string Name)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    int? id = db.Colors.Where(b => b.Name == Name).Select(b => b.ID).SingleOrDefault();
                    if (id.HasValue)
                        return Json(id);
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
        public JsonResult GetColorDetails(int id)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    var m = new ColorModel(id);
                    return Json(m);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveColor(ColorModel model)
        {
            try
            {
                var resp = model.Save();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Color Saved!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("Colors");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteColor(ColorModel model)
        {
            try
            {
                var resp = model.Delete();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Color Deleted!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("Colors");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddColor(string Name, string description = "")
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    var br = db.Colors.Where(b => b.Name == Name).SingleOrDefault();
                    if (br != null)
                        throw new Exception("Color already exists!");

                    var color = new Color()
                    {
                        Name = Name,
                        Description = description
                    };

                    db.Colors.Add(color);
                    db.SaveChanges();
                    return Json(color.ID);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        #endregion

        #region Brands
        public ActionResult Brands()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var cat = db.BrandCategories.ToArray();

                if (cat != null)
                {
                    ViewBag.BrandCategories = cat;
                }
                var brands = db.Brands.Select(b => new BrandModel()
                {
                    ID = b.ID,
                    Name = b.Name,
                    Description = b.Description,
                    Category = new BrandCategoryModel()
                    {
                        ID = b.BrandCategory.ID,
                        Name = b.BrandCategory.Name,
                        Description = b.BrandCategory.Description
                    }
                }).OrderBy(p => p.Name).ToArray();
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
        public JsonResult GetBrandDetails(int id)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    var bm = new BrandModel(id);
                    return Json(bm);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBrand(BrandModel brand)
        {
            try
            {
                var resp = brand.Save();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Brand Saved!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("Brands");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBrand(BrandModel brand)
        {
            try
            {
                var resp = brand.Delete();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Brand Deleted!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("Brands");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddBrand(string BrandName, int? CategoryID)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    var br = db.Brands.Where(b => b.Name == BrandName).SingleOrDefault();
                    if (br != null)
                        throw new Exception("Brand Name already exists!");

                    var brand = new Brand()
                    {
                        Name = BrandName,
                        CategoryID = CategoryID.HasValue ? CategoryID.Value : 1
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
        #endregion

        #region PolishTypes
        public ActionResult PolishTypes()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var PolishTypes = db.PolishTypes.Select(b => new PolishTypeModel()
                {
                    ID = b.ID,
                    Name = b.Name,
                    Description = b.Description
                }).OrderBy(p => p.Name).ToArray();
                return View(PolishTypes);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetPolishTypeID(string Name)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    int? id = db.PolishTypes.Where(b => b.Name == Name).Select(b => b.ID).SingleOrDefault();
                    if (id.HasValue)
                        return Json(id);
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
        public JsonResult GetPolishTypeDetails(int id)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    var m = new PolishTypeModel(id);
                    return Json(m);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePolishType(PolishTypeModel model)
        {
            try
            {
                var resp = model.Save();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Polish Type Saved!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("PolishTypes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePolishType(PolishTypeModel model)
        {
            try
            {
                var resp = model.Delete();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Polish Type Deleted!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("PolishTypes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPolishType(string Name, string description = "")
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    var br = db.PolishTypes.Where(b => b.Name == Name).SingleOrDefault();
                    if (br != null)
                        throw new Exception("Polish Type already exists!");

                    var type = new PolishType()
                    {
                        Name = Name,
                        Description = description
                    };

                    db.PolishTypes.Add(type);
                    db.SaveChanges();
                    return Json(type.ID);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        #endregion

        #region BrandCategories
        public ActionResult BrandCategories()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var BrandCategories = db.BrandCategories.Select(b => new BrandCategoryModel()
                {
                    ID = b.ID,
                    Name = b.Name,
                    Description = b.Description
                }).OrderBy(p => p.Name).ToArray();
                return View(BrandCategories);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetBrandCategoryID(string Name)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    int? id = db.BrandCategories.Where(b => b.Name == Name).Select(b => b.ID).SingleOrDefault();
                    if (id.HasValue)
                        return Json(id);
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
        public JsonResult GetBrandCategoryDetails(int id)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    var m = new BrandCategoryModel(id);
                    return Json(m);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBrandCategory(BrandCategoryModel model)
        {
            try
            {
                var resp = model.Save();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Category Saved!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("BrandCategories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBrandCategory(BrandCategoryModel model)
        {
            try
            {
                var resp = model.Delete();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Category Deleted!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("BrandCategories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddBrandCategory(string Name, string description = "")
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    var br = db.BrandCategories.Where(b => b.Name == Name).SingleOrDefault();
                    if (br != null)
                        throw new Exception("Category already exists!");

                    var category = new BrandCategory()
                    {
                        Name = Name,
                        Description = description
                    };

                    db.BrandCategories.Add(category);
                    db.SaveChanges();
                    return Json(category.ID);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        #endregion
    }
}