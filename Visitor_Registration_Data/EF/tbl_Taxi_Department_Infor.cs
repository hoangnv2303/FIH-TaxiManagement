namespace Visitor_Registration_Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Taxi_Department_Infor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Taxi_Department_Infor()
        {
            tbl_Taxi_Approval_Infor = new HashSet<tbl_Taxi_Approval_Infor>();
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Taxi_Approval_Infor> tbl_Taxi_Approval_Infor { get; set; }
    }
}
