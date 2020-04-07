using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Visitor_Registration.Models
{
    public class ApprovalModel
    {
        public ApprovalModel() { }
        
        public List<ApprovalModel> approval { get; set; }
        public Guid Id { get;set; }
        public int type { get; set; }
        public int process { get; set; }
        public string comment { get; set; }
    }
}