namespace Visitor_Registration_Data.newHoang
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_User_Role
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Role_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string EmployeeId { get; set; }

        public DateTime? CreateDate { get; set; }

        public virtual tbl_Role tbl_Role { get; set; }

        public virtual tbl_User tbl_User { get; set; }
    }
}
