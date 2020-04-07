namespace Visitor_Registration_Data.newHoang
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_User()
        {
            tbl_Request_Infor = new HashSet<tbl_Request_Infor>();
            tbl_Taxi_Request_Infor = new HashSet<tbl_Taxi_Request_Infor>();
            tbl_User_Role = new HashSet<tbl_User_Role>();
        }

        [Key]
        [StringLength(20)]
        public string EmployeeId { get; set; }

        [Required]
        [StringLength(20)]
        public string FushanAd { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; }

        [Required]
        [StringLength(150)]
        public string DeparmentName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Request_Infor> tbl_Request_Infor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Taxi_Request_Infor> tbl_Taxi_Request_Infor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_User_Role> tbl_User_Role { get; set; }
    }
}
