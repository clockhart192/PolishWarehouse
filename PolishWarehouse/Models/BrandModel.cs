using PolishWarehouseData;
using System.Linq;

namespace PolishWarehouse.Models
{
    public class BrandModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CategoryID { get; set; }
        public BrandCategoryModel Category { get; set; }
        public bool PolishBrand { get; set; } = true;
        public bool PlateBrand { get; set; } = true;

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
                PolishBrand = b.PolishBrand;
                PlateBrand = b.PlateBrand;
                Category = new BrandCategoryModel(b.BrandCategory);
            }
        }

        public BrandModel(Brand brand)
        {
            ID = brand.ID;
            Name = brand.Name;
            Description = brand.Description;
            CategoryID = brand.CategoryID;
            PolishBrand = brand.PolishBrand;
            PlateBrand = brand.PlateBrand;
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
                brand.PolishBrand = PolishBrand;
                brand.PlateBrand = PlateBrand;
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

        public static BrandModel GetModel(Brand brand)
        {
            return new BrandModel(brand);
        }

        public static BrandModel[] GetModels(Brand[] brands)
        {
            return brands.Select(b => GetModel(b)).ToArray();
        }

        public static BrandModel[] getBrands()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.Brands.Select(b => new BrandModel(b)).ToArray();
            }
        }

        public static BrandModel[] getBrands(BrandFor brand)
        {
            using (var db = new PolishWarehouseEntities())
            {
                switch (brand)
                {
                    case BrandFor.polish:
                        return GetModels(db.Brands.Where(b => b.PolishBrand).ToArray());
                    case BrandFor.stampingPlate:
                        return GetModels(db.Brands.Where(b => b.PlateBrand).ToArray());
                    default:
                        return GetModels(db.Brands.ToArray());
                }
            }
        }

        public enum BrandFor
        {
            polish = 0,
            stampingPlate = 1,
        }
    }
}