using PolishWarehouseData;
using System.Linq;

namespace PolishWarehouse.Models
{
    public class StampingPlateShapeModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StampingPlateShapeModel() { }
        public StampingPlateShapeModel(int? id)
        {
            if (!id.HasValue)
                return;

            using (var db = new PolishWarehouseEntities())
            {
                var b = db.StampingPlateShapes.Where(po => po.ID == id).SingleOrDefault();

                ID = b.ID;
                Name = b.Name;
                Description = b.Description;
            }
        }

        public StampingPlateShapeModel(StampingPlateShape b) {
            ID = b.ID;
            Name = b.Name;
            Description = b.Description;
        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var shape = db.StampingPlateShapes.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (shape == null)
                {
                    shape = new StampingPlateShape();
                    db.StampingPlateShapes.Add(shape);
                }

                shape.Name = Name;
                shape.Description = Description;

                db.SaveChanges();
                return new Response(true);
            }
        }

        public Response Delete()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var plates = db.StampingPlates.Where(p => p.ShapeID == ID.Value).ToArray();
                if (plates.Count() > 0)
                {
                    return new Response(false, "Delete Failed: There are plates that belong to this shape.");
                }


                var shape = db.StampingPlateShapes.Where(b => ID.HasValue && b.ID == ID).SingleOrDefault();
                if (shape == null)
                {
                    return new Response(true);
                }

                db.StampingPlateShapes.Remove(shape);
                db.SaveChanges();

                return new Response(true);
            }
        }
    }
}