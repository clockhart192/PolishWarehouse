using PolishWarehouseData;
using System.Linq;

namespace PolishWarehouse.Models
{
    public class StampingPlateThemeModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StampingPlateThemeModel() { }
        public StampingPlateThemeModel(int? id)
        {
            if (!id.HasValue)
                return;

            using (var db = new PolishWarehouseEntities())
            {
                var b = db.StampingPlateThemes.Where(po => po.ID == id).SingleOrDefault();

                ID = b.ID;
                Name = b.Name;
                Description = b.Description;
            }
        }

        public StampingPlateThemeModel(StampingPlateDesign b)
        {
            ID = b.ID;
            Name = b.Name;
            Description = b.Description;
        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var stampingPlateTheme = db.StampingPlateThemes.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (stampingPlateTheme == null)
                {
                    stampingPlateTheme = new StampingPlateTheme();
                    db.StampingPlateThemes.Add(stampingPlateTheme);
                }

                stampingPlateTheme.Name = Name;
                stampingPlateTheme.Description = Description;

                db.SaveChanges();
                return new Response(true);
            }
        }

        public Response Delete()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var plates = db.StampingPlates_StampingPlateThemes.Where(p => p.ThemeID == ID.Value).ToArray();
                if (plates.Count() > 0)
                {
                    return new Response(false, "Delete Failed: There are plates that belong to this theme.");
                }

                var stampingPlateTheme = db.StampingPlateThemes.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (stampingPlateTheme == null)
                {
                    return new Response(true);
                }

                db.StampingPlateThemes.Remove(stampingPlateTheme);
                db.SaveChanges();

                return new Response(true);
            }
        }
    }
}