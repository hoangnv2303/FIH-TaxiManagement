namespace Visitor_Registration_Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Request_Infor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Request_Infor()
        {
            tbl_Visitor_Infor = new HashSet<tbl_Visitor_Infor>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string EmployeeId { get; set; }

        [Required]
        [StringLength(150)]
        public string PurposeVisit { get; set; }

        public DateTime IncommingDate { get; set; }

        public DateTime OutgoingDate { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual tbl_User tbl_User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Visitor_Infor> tbl_Visitor_Infor { get; set; }
    }
}
