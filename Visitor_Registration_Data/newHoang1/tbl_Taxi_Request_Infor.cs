namespace Visitor_Registration_Data.newHoang1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Taxi_Request_Infor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Taxi_Request_Infor()
        {
            tbl_Taxi_Approval_Infor = new HashSet<tbl_Taxi_Approval_Infor>();
            tbl_Taxi_User_Infor = new HashSet<tbl_Taxi_User_Infor>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string EmployeeId { get; set; }

        [Required]
        [StringLength(150)]
        public string Purpose { get; set; }

        [StringLength(100)]
        public string Head { get; set; }

        public DateTime ScheduleTime { get; set; }

        [Required]
        [StringLength(150)]
        public string Pickup { get; set; }

        [Required]
        [StringLength(150)]
        public string DropOff1 { get; set; }

        [StringLength(150)]
        public string DropOff2 { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Taxi_Approval_Infor> tbl_Taxi_Approval_Infor { get; set; }

        public virtual tbl_User tbl_User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Taxi_User_Infor> tbl_Taxi_User_Infor { get; set; }
    }
}
