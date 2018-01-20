using PolishWarehouseData;
using System.Linq;

namespace PolishWarehouse.Models
{
    public class StampingPlateDesignModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StampingPlateDesignModel() { }
        public StampingPlateDesignModel(int? id)
        {
            if (!id.HasValue)
                return;

            using (var db = new PolishWarehouseEntities())
            {
                var b = db.StampingPlateDesigns.Where(po => po.ID == id).SingleOrDefault();

                ID = b.ID;
                Name = b.Name;
                Description = b.Description;
            }
        }

        public StampingPlateDesignModel(StampingPlateDesign b) {
            ID = b.ID;
            Name = b.Name;
            Description = b.Description;
        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var stampingPlateDesign = db.StampingPlateDesigns.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (stampingPlateDesign == null)
                {
                    stampingPlateDesign = new StampingPlateDesign();
                    db.StampingPlateDesigns.Add(stampingPlateDesign);
                }

                stampingPlateDesign.Name = Name;
                stampingPlateDesign.Description = Description;

                db.SaveChanges();
                return new Response(true);
            }
        }

        public Response Delete()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var plates = db.StampingPlates_StampingPlateDesigns.Where(p => p.DesignID == ID.Value).ToArray();
                if (plates.Count() > 0)
                {
                    return new Response(false, "Delete Failed: There are plates that belong to this design.");
                }

                var stampingPlateDesign = db.StampingPlateDesigns.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (stampingPlateDesign == null)
                {
                    return new Response(true);
                }

                db.StampingPlateDesigns.Remove(stampingPlateDesign);
                db.SaveChanges();

                return new Response(true);
            }
        }
    }
}