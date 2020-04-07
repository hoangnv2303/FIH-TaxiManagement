using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor_Registration_Data.Model
{
    public class MyRequestModel
    {

        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string EmployeeId { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(150)]
        public string Purpose { get; set; }

        [StringLength(100)]
        public string Head { get; set; }

        public DateTime ScheduleTime { get; set; }

        [Required]
        [StringLength(150)]
        public string DepartmentName { get; set; }

        [StringLength(20)]
        public string ApproverId { get; set; }

        [StringLength(150)]
        public string ApprovalRemark { get; set; }

        public DateTime UpdateDate { get; set; }

        public int Process { get; set; }

        public int State { get; set; }

        public int Result { get; set; }

        public int NoShow { get; set; }
    }
}
