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
    
    public partial class spMDSKGetCustomerByUserID_Result
    {
        public int customerID { get; set; }
        public string customerManagerName { get; set; }
        public string customerManagerFamily { get; set; }
        public string customerManagerMobileNo { get; set; }
        public string customerMelliNo { get; set; }
        public string customerAddress { get; set; }
        public Nullable<System.DateTime> customerBirthDate { get; set; }
        public int customerCustomerGroupID { get; set; }
        public string custgroupName { get; set; }
        public bool customerManagerGender { get; set; }
        public int customerAreaID { get; set; }
        public string areaFullTitle { get; set; }
    }
}
