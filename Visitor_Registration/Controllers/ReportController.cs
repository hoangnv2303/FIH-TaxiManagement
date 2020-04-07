using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Visitor_Registration_Data.Dao;
using VisitorRegistration.Common;

namespace Visitor_Registration.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Report
        [HasCredential(RoleID = "REPORT")]
        public ActionResult Index()
        {
            ViewBag.ListReportDaily = new Taxi_RequestDao().GetListReportDaily();
            SelectList groupList = new SelectList(GetDepartMent(), "Value", "Value");
            ViewBag.GroupList = groupList;
            return View();
        }
    }
}