//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mediska.Models
{
    using System;
    
    public partial class cmplxInsertContractAndPackage
    {
        public int PackageID { get; set; }
        public string packageName { get; set; }
        public decimal packagePrice { get; set; }
        public float OffPercent { get; set; }
        public Nullable<decimal> OffPrice { get; set; }
        public Nullable<decimal> FinalPrice { get; set; }
    }
}