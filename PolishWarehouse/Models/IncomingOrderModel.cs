using PolishWarehouseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolishWarehouse.Models
{
    public class IncomingOrderModel
    {
        public long? ID { get; set; }
        //public byte[] Timestamp { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string Name { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public string Notes { get; set; }
        public string Tracking { get; set; }
        public string TrackingURL { get; set; }
        public string ShippingProviderName { get; set; }
        public Nullable<int> TrackingProviderID { get; set; }
        public decimal? Price { get; set; }
        public bool OrderComplete { get; set; } = false;

        public IEnumerable<IncomingOrderLineModel> Lines { get; set; }

        public IncomingOrderModel() { }

        public IncomingOrderModel(int id)
        {
            using (var db = new PolishWarehouseEntities())
            {
                var o = db.IncomingOrders.Where(oi => oi.ID == id).SingleOrDefault();

                ID = o.ID;
                CreatedOn = o.CreatedOn;
                Name = o.Name;
                Notes = o.Notes;
                Tracking = o.Tracking;
                TrackingProviderID = o.TrackingProviderID;
                Price = o.Price;
                TrackingURL = o.ShippingProvider.TrackingBaseURL + o.Tracking;
                ShippingProviderName = o.ShippingProvider.Name;
                OrderComplete = o.OrderComplete;
                PurchaseDate = o.PurchaseDate;

                if (o.IncomingOrderLines != null)
                    Lines = o.IncomingOrderLines.Select(ol => new IncomingOrderLineModel(ol)).ToArray();
            }
        }

        public IncomingOrderModel(IncomingOrder o)
        {
            ID = o.ID;
            CreatedOn = o.CreatedOn;
            Name = o.Name;
            PurchaseDate = o.PurchaseDate;
            Notes = o.Notes;
            Tracking = o.Tracking;
            TrackingProviderID = o.TrackingProviderID;
            Price = o.Price;
            TrackingURL = o.ShippingProvider.TrackingBaseURL + o.Tracking;
            ShippingProviderName = o.ShippingProvider.Name;
            OrderComplete = o.OrderComplete;

            Lines = o.IncomingOrderLines.Select(ol => new IncomingOrderLineModel(ol)).ToArray();
        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                //Save the Order
                var incomingOrder = db.IncomingOrders.Where(p => p.ID == ID).SingleOrDefault();
                if (incomingOrder == null)
                {
                    incomingOrder = new IncomingOrder();
                    incomingOrder.CreatedOn = DateTime.UtcNow;
                    db.IncomingOrders.Add(incomingOrder);
                }

                if (incomingOrder.OrderComplete)
                    return new Response(false, "Order flagged as complete, save failed.");

                incomingOrder.Name = Name;
                incomingOrder.Notes = Notes;
                incomingOrder.Tracking = Tracking;
                incomingOrder.TrackingProviderID = TrackingProviderID;
                incomingOrder.Price = Price ?? 0;
                incomingOrder.PurchaseDate = PurchaseDate;
                incomingOrder.OrderComplete = OrderComplete;

                db.SaveChanges();
                ID = incomingOrder.ID;

                if (Lines != null)
                {
                    foreach (var line in Lines)
                    {
                        line.Save();
                    }
                }

            }
            return new Response(true);
        }

        public static IncomingOrderModel[] GetIncomingOrderModelList()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var orders = db.IncomingOrders.Select(o => new IncomingOrderModel
                {
                    ID = o.ID,
                    CreatedOn = o.CreatedOn,
                    Name = o.Name,
                    Notes = o.Notes,
                    Tracking = o.Tracking,
                    TrackingProviderID = o.TrackingProviderID,
                    Price = o.Price,
                    TrackingURL = o.ShippingProvider.TrackingBaseURL + o.Tracking,
                    ShippingProviderName = o.ShippingProvider.Name,
                    OrderComplete = o.OrderComplete,
                    PurchaseDate = o.PurchaseDate,
                    Lines = o.IncomingOrderLines.Select(ol => new IncomingOrderLineModel()
                    {
                        ID = ol.ID,
                        CreatedOn = ol.CreatedOn,
                        IncomingOrderID = ol.IncomingOrderID,
                        IncomingLineTypeID = ol.IncomingLineTypeID,
                        Name = ol.Name,
                        Price = ol.Price,
                        Qty = ol.Qty,
                        Notes = ol.Notes,
                        Tracking = ol.Tracking,
                        ShippingProviderID = ol.ShippingProviderID,
                        TrackingURL = ol.ShippingProvider.TrackingBaseURL + ol.Tracking,
                        ShippingProviderName = ol.ShippingProvider.Name,
                        IncomingOrderLinePolish = ol.IncomingOrderLines_Polishes.Select(p => new IncomingOrderLinePolishModel()
                        {
                            ID = p.ID,
                            CreatedOn = p.IncomingOrderLine.CreatedOn,
                            IncomingOrderID = p.IncomingOrderLine.IncomingOrderID,
                            IncomingOrderLinesID = p.IncomingOrderLinesID,
                            IncomingLineTypeID = p.IncomingOrderLine.IncomingLineTypeID,
                            Price = p.IncomingOrderLine.Price,
                            Qty = p.IncomingOrderLine.Qty,
                            Notes = p.IncomingOrderLine.Notes,
                            Tracking = p.IncomingOrderLine.Tracking,
                            ShippingProviderID = p.IncomingOrderLine.ShippingProviderID,
                            TrackingURL = p.IncomingOrderLine.ShippingProvider.TrackingBaseURL + p.IncomingOrderLine.Tracking,
                            ShippingProviderName = p.IncomingOrderLine.ShippingProvider.Name,

                            Color = (p.Color != null) ? new ColorModel()
                            {
                                ID = p.Color.ID,
                                Name = p.Color.Name,
                                Description = p.Color.Description,
                                IsPrimary = p.Color.IsPrimary,
                                IsSecondary = p.Color.IsSecondary,
                                IsGlitter = p.Color.IsGlitter,
                            } : null,
                            Brand = (p.Brand != null) ? new BrandModel()
                            {
                                ID = p.Brand.ID,
                                Name = p.Brand.Name,
                                Description = p.Brand.Description,
                            } : null,
                            ColorID = p.ColorID,
                            BrandID = p.BrandID,
                            PolishName = p.PolishName,
                            Coats = p.Coats,
                            HasBeenTried = p.HasBeenTried,
                            WasGift = p.WasGift,
                            GiftFromName = p.GiftFromName,
                            Description = p.Description,
                        }).FirstOrDefault(),
                    }).OrderBy(p=> p.IncomingLineTypeID),

                }).ToArray();

                foreach (var order in orders){
                    foreach(var line in order.Lines)
                    {
                        if (string.IsNullOrWhiteSpace(line.Tracking))
                        {
                            line.Tracking = order.Tracking;
                            line.TrackingURL = order.TrackingURL;
                        }
                        if(line.IncomingOrderLinePolish != null)
                        {
                            if (string.IsNullOrWhiteSpace(line.IncomingOrderLinePolish.Tracking))
                            {
                                line.IncomingOrderLinePolish.Tracking = order.Tracking;
                                line.IncomingOrderLinePolish.TrackingURL = order.TrackingURL;
                            }
                        }
                    }
                }

                return orders;
            }
        }

        public static IncomingOrderModel GetIncomingOrder(long id)
        {
            return GetIncomingOrderModelList().Where(o => o.ID == id).SingleOrDefault();
        }

    }

    public class IncomingOrderLineModel
    {
        public long? ID { get; set; }
        //public byte[] Timestamp { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long IncomingOrderID { get; set; }
        public long IncomingLineTypeID { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public string Notes { get; set; }
        public string Tracking { get; set; }
        public string TrackingURL { get; set; }
        public string ShippingProviderName { get; set; }
        public Nullable<int> ShippingProviderID { get; set; }

        public IncomingOrderLineTypeModel IncomingLineType { get; set; }
        public ShippingProviderModel ShippingProvider { get; set; }
        public IncomingOrderLinePolishModel IncomingOrderLinePolish { get; set; }

        public IncomingOrderLineModel() { }
        public IncomingOrderLineModel(IncomingOrderLine o)
        {
            ID = o.ID;
            CreatedOn = o.CreatedOn;
            IncomingOrderID = o.IncomingOrderID;
            IncomingLineTypeID = o.IncomingLineTypeID;
            Name = o.Name;
            Price = o.Price;
            Qty = o.Qty;
            Notes = o.Notes;
            Tracking = o.Tracking;
            ShippingProviderID = o.ShippingProviderID;
            TrackingURL = o.ShippingProvider.TrackingBaseURL + o.Tracking;
            ShippingProviderName = o.ShippingProvider.Name;

            IncomingLineType = new IncomingOrderLineTypeModel(o.IncomingLineType);
            ShippingProvider = new ShippingProviderModel(o.ShippingProvider);
            if (o.IncomingOrderLines_Polishes.Count > 0)
                IncomingOrderLinePolish = new IncomingOrderLinePolishModel(o.IncomingOrderLines_Polishes.SingleOrDefault());

        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                //Save the Order
                var incomingOrderline = db.IncomingOrderLines.Where(p => p.ID == ID).SingleOrDefault();
                if (incomingOrderline == null)
                {
                    incomingOrderline = new IncomingOrderLine();
                    incomingOrderline.CreatedOn = DateTime.UtcNow;
                    db.IncomingOrderLines.Add(incomingOrderline);
                }

                incomingOrderline.Name = Name;
                incomingOrderline.IncomingOrderID = IncomingOrderID;
                incomingOrderline.IncomingLineTypeID = IncomingLineTypeID;
                incomingOrderline.Price = Price;
                incomingOrderline.Qty = Qty;
                incomingOrderline.Notes = Notes;
                incomingOrderline.Tracking = Tracking;
                incomingOrderline.ShippingProviderID = ShippingProviderID;

                db.SaveChanges();
            }
            return new Response(true);
        }
    }

    public class IncomingOrderLinePolishModel
    {
        public long ID { get; set; }
        //public byte[] Timestamp { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string Name { get; set; }
        public long IncomingOrderID { get; set; }
        public long IncomingLineTypeID { get; set; }
        public long IncomingOrderLinesID { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public string Notes { get; set; }
        public string Tracking { get; set; }
        public string TrackingURL { get; set; }
        public string ShippingProviderName { get; set; }
        public Nullable<int> ShippingProviderID { get; set; }

        public IncomingOrderLineTypeModel IncomingLineType { get; set; }
        public ShippingProviderModel ShippingProvider { get; set; }
        //public IncomingOrderModel IncomingOrder { get; set; }

        public int? ColorID { get; set; }
        public int? BrandID { get; set; }
        public string PolishName { get; set; }
        public int? Coats { get; set; }
        public bool HasBeenTried { get; set; }
        public bool WasGift { get; set; }
        public string GiftFromName { get; set; }
        public string Description { get; set; }
        public long? PolishID { get; set; }
        public bool Converted { get; set; } = false;

        public BrandModel Brand { get; set; }
        public ColorModel Color { get; set; }

        public IncomingOrderLinePolishModel() { }
        public IncomingOrderLinePolishModel(IncomingOrderLines_Polishes o)
        {
            ID = o.ID;
            CreatedOn = o.IncomingOrderLine.CreatedOn;
            IncomingOrderID = o.IncomingOrderLine.IncomingOrderID;
            IncomingOrderLinesID = o.IncomingOrderLinesID;
            IncomingLineTypeID = o.IncomingOrderLine.IncomingLineTypeID;
            Price = o.IncomingOrderLine.Price;
            Qty = o.IncomingOrderLine.Qty;
            Notes = o.IncomingOrderLine.Notes;
            Tracking = o.IncomingOrderLine.Tracking;
            ShippingProviderID = o.IncomingOrderLine.ShippingProviderID;
            TrackingURL = o.IncomingOrderLine.ShippingProvider.TrackingBaseURL + o.IncomingOrderLine.Tracking;
            ShippingProviderName = o.IncomingOrderLine.ShippingProvider.Name;

            IncomingOrderLinesID = o.IncomingOrderLine.ID;
            ColorID = o.ColorID;
            BrandID = BrandID;
            PolishName = o.PolishName;
            Coats = o.Coats;
            HasBeenTried = o.HasBeenTried;
            WasGift = o.WasGift;
            GiftFromName = o.GiftFromName;
            Description = o.Description;
            Converted = o.Converted;

            IncomingLineType = new IncomingOrderLineTypeModel(o.IncomingOrderLine.IncomingLineType);
            ShippingProvider = new ShippingProviderModel(o.IncomingOrderLine.ShippingProvider);


            Brand = o.Brand == null ? null : new BrandModel(o.Brand);
            Color = o.Color == null ? null : new ColorModel(o.Color);

        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                //Save the Order
                var incomingOrderline = db.IncomingOrderLines.Where(p => p.ID == IncomingOrderLinesID).SingleOrDefault();
                if (incomingOrderline == null)
                {
                    incomingOrderline = new IncomingOrderLine();
                    incomingOrderline.CreatedOn = DateTime.UtcNow;
                    db.IncomingOrderLines.Add(incomingOrderline);
                }

                incomingOrderline.Name = Name;
                incomingOrderline.IncomingOrderID = IncomingOrderID;
                incomingOrderline.IncomingLineTypeID = IncomingLineTypeID;
                incomingOrderline.Price = Price;
                incomingOrderline.Qty = Qty;
                incomingOrderline.Notes = Notes;
                incomingOrderline.Tracking = Tracking;
                incomingOrderline.ShippingProviderID = ShippingProviderID;

                db.SaveChanges();

                var incomingPolish = db.IncomingOrderLines_Polishes.Where(p => p.ID == ID).SingleOrDefault();
                if (incomingPolish == null)
                {
                    incomingPolish = new IncomingOrderLines_Polishes();
                    incomingPolish.CreatedOn = DateTime.UtcNow;
                    db.IncomingOrderLines_Polishes.Add(incomingPolish);
                }

                incomingPolish.IncomingOrderLinesID = incomingOrderline.ID;
                incomingPolish.ColorID = ColorID;
                incomingPolish.BrandID = BrandID;
                incomingPolish.PolishName = PolishName;
                incomingPolish.Coats = Coats.HasValue ? Coats.Value : incomingPolish.Coats;
                incomingPolish.HasBeenTried = HasBeenTried;
                incomingPolish.WasGift = WasGift;
                incomingPolish.GiftFromName = GiftFromName;
                incomingPolish.Description = Description;
                incomingPolish.Converted = Converted;

                db.SaveChanges();
            }
            return new Response(true);
        }

        public Response ConvertToPolish(DupeAction dupeAction = DupeAction.DoNotImport)
        {
            if (Converted)
                return new Response(false, "Line already imported");

            //if (Brand == null)
            //   return new Response(false,"Polish brand required.");

            //if (Color == null)
            //    return new Response(false, "Polish primary color required.");


            using (var db = new PolishWarehouseEntities())
            {
                var existing = db.Polishes.Where(p => p.Name == PolishName && p.Brand.ID == Brand.ID).ToArray();
               
                if (existing.Count() > 0)
                {
                    var inc = db.IncomingOrderLines_Polishes.Where(p => p.ID == ID).SingleOrDefault();
                    switch (dupeAction)
                    {
                        case DupeAction.DoNotImport:
                            return new Response(true, "Polish Already exists.", null);
                        case DupeAction.AddQty:
                            existing[0].Quantity += Qty;
                            inc.Converted = true;
                            db.SaveChanges();
                            return new Response(true, $"Incoming polish quantity was added to existing polish", new PolishModel(existing[0]));
                        case DupeAction.AddNew:
                            break;
                        case DupeAction.Prompt:
                            return new Response(false, "Prompt", new { prompt = true });
                    }
                }


                var colorNum = Color == null ? 0: PolishModel.getNextColorNumber(Color.ID.Value);

                var label = Color == null ? "" : $"{Color.Name} {colorNum.ToString()}";

                var polish = new PolishModel()
                {
                    BrandID = Brand.ID.Value,
                    ColorID = Color == null ?  0 : Color.ID.Value,
                    BrandName = Brand.Name,
                    PolishName = PolishName,
                    ColorName = Color == null ? null : Color.Name,
                    ColorNumber = colorNum,
                    Description = Description,
                    Label = label,
                    Coats = Coats,
                    Quantity = Qty,
                    HasBeenTried = HasBeenTried,
                    WasGift = WasGift,
                    GiftFromName = GiftFromName,
                    Notes = Notes,
                };

                //polish.Save();
                //Converted = true;
                //inc.Converted = true;
                //db.SaveChanges();


                return new Response(p: polish);
            }
        }

        public enum DupeAction
        {
            Prompt,
            DoNotImport,
            AddQty,
            AddNew
        }
    }

    public class IncomingOrderLineTypeModel
    {
        public long ID { get; set; }
        //public byte[] Timestamp { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string Name { get; set; }

        public IncomingOrderLineTypeModel() { }
        public IncomingOrderLineTypeModel(IncomingLineType o)
        {
            ID = o.ID;
            CreatedOn = o.CreatedOn;
            Name = o.Name;
        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var line = db.IncomingLineTypes.Where(p => p.ID == ID).SingleOrDefault();
                if (line == null)
                {
                    line = new IncomingLineType();
                    line.CreatedOn = DateTime.UtcNow;
                    db.IncomingLineTypes.Add(line);
                }

                line.Name = Name;

                db.SaveChanges();
            }
            return new Response(true);
        }

        public static IncomingOrderLineTypeModel[] GetLineTypes()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.IncomingLineTypes.Select(o => new IncomingOrderLineTypeModel()
                {
                    ID = o.ID,
                    CreatedOn = o.CreatedOn,
                    Name = o.Name
                }).ToArray();
            }
        }
    }

    public class ShippingProviderModel
    {
        public long ID { get; set; }
        //public byte[] Timestamp { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string Name { get; set; }
        public string TrackingBaseUrl { get; set; }
        public ShippingProviderModel() { }
        public ShippingProviderModel(ShippingProvider o)
        {
            ID = o.ID;
            CreatedOn = o.CreatedOn;
            Name = o.Name;
            TrackingBaseUrl = o.TrackingBaseURL;
        }

        public Response Save()
        {
            using (var db = new PolishWarehouseEntities())
            {
                var provider = db.ShippingProviders.Where(p => p.ID == ID).SingleOrDefault();
                if (provider == null)
                {
                    provider = new ShippingProvider();
                    provider.CreatedOn = DateTime.UtcNow;
                    db.ShippingProviders.Add(provider);
                }

                provider.Name = Name;
                provider.TrackingBaseURL = TrackingBaseUrl;

                db.SaveChanges();
            }
            return new Response(true);
        }

        public static ShippingProviderModel[] GetShippingProviders()
        {
            using (var db = new PolishWarehouseEntities())
            {
                return db.ShippingProviders.Select(o => new ShippingProviderModel()
                {
                    ID = o.ID,
                    CreatedOn = o.CreatedOn,
                    Name = o.Name,
                    TrackingBaseUrl = o.TrackingBaseURL
                }).ToArray();
            }
        }
    }
}