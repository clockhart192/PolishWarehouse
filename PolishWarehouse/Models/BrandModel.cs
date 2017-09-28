using PolishWarehouseData;
using System.Linq;

namespace PolishWarehouse.Models
{
    public class BrandModel
    {
        public long? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CategoryID { get; set; }
        public BrandCategoryModel Category { get; set; }

        public BrandModel() { }
        public BrandModel(int? id)
        {
            if (!id.HasValue)
                return;

            using (var db = new PolishWarehouseEntities())
            {
                var b = db.Brands.Where(po => po.ID == id).SingleOrDefault();

                ID = b.ID;
                Name = b.Name;
                Description = b.Description;
                Category = new BrandCategoryModel(b.BrandCategory);
            }
        }

        public BrandModel(Brand brand)
        {
            ID = brand.ID;
            Name = brand.Name;
            Description = brand.Description;
            CategoryID = brand.CategoryID;
            using (var db = new PolishWarehouseEntities())
            {
                var cat = db.BrandCategories.Where(bc => bc.ID == CategoryID).SingleOrDefault();
                Category = new BrandCategoryModel(cat);
            }
        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                if(Category == null && CategoryID.HasValue)
                    Category = new BrandCategoryModel(CategoryID.Value);

                var brand = db.Brands.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (brand == null)
                {
                    brand = new Brand();
                    db.Brands.Add(brand);
                }

                brand.Name = Name;
                brand.CategoryID = Category.ID.Value;
                brand.Description = Description;

                db.SaveChanges();
                return new Response(true);
            }
        }

        public Response Delete()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Where(p => p.BrandID == ID.Value).ToArray();
                if(polishes.Count() > 0)
                {
                    return new Response(false, "Delete Failed: There are nail polishes that belong to this brand.");
                }

                var brand = db.Brands.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (brand == null)
                {
                    return new Response(true);
                }

                db.Brands.Remove(brand);
                db.SaveChanges();

                return new Response(true);
            }
        }
    }
}