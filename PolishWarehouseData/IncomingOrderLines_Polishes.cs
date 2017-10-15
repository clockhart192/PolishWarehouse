//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PolishWarehouseData
{
    using System;
    using System.Collections.Generic;
    
    public partial class IncomingOrderLines_Polishes
    {
        public long ID { get; set; }
        public byte[] Timestamp { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long IncomingOrderLinesID { get; set; }
        public int ColorID { get; set; }
        public int BrandID { get; set; }
        public string PolishName { get; set; }
        public int Coats { get; set; }
        public bool HasBeenTried { get; set; }
        public bool WasGift { get; set; }
        public string GiftFromName { get; set; }
        public Nullable<long> PolishID { get; set; }
        public string Description { get; set; }
    
        public virtual Brand Brand { get; set; }
        public virtual Color Color { get; set; }
        public virtual IncomingOrderLine IncomingOrderLine { get; set; }
    }
}
