namespace Visitor_Registration_Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Taxi_Approval_Infor
    {
        public int Id { get; set; }

        public Guid Request_Id { get; set; }

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

        public virtual tbl_Taxi_Department_Infor tbl_Taxi_Department_Infor { get; set; }

        public virtual tbl_Taxi_Request_Infor tbl_Taxi_Request_Infor { get; set; }
    }
}
