using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Visitor_Registration.Models
{
    public class VisitorModel
    {
        public VisitorModel() { }

        public List<VisitorModel> visitor { get; set; }
        public int Id { get; set; }

        public Guid Request_Infor_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string VisitorName { get; set; }

        [Required]
        [StringLength(150)]
        public string CompanyName { get; set; }

        public string NationalId { get; set; }

        public int? Badge { get; set; }

        public int? Access { get; set; }

        public int? Tem { get; set; }

        public int? MealCard { get; set; }

        public DateTime? EntryTime { get; set; }

        public DateTime? ExitTime { get; set; }

        public bool Package { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }

        [StringLength(50)]
        public string EscorterId { get; set; }

        [StringLength(150)]
        public string RemarkVisitor { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        public string UpdateBy { get; set; }
        public bool BadgeReturn { get; set; }
        public bool AccessReturn { get; set; }
        public bool MealCardReturn { get; set; }

        public string Type { get; set; }
    }
}