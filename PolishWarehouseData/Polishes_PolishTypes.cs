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
    
    public partial class Polishes_PolishTypes
    {
        public long ID { get; set; }
        public long PolishID { get; set; }
        public int PolishTypeID { get; set; }
    
        public virtual Polish Polish { get; set; }
        public virtual PolishType PolishType { get; set; }
    }
}