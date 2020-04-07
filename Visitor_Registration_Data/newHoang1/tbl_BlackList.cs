namespace Visitor_Registration_Data.newHoang1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_BlackList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public string VisitorName { get; set; }

        [StringLength(50)]
        public string CompanyName { get; set; }

        [Key]
        [StringLength(50)]
        public string NationalId { get; set; }

        [StringLength(150)]
        public string Reason { get; set; }

        [StringLength(20)]
        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
