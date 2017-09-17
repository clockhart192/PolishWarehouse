using PolishWarehouseData;
using System;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic.FileIO;

namespace PolishWarehouse.Models
{
    public class PolishModel
    {
        public long? ID { get; set; }
        public string PolishName { get; set; }
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public int ColorNumber { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public int? Coats { get; set; }
        public int? Quantity { get; set; }
        public bool HasBeenTried { get; set; } = false;
        public bool WasGift { get; set; } = false;
        public string GiftFromName { get; set; }
        public string Notes { get; set; }
        public string MakerImage { get; set; }
        public string MakerImageURL { get; set; }
        public string SelfImage { get; set; }
        public string SelfImageURL { get; set; }
        public Color[] SecondaryColors { get; set; }
        public Color[] GlitterColors { get; set; }
        public PolishType[] Types { get; set; }
        public int[] SecondaryColorsIDs { get; set; }
        public int[] GlitterColorsIDs { get; set; }
        public int[] TypesIDs { get; set; }

        public PolishModel() { }
        public PolishModel(int? id)
        {
            if (!id.HasValue)
                return;

            using (var db = new PolishWarehouseEntities())
            {
                var p = db.Polishes.Where(po => po.ID == id).SingleOrDefault();

                ID = p.ID;
                BrandID = p.BrandID;
                ColorID = p.ColorID;
                BrandName = p.Brand.Name;
                PolishName = p.Name;
                ColorName = p.Color.Name;
                ColorNumber = p.ColorNumber;
                Description = p.Polishes_AdditionalInfo.Description;
                Label = p.Label;
                Coats = p.Coats;
                Quantity = p.Quantity;
                HasBeenTried = p.HasBeenTried;
                WasGift = p.WasGift;
                GiftFromName = p.Polishes_AdditionalInfo.GiftFromName;
                Notes = p.Polishes_AdditionalInfo.Notes;
                SecondaryColors = p.Polishes_Secondary_Colors.Select(pec => pec.Color).ToArray();
                GlitterColors = p.Polishes_Glitter_Colors.Select(pec => pec.Color).ToArray();
                Types = p.Polishes_PolishTypes.Select(ppt => ppt.PolishType).ToArray();
                MakerImage = p.Polishes_AdditionalInfo.MakerImage;
                MakerImageURL = p.Polishes_AdditionalInfo.MakerImageURL;
                SelfImage = p.Polishes_AdditionalInfo.SelfImage;
                SelfImageURL = p.Polishes_AdditionalInfo.SelfImageURL;
            }
        }

        public static bool processCSV(HttpPostedFileBase file, bool overwriteIfExists = false)
        {

            //db.Configuration.AutoDetectChangesEnabled = false;
            using (var reader = new TextFieldParser(file.InputStream))
            {
                reader.TextFieldType = FieldType.Delimited;
                reader.SetDelimiters(",");
                reader.ReadFields(); //Skip the header
                while (!reader.EndOfData)
                {
                    using (var db = new PolishWarehouseEntities())
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
                            catch (Exception ex)
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
                                if (polishType == null)//Add this type/polish combo if it did not already exist.
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

        public static Color[] getPrimaryColors()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.Colors.Where(c => c.IsPrimary).ToArray();
            }
        }

        public static Color[] getSecondaryColors()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.Colors.Where(c => c.IsSecondary).ToArray();
            }
        }

