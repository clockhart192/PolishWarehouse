using PolishWarehouseData;
using System.Linq;

namespace PolishWarehouse.Models
{
    public class BrandCategoryModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BrandCategoryModel() { }
        public BrandCategoryModel(int? id)
        {
            if (!id.HasValue)
                return;

            using (var db = new PolishWarehouseEntities())
            {
                var b = db.BrandCategories.Where(po => po.ID == id).SingleOrDefault();

                ID = b.ID;
                Name = b.Name;
                Description = b.Description;
            }
        }

        public BrandCategoryModel(BrandCategory b) {
            ID = b.ID;
            Name = b.Name;
            Description = b.Description;
        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var brandCategory = db.BrandCategories.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (brandCategory == null)
                {
                    brandCategory = new BrandCategory();
                    db.BrandCategories.Add(brandCategory);
                }

                brandCategory.Name = Name;
                brandCategory.Description = Description;

                db.SaveChanges();
                return new Response(true);
            }
        }

        public Response Delete()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var brands = db.Brands.Where(p => p.CategoryID == ID.Value).ToArray();
                if (brands.Count() > 0)
                {
                    return new Response(false, "Delete Failed: There are brands that belong to this category.");
                }

                var brandCategory = db.BrandCategories.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (brandCategory == null)
                {
                    return new Response(true);
                }

                db.BrandCategories.Remove(brandCategory);
                db.SaveChanges();

                return new Response(true);
            }
        }
    }
}