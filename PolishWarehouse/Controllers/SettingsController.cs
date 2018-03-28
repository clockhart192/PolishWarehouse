using PolishWarehouse.Models;
using PolishWarehouseData;
using System;
using System.Linq;
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
                var colors = db.Colors.Where(c=> c.ID != 0).Select(b => new ColorModel()
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
                return Json(Logging.LogEvent(LogTypes.Error, "Error getting color ID", "There was an error fetching your color.", ex));
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
                    return Json(Logging.LogEvent(LogTypes.Error, "Error getting color details", "There was an error getting the details of the color", ex));
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
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving the color", "There was an error saving the color", ex);
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
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error deleting color", "There was an error deleting the color", ex);
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
                return Json(Logging.LogEvent(LogTypes.Error, "Error adding color", "There was an error adding your color", ex));
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
                return Json(Logging.LogEvent(LogTypes.Error, "Error getting brand id", "There was an error getting the brand.", ex));
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
                    return Json(Logging.LogEvent(LogTypes.Error, "Error getting brand details", "There was an error getting the brand details.", ex));
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
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving the brand.", "There was an error saving the brand.", ex);
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
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error deleting brand.", "There was an error deleting the brand.", ex);
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
                return Json(Logging.LogEvent(LogTypes.Error, "Error adding brand.", "There was an error adding your brand", ex));
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
                return Json(Logging.LogEvent(LogTypes.Error, "Error getting polish type id", "There was an error getting the polish type", ex));
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
                    return Json(Logging.LogEvent(LogTypes.Error, "Error getting polish type details", "There was an error getting the polish type details", ex));
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
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving the polish type", "There was an error saving the polish type", ex);
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
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error deleting polish type", "There was an error deleting the polish type.", ex);
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
                return Json(Logging.LogEvent(LogTypes.Error, "Error adding polish type", "There was an error adding your polish type", ex));
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
                return Json(Logging.LogEvent(LogTypes.Error, "Error getting brand category id", "There was an error getting the brand category", ex));
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
                    return Json(Logging.LogEvent(LogTypes.Error, "Error getting brand category details", "There was an error getting the brand category details.", ex));
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
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving brand category", "There was an error saving the brand category.", ex);
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
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error deleting brand category", "There was an error deleting the brand category.", ex);
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
                return Json(Logging.LogEvent(LogTypes.Error, "Error adding brand category", "There was an error adding the brand category.", ex));
            }
        }
        #endregion

        #region StampingPlateShapes
        public ActionResult StampingPlateShapes()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var shapes = db.StampingPlateShapes.Select(b => new StampingPlateShapeModel()
                {
                    ID = b.ID,
                    Name = b.Name,
                    Description = b.Description
                }).OrderBy(p => p.Name).ToArray();
                return View(shapes);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetStampingPlateShapeID(string ShapeName)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    int? ID = db.StampingPlateShapes.Where(b => b.Name == ShapeName).Select(b => b.ID).SingleOrDefault();
                    if (ID.HasValue)
                        return Json(ID);
                    else
                        return Json(0);
                }
            }
            catch (Exception ex)
            {
                return Json(Logging.LogEvent(LogTypes.Error, "Error getting shape id", "There was an error getting the shape.", ex));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetStampingPlateShapeDetails(int id)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    var bm = db.StampingPlateShapes.Where(b => b.ID == id).Select(b => new StampingPlateShapeModel()
                    {
                        ID = b.ID,
                        Name = b.Name,
                        Description = b.Description
                    }).SingleOrDefault();
                    return Json(bm);
                }
                catch (Exception ex)
                {
                    return Json(Logging.LogEvent(LogTypes.Error, "Error getting shape details", "There was an error getting the shape details.", ex));
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveStampingPlateShape(StampingPlateShapeModel shape)
        {
            try
            {
                var resp = shape.Save();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Shape Saved!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving the shape.", "There was an error saving the shape.", ex);
            }

            return RedirectToAction("StampingPlateShapes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStampingPlateShape(StampingPlateShapeModel shape)
        {
            try
            {
                var resp = shape.Delete();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Shape Deleted!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error deleting shape.", "There was an error deleting the shape.", ex);
            }

            return RedirectToAction("StampingPlateShapes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddStampingPlateShape(string Name)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    var br = db.StampingPlateShapes.Where(b => b.Name == Name).SingleOrDefault();
                    if (br != null)
                        throw new Exception("Name already exists!");

                    var brand = new Brand()
                    {
                        Name = Name,
                    };

                    db.Brands.Add(brand);
                    db.SaveChanges();
                    return Json(brand.ID);
                }
            }
            catch (Exception ex)
            {
                return Json(Logging.LogEvent(LogTypes.Error, "Error adding shape.", "There was an error adding your shape", ex));
            }
        }
        #endregion

        #region StampingPlateDesigns
        public ActionResult StampingPlateDesigns()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var designs = db.StampingPlateDesigns.Select(b => new StampingPlateDesignModel()
                {
                    ID = b.ID,
                    Name = b.Name,
                    Description = b.Description
                }).OrderBy(p => p.Name).ToArray();
                return View(designs);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetStampingPlateDesignID(string DesignName)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    int? ID = db.StampingPlateDesigns.Where(b => b.Name == DesignName).Select(b => b.ID).SingleOrDefault();
                    if (ID.HasValue)
                        return Json(ID);
                    else
                        return Json(0);
                }
            }
            catch (Exception ex)
            {
                return Json(Logging.LogEvent(LogTypes.Error, "Error getting design id", "There was an error getting the design.", ex));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetStampingPlateDesignDetails(int id)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    var bm = db.StampingPlateDesigns.Where(b => b.ID == id).Select(b => new StampingPlateDesignModel()
                    {
                        ID = b.ID,
                        Name = b.Name,
                        Description = b.Description
                    }).SingleOrDefault();
                    return Json(bm);
                }
                catch (Exception ex)
                {
                    return Json(Logging.LogEvent(LogTypes.Error, "Error getting design details", "There was an error getting the design details.", ex));
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveStampingPlateDesign(StampingPlateDesignModel design)
        {
            try
            {
                var resp = design.Save();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Design Saved!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving the design.", "There was an error saving the design.", ex);
            }

            return RedirectToAction("StampingPlateDesigns");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStampingPlateDesign(StampingPlateDesignModel design)
        {
            try
            {
                var resp = design.Delete();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Design Deleted!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error deleting design.", "There was an error deleting the design.", ex);
            }

            return RedirectToAction("StampingPlateDesigns");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPlateDesign(string Name)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    var br = db.StampingPlateDesigns.Where(b => b.Name == Name).SingleOrDefault();
                    if (br != null)
                        throw new Exception("Name already exists!");

                    var design = new StampingPlateDesign()
                    {
                        Name = Name,
                    };

                    db.StampingPlateDesigns.Add(design);
                    db.SaveChanges();
                    return Json(design);
                }
            }
            catch (Exception ex)
            {
                return Json(Logging.LogEvent(LogTypes.Error, "Error adding design.", "There was an error adding your design", ex));
            }
        }
        #endregion

        #region StampingPlateThemes
        public ActionResult StampingPlateThemes()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var theme = db.StampingPlateThemes.Select(b => new StampingPlateThemeModel()
                {
                    ID = b.ID,
                    Name = b.Name,
                    Description = b.Description
                }).OrderBy(p => p.Name).ToArray();
                return View(theme);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetStampingPlateThemeID(string ThemeName)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    int? ID = db.StampingPlateThemes.Where(b => b.Name == ThemeName).Select(b => b.ID).SingleOrDefault();
                    if (ID.HasValue)
                        return Json(ID);
                    else
                        return Json(0);
                }
            }
            catch (Exception ex)
            {
                return Json(Logging.LogEvent(LogTypes.Error, "Error getting theme id", "There was an error getting the theme.", ex));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetStampingPlateThemeDetails(int id)
        {
            using (var db = new PolishWarehouseEntities())
            {
                try
                {
                    var bm = db.StampingPlateThemes.Where(b => b.ID == id).Select(b => new StampingPlateThemeModel()
                    {
                        ID = b.ID,
                        Name = b.Name,
                        Description = b.Description
                    }).SingleOrDefault();
                    return Json(bm);
                }
                catch (Exception ex)
                {
                    return Json(Logging.LogEvent(LogTypes.Error, "Error getting theme details", "There was an error getting the theme details.", ex));
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveStampingPlateTheme(StampingPlateThemeModel theme)
        {
            try
            {
                var resp = theme.Save();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Theme Saved!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error saving the theme.", "There was an error saving the theme.", ex);
            }

            return RedirectToAction("StampingPlateThemes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStampingPlateTheme(StampingPlateThemeModel theme)
        {
            try
            {
                var resp = theme.Delete();
                if (resp.WasSuccessful)
                    TempData["Messages"] = "Theme Deleted!";
                else
                    TempData["Errors"] = resp.Message;
            }
            catch (Exception ex)
            {
                TempData["Errors"] = Logging.LogEvent(LogTypes.Error, "Error deleting theme.", "There was an error theme the shape.", ex);
            }

            return RedirectToAction("StampingPlateThemes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPlateTheme(string Name)
        {
            try
            {
                using (var db = new PolishWarehouseEntities())
                {
                    var br = db.StampingPlateThemes.Where(b => b.Name == Name).SingleOrDefault();
                    if (br != null)
                        throw new Exception("Name already exists!");

                    var theme = new StampingPlateTheme()
                    {
                        Name = Name,
                    };

                    db.StampingPlateThemes.Add(theme);
                    db.SaveChanges();
                    return Json(theme);
                }
            }
            catch (Exception ex)
            {
                return Json(Logging.LogEvent(LogTypes.Error, "Error adding theme.", "There was an error adding your theme", ex));
            }
        }
        #endregion
    }
}