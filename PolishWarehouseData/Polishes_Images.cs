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

    public partial class Polishes_Images
    {
        public long ID { get; set; }
        public long PolishID { get; set; }
        public string Image { get; set; }
        public string MIMEType { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> MakerImage { get; set; }
        public bool PublicImage { get; set; }
        public Nullable<bool> DisplayImage { get; set; }
    
        public virtual Polish Polish { get; set; }
    }
}
