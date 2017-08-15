using PolishWarehouseData;
using System;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic.FileIO;

namespace PolishWarehouse.Models
{
    public class PolishModel
    {
        public string BrandName { get; set; }
        public string PolishName { get; set; }
        public string ColorName { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public int Coats { get; set; }
        public int Quantity { get; set; }
        public bool HasBeenTried { get; set; }
        public bool WasGift { get; set; }
        public string GiftFromName { get; set; }
        public string Notes { get; set; }

        public static bool processCSV(HttpPostedFileBase file, bool overwriteIfExists = false)
        {
            using (var db = new PolishWarehouseEntities())
            {
                using (var reader = new TextFieldParser(file.InputStream))
                {
                    reader.TextFieldType = FieldType.Delimited;
                    reader.SetDelimiters(",");
                    reader.ReadFields(); //Skip the header
                    while (!reader.EndOfData)
                    {
                        string[] fields = reader.ReadFields();
                        var swatchWheel = fields[(int)Column.swatchWheel];

                        if (!db.Polishes.Any(p => p.Label == swatchWheel) || overwriteIfExists)//The swatch number doesn't exist
                        {
                            var swatchColor = fields[(int)Column.swatchColor];
                            var brand = fields[(int)Column.brand];
                            var colorID = db.Colors.Where(c => c.Name == swatchColor).Select(c => c.ID).SingleOrDefault();
                            var brandID = db.Brands.Where(b => b.Name == brand).Select(b => b.ID).SingleOrDefault();

                            //Add the polish
                            var polish = db.Polishes.Where(p => p.Label == swatchWheel).SingleOrDefault();
                            if (polish == null)
                            {
                                polish = new Polish();
                                db.Polishes.Add(polish);

                            }

                            var colornum = 0;
                            try
                            {
                                colornum = Convert.ToInt32(fields[(int)Column.swatchNum].Trim());
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                throw new Exception(string.Format("Swatch #{0} did not register as a number.", fields[(int)Column.swatchNum]));
                            }

                            var coats = 0;
                            try
                            {
                                coats = Convert.ToInt32(fields[(int)Column.coats].Trim());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                throw new Exception(string.Format("Coat Value for swatch label {0} did not register as a number.", fields[(int)Column.swatchWheel]));
                            }

                            polish.ColorID = colorID;
                            polish.BrandID = brandID;
                            polish.CreatedOn = DateTime.UtcNow;
                            polish.Name = fields[(int)Column.polishName];
                            polish.ColorNumber = colornum;
                            polish.Quantity = 1;
                            polish.Coats = coats;
                            polish.Label = fields[(int)Column.swatchWheel];
                            polish.HasBeenTried = !string.IsNullOrWhiteSpace(fields[(int)Column.tried]);
                            polish.WasGift = !string.IsNullOrWhiteSpace(fields[(int)Column.gift]);

                            db.SaveChanges();

                            //Add the additional info
                            var polishAdditional = db.Polishes_AdditionalInfo.Where(p => p.PolishID == polish.ID).SingleOrDefault();
                            if (polishAdditional == null)
                            {
                                polishAdditional = new Polishes_AdditionalInfo();
                                db.Polishes_AdditionalInfo.Add(polishAdditional);
                            }
                            polishAdditional.PolishID = polish.ID;
                            polishAdditional.Description = fields[(int)Column.desc];

                            db.SaveChanges();

                            //Check to see if we have a valid polish type and add it.
                            var type = fields[(int)Column.type];
                            var ptype = db.PolishTypes.Where(p => p.Name == type).SingleOrDefault();
                            if (ptype != null)
                            {
                                var polishType = db.Polishes_PolishTypes.Where(p => p.PolishID == polish.ID && p.PolishTypeID == ptype.ID).SingleOrDefault();
                                if(polishType == null)//Add this type/polish combo if it did not already exist.
                                {
                                    polishType = new Polishes_PolishTypes()
                                    {
                                        PolishID = polish.ID,
                                        PolishTypeID = ptype.ID
                                    };
                                    db.Polishes_PolishTypes.Add(polishType);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public enum Column
        {
            brand = 0,
            polishName = 1,
            pintrest = 2,
            desc = 3,
            type = 4,
            swatchWheel = 5,
            swatchColor = 6,
            swatchNum = 7,
            coats = 8,
            tried = 9,
            gift = 10,
            notes = 11
        }
    }
}