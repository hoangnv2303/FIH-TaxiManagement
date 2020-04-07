namespace Visitor_Registration_Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Department_Infor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Department_Infor()
        {
        }

        [Key]
        [StringLength(150)]
        public string DepartmentName { get; set; }

        [Required]
        [StringLength(20)]
        public string Head_id { get; set; }

        [StringLength(200)]
        public string Deputy_id { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
