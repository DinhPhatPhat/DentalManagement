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
    
    public partial class Service_Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service_Category()
        {
            this.Services = new HashSet<Service>();
        }
    
        public string id { get; set; }
        public string name { get; set; }
        public string descrip { get; set; }
        public string note { get; set; }
        public string meta { get; set; }
        public bool hide { get; set; }
        public bool able { get; set; }
        public int order { get; set; }
        public System.DateTime datebegin { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> Services { get; set; }
    }
}
