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
    
    public partial class Medicine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Medicine()
        {
            this.Prescription_Medicine = new HashSet<Prescription_Medicine>();
        }
    
        public string caredACtor { get; set; }
        public int price { get; set; }
        public bool hide { get; set; }
        public string meta { get; set; }
        public string instruction { get; set; }
        public int order { get; set; }
        public System.DateTime datebegin { get; set; }
        public bool able { get; set; }
        public string id { get; set; }
    
        public virtual ConsumableMaterial ConsumableMaterial { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prescription_Medicine> Prescription_Medicine { get; set; }
    }
}
