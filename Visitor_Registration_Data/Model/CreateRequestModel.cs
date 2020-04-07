using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor_Registration_Data.EF;

namespace Visitor_Registration_Data.Model
{
    public class CreateRequestModel
    {
        public CreateRequestModel() {
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

        public List<tbl_Taxi_User_Infor> Visitor { get; set; }
        public string ListVisitorCSV { get; set; }

        // Requester infor
        public string FullName { get; set; }
        public string SLM { get; set; }

    }
}
