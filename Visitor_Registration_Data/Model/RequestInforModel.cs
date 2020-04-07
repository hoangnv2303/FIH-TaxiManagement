using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor_Registration_Data.Model
{
    public class RequestInforModel
    {
 
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string EmployeeId { get; set; }

        [Required]
        [StringLength(20)]
        public string User_EmployeeId { get; set; }

        [Required]
        [StringLength(20)]
        public string EmployeeId_Old { get; set; }

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

        public DateTime? User_CreateDate { get; set; }

        [StringLength(150)]
        public string User_Remark { get; set; }

        [Required]
        public int Process { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string SLM_Name { get; set; }

        public DateTime? SwipeTime { get; set; }

        public int? CarNumber { get; set; }

        [Column(TypeName = "money")]
        public decimal? Cost { get; set; }

        public int? CardNumber { get; set; }

        public long? RefNumber { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        public string UpdateBy { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }

        public Guid Request_Id { get; set; }

        [Required]
        [StringLength(150)]
        public string DepartmentName { get; set; }

        [StringLength(20)]
        public string ApproverId { get; set; }

        [StringLength(150)]
        public string Approval_Remark { get; set; }

        public DateTime Approval_UpdateDate { get; set; }

        public int Approval_Process { get; set; }

        public int State { get; set; }

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

    }
}
