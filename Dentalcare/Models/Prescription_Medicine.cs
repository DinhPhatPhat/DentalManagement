//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dentalcare.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Prescription_Medicine
    {
        public bool able { get; set; }
        public string denid { get; set; }
        public string patid { get; set; }
        public string billid { get; set; }
        public string medId { get; set; }
        public int quantityMedicine { get; set; }
        public string meta { get; set; }
        public bool hide { get; set; }
        public int order { get; set; }
        public System.DateTime datebegin { get; set; }
    
        public virtual Medicine Medicine { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
