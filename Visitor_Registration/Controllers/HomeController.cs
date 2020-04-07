using EquipmentRegistation.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Visitor_Registration_Data.Dao;
using Visitor_Registration_Data.EF;
using VisitorRegistration.Common;
using VisitorRegistration.Models;

namespace Visitor_Registration.Controllers
{
    public class HomeController : BaseController
    {
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index(int? type)
        {
            if (type == 0)
            {
                Session["Type"] = type;
            }
            return View();
        }
        //Public show in dashboard
        public PartialViewResult ShowListRequest(int type)
        {
            // Get list request

            var request = new Taxi_RequestDao().GetListRequests(type, mEmployee.employee.employee_id).FindAll(x => x.Process != 1);
            Session["Type"] = type;
            ViewBag.TypeName = (type == 0) ? "new requests" : ((type == 1) ? "requests approved" : ((type == 2) ? "requests in company" : ((type == 4) ? "requests check in/out" : "requests closed"))); // 4: combine checkin/out
            ViewBag.ListRequests = request;
            return PartialView();
        }

        // Security
        public ActionResult ShowSecurity()
        {
            // Type = 1
            return View();
        }

        // Receptionist
        public ActionResult ShowReceptionist()
        {
            // Type = 4
            return View();
        }

    }
}