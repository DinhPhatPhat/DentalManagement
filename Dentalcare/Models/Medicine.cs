﻿//------------------------------------------------------------------------------
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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Medicine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Medicine()
        {
            this.Prescription_Medicine = new HashSet<Prescription_Medicine>();
        }
        [DisplayName("Đối tượng sử dụng")]
        [Required(ErrorMessage = "Vui lòng đối tượng sử dụng.")]
        public string caredACtor { get; set; }
        [ValidPositiveInt]
        [DisplayName("Giá")]
        [Required(ErrorMessage = "Vui lòng nhập giá.")]
        public int price { get; set; }
        [DisplayName("Hiển thị")]
        public bool hide { get; set; }
        [DisplayName("Hướng dẫn sử dụng")]
        [Required(ErrorMessage = "Vui lòng nhâp hướng dẫn sử dụng.")]
        public string instruction { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập meta.")]
        public string meta { get; set; }
        public int order { get; set; }
        [ValidPositiveInt]
        [DisplayName("Vị trí")]
        [Required(ErrorMessage = "Vui lòng nhập vị trí.")]
        public int new_order { get; set; }
        [DisplayName("Ngày cập nhật")]
        public System.DateTime datebegin { get; set; }
        public bool able { get; set; }
        [DisplayName("Mã số")]
        public string id { get; set; }

        public virtual ConsumableMaterial ConsumableMaterial { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prescription_Medicine> Prescription_Medicine { get; set; }
    }
}