        public static Color[] getGlitterColors()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.Colors.Where(c => c.IsGlitter).ToArray();
            }
        }

        public static Brand[] getBrands()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.Brands.ToArray();
            }
        }

        public static PolishType[] getPolishTypes()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.PolishTypes.ToArray();
            }
        }

        public static int getNextColorNumber(int colorID)
        {
            using (var db = new PolishWarehouseEntities())
            {
                var polishes = db.Polishes.Where(p => p.ColorID == colorID).ToArray();

                //This is our first one for this color clearly.
                if (polishes.Length <= 0)
                    return 1;

                //loop through the polishes until you find one where the i and the color number does not align,
                //this should be because there is a gap in the color numbers and we want to fill that gap.
                for (int i = 0; i < polishes.Length; i++)
                {
                    if (polishes[i].ColorNumber != (i + 1))
                    {
                        return i + 1;
                    }
                }

                //If we got this far, we are going to the next number in line.
                return polishes.Length + 1;

                //throw new Exception("A color number could not be generated.");
            }
        }

        public bool Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                //Add the polish
                var polish = db.Polishes.Where(p => p.ID == ID).SingleOrDefault();
                if (polish == null)
                {
                    polish = new Polish();
                    db.Polishes.Add(polish);
                }

                var colornum = 0;
                try
                {
                    colornum = ColorNumber;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception(string.Format("Swatch #{0} did not register as a number.", ColorNumber));
                }

                ColorName = db.Colors.Where(c => c.ID == ColorID).Select(c => c.Name).SingleOrDefault();
                Label = string.Format("{0} {1}", ColorName, colornum.ToString());
                Types = TypesIDs == null ? null : db.PolishTypes.Where(pt => TypesIDs.Contains(pt.ID)).ToArray();
                SecondaryColors = SecondaryColorsIDs == null ? null : db.Colors.Where(c => SecondaryColorsIDs.Contains(c.ID)).ToArray();
                GlitterColors = GlitterColorsIDs == null ? null : db.Colors.Where(c => GlitterColorsIDs.Contains(c.ID)).ToArray();


                polish.ColorID = ColorID;
                polish.BrandID = BrandID;
                polish.CreatedOn = DateTime.UtcNow;
                polish.Name = PolishName;
                polish.ColorNumber = colornum;
                polish.Quantity = 1;
                polish.Coats = Coats.HasValue ? Coats.Value : 1;
                polish.Label = Label;
                polish.HasBeenTried = HasBeenTried;
                polish.WasGift = WasGift;

                db.SaveChanges();
                ID = polish.ID;

                //Add the additional info
                var polishAdditional = db.Polishes_AdditionalInfo.Where(p => p.PolishID == polish.ID).SingleOrDefault();
                if (polishAdditional == null)
                {
                    polishAdditional = new Polishes_AdditionalInfo();
                    db.Polishes_AdditionalInfo.Add(polishAdditional);
                }
                polishAdditional.PolishID = polish.ID;
                polishAdditional.Description = Description;
                polishAdditional.Notes = Notes;
                polishAdditional.GiftFromName = GiftFromName;
                //polishAdditional.MakerImage = MakerImage;
                polishAdditional.MakerImageURL = MakerImageURL;
                //polishAdditional.SelfImage = SelfImage;
                polishAdditional.SelfImageURL = SelfImageURL;

                db.SaveChanges();

                //Polish Types
                //If this is a new add, this should be empty.
                //If it is not, we need to purge all of these so that we can refresh the table.
                var oldPtypes = db.Polishes_PolishTypes.Where(p => p.PolishID == polish.ID).ToArray();
                db.Polishes_PolishTypes.RemoveRange(oldPtypes);
                db.SaveChanges();

                if (Types != null)
                {
                    foreach (var ptype in Types)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var polishType = db.Polishes_PolishTypes.Where(p => p.PolishID == polish.ID && p.PolishTypeID == ptype.ID).SingleOrDefault();
                        if (polishType == null)//Add this type/polish combo if it did not already exist.
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


                if (SecondaryColors != null)
                {
                    //Secondary Colors
                    //If this is a new add, this should be empty.
                    //If it is not, we need to purge all of these so that we can refresh the table.
                    var oldSColors = db.Polishes_Secondary_Colors.Where(p => p.PolishID == polish.ID).ToArray();
                    db.Polishes_Secondary_Colors.RemoveRange(oldSColors);
                    db.SaveChanges();

                    foreach (var color in SecondaryColors)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var secondaryColor = db.Polishes_Secondary_Colors.Where(sc => sc.ColorID == color.ID && sc.PolishID == polish.ID).SingleOrDefault();
                        if (secondaryColor == null)
                        {
                            secondaryColor = new Polishes_Secondary_Colors()
                            {
                                PolishID = polish.ID,
                                ColorID = color.ID
                            };
                            db.Polishes_Secondary_Colors.Add(secondaryColor);
                            db.SaveChanges();
                        }
                    }
                }

                //Glitter Colors
                //If this is a new add, this should be empty.
                //If it is not, we need to purge all of these so that we can refresh the table.
                var oldGColors = db.Polishes_Glitter_Colors.Where(p => p.PolishID == polish.ID).ToArray();
                db.Polishes_Glitter_Colors.RemoveRange(oldGColors);
                db.SaveChanges();

                if (GlitterColors != null)
                {
                    foreach (var color in GlitterColors)
                    {
                        //Given the above delete, this should always be null, sanity check though.
                        var glitterColor = db.Polishes_Glitter_Colors.Where(gc => gc.ColorID == color.ID && gc.PolishID == polish.ID).SingleOrDefault();
                        if (glitterColor == null)
                        {
                            glitterColor = new Polishes_Glitter_Colors()
                            {
                                PolishID = polish.ID,
                                ColorID = color.ID
                            };
                            db.Polishes_Glitter_Colors.Add(glitterColor);
                            db.SaveChanges();
                        }
                    }
                }



            }
            return true;
        }

        public Response Delete()
        {
            using (var db = new PolishWarehouseEntities())
            {
                //Remove the dependants
                var additional = db.Polishes_AdditionalInfo.Where(a => a.PolishID == ID).SingleOrDefault();
                if (additional != null)
                    db.Polishes_AdditionalInfo.Remove(additional);

                var secondary = db.Polishes_Secondary_Colors.Where(a => a.PolishID == ID).ToArray();
                if (secondary != null)
                    db.Polishes_Secondary_Colors.RemoveRange(secondary);

                var glitter = db.Polishes_Glitter_Colors.Where(a => a.PolishID == ID).ToArray();
                if (glitter != null)
                    db.Polishes_Glitter_Colors.RemoveRange(glitter);

                var types = db.Polishes_PolishTypes.Where(a => a.PolishID == ID).ToArray();
                if (types != null)
                    db.Polishes_PolishTypes.RemoveRange(types);

                //Remove the polish
                var polish = db.Polishes.Where(p => p.ID == ID.Value).SingleOrDefault();

                db.Polishes.Remove(polish);
                //db.SaveChanges();

                return new Response(false,"Polishes can't be removed yet like this because your husband didn't do it right.");
            }
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

    public class PolishDetailModel
    {
        public PolishModel Model { get; set; }
        public Color[] PrimaryColors { get; set; }
        public Color[] SecondaryColors { get; set; }
        public Color[] GlitterColors { get; set; }
        public Brand[] Brands { get; set; }
    }
}