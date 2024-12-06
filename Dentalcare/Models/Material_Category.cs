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

    public partial class Material_Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material_Category()
        {
            this.Materials = new HashSet<Material>();
        }
    
        [DisplayName("Mã số")]
        public string id { get; set; }
        [DisplayName("Tên loại vật liệu")]
        [Required(ErrorMessage = "Vui lòng nhập loại vật liệu.")]
        public string name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả.")]
        [DisplayName("Mô tả")]
        public string descrip { get; set; }
        [DisplayName("Ghi chú")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string note { get; set; }
        public string meta { get; set; }
        [DisplayName("Hiển thị")]
        public bool hide { get; set; }
        public bool able { get; set; }
        public int order { get; set; }
        [ValidPositiveInt]
        [DisplayName("Vị trí")]
        [Required(ErrorMessage = "Vui lòng nhập vị trí.")]
        public int new_order { get; set; }
        [DisplayName("Ngày cập nhật")]
        public System.DateTime datebegin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material> Materials { get; set; }
    }
}
