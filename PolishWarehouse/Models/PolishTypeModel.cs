using PolishWarehouseData;
using System.Linq;

namespace PolishWarehouse.Models
{
    public class PolishTypeModel
    {
        public long? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public PolishTypeModel() { }
        public PolishTypeModel(int? id)
        {
            if (!id.HasValue)
                return;

            using (var db = new PolishWarehouseEntities())
            {
                var b = db.PolishTypes.Where(po => po.ID == id).SingleOrDefault();

                ID = b.ID;
                Name = b.Name;
                Description = b.Description;
            }
        }

        public PolishTypeModel(PolishTypeModel polishType)
        {
            ID = polishType.ID;
            Name = polishType.Name;
            Description = polishType.Description;
           
        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishType = db.PolishTypes.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (polishType == null)
                {
                    polishType = new PolishType();
                    db.PolishTypes.Add(polishType);
                }

                polishType.Name = Name;
                polishType.Description = Description;

                db.SaveChanges();
                return new Response(true);
            }
        }

        public Response Delete()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes_PolishTypes.Where(p => p.PolishTypeID == ID.Value).ToArray();
                if(polishes.Count() > 0)
                {
                    return new Response(false, "Delete Failed: There are nail polishes that belong to this polish type.");
                }

                var polishType = db.PolishTypes.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (polishType == null)
                {
                    return new Response(true);
                }

                db.PolishTypes.Remove(polishType);
                db.SaveChanges();

                return new Response(true);
            }
        }
    }
}