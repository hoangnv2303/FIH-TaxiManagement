namespace Visitor_Registration_Data.newHoang1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Approval
    {
        public int Id { get; set; }

        public Guid Request_Infor_Id { get; set; }

        [Required]
        [StringLength(150)]
        public string DepartmentName { get; set; }

        [StringLength(20)]
        public string ApproverId { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }

        public DateTime UpdateDate { get; set; }

        public int Process { get; set; }

        public int State { get; set; }

        public virtual tbl_Department_Infor tbl_Department_Infor { get; set; }

        public virtual tbl_Request_Infor tbl_Request_Infor { get; set; }
    }
}
