
namespace Visitor_Registration_Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_BlackList
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_BlackList()
        {
        }
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string VisitorName { get; set; }

        [Required]
        [StringLength(150)]
        public string CompanyName { get; set; }
        [Required]
        [StringLength(50)]
        public string NationalId { get; set; }

        [Required]
        [StringLength(150)]
        public string Reason { get; set; }

        [Required]
        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
