using PolishWarehouseData;
using System.Linq;

namespace PolishWarehouse.Models
{
    public class ColorModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsSecondary { get; set; }
        public bool IsGlitter { get; set; }

        public ColorModel() { }
        public ColorModel(int? id)
        {
            if (!id.HasValue)
                return;

            using (var db = new PolishWarehouseEntities())
            {
                var c = db.Colors.Where(po => po.ID == id).SingleOrDefault();

                ID = c.ID;
                Name = c.Name;
                Description = c.Description;
                IsPrimary = c.IsPrimary;
                IsSecondary = c.IsSecondary;
                IsGlitter = c.IsGlitter;
            }
        }

        public ColorModel(Color c)
        {
            ID = c.ID;
            Name = c.Name;
            Description = c.Description;
            IsPrimary = c.IsPrimary;
            IsSecondary = c.IsSecondary;
            IsGlitter = c.IsGlitter;
        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var color = db.Colors.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (color == null)
                {
                    color = new Color();
                    db.Colors.Add(color);
                }

                //color.ID = ID;
                color.Name = Name;
                color.Description = Description;
                color.IsPrimary = IsPrimary;
                color.IsSecondary = IsSecondary;
                color.IsGlitter = IsGlitter;

                db.SaveChanges();
                return new Response(true);
            }
        }

        public Response Delete()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Where(p => p.ColorID == ID.Value).ToArray();
                if (polishes.Count() > 0)
                {
                    return new Response(false, "Delete Failed: There are nail polishes that have this color assigned to it as a primary.");
                }

                var secondaryPolishes = db.Polishes_Secondary_Colors.Where(p => p.ColorID == ID.Value).ToArray();
                if (secondaryPolishes.Count() > 0)
                {
                    return new Response(false, "Delete Failed: There are nail polishes that have this color assigned to it as a secondary color.");
                }

                var glitterPolishes = db.Polishes_Glitter_Colors.Where(p => p.ColorID == ID.Value).ToArray();
                if (glitterPolishes.Count() > 0)
                {
                    return new Response(false, "Delete Failed: There are nail polishes that have this color assigned to it as a glitter color.");
                }

                var color = db.Colors.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (color == null)
                {
                    return new Response(true);
                }

                db.Colors.Remove(color);
                db.SaveChanges();

                return new Response(true);
            }
        }

        public static ColorModel GetModel(Color c)
        {
            return new ColorModel(c);
        }
    }
}