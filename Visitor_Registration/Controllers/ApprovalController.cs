using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Visitor_Registration.Models;
using Visitor_Registration_Data.Dao;
using Visitor_Registration_Data.EF;
using VisitorRegistration.Common;
using VisitorRegistration.Helper;

namespace Visitor_Registration.Controllers
{
    public class ApprovalController : BaseController
    {
        [HttpGet]
        [HasCredential(RoleID = "APPROVER")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Approval(int levelType)
        {
            var request = new Taxi_ApprovalDao().GetListApproval(mEmployee.employee.employee_id, levelType);
            ViewBag.ListRequests = request;
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "APPROVER")]
        public ActionResult Approval(List<ApprovalModel> approval)
        {
            bool check = true;
            foreach (var item in approval)
            {
                if (item.type != 0)
                {
                    // set approval
                    tbl_Taxi_Approval_Infor approve = new tbl_Taxi_Approval_Infor();
                    approve.Request_Id = item.Id;
                    approve.Process = item.process;
                    approve.ApproverId = mEmployee.employee.employee_id;
                    approve.Remark = (item.comment != null) ? item.comment.ToString() : null;
                    approve.UpdateDate = DateTime.Now;
                    approve.State = item.type;
                    var approvalDao = new Taxi_ApprovalDao().InsertOrUpdateApproval(approve);
                    // If approve process 2 -> insert new row
                    var resultNew = true;
                    if (item.type == 1 && item.process == 1)
                    {
                        tbl_Taxi_Approval_Infor approveNew = new tbl_Taxi_Approval_Infor();
                        approveNew.Request_Id = item.Id;
                        approveNew.Process = 2;
                        approveNew.UpdateDate = DateTime.Now;
                        approveNew.State = 0;
                        approveNew.DepartmentName = DepartmentLevel2;
                        resultNew = new Taxi_ApprovalDao().InsertOrUpdateApproval(approveNew);
                    }
                    if (!approvalDao || resultNew == false)
                    {
                        check = false;
                    }
                }
            }
            if (check == true)
            {
                Notification("Success", "Approval request successfully", MyConstants.NOTIFY_SUCCESS);
                TempData["ApprovalTotalL1"] = new Taxi_ApprovalDao().GetTotalApproval(mEmployee.employee.employee_id, 1);
                TempData["ApprovalTotalL2"] = new Taxi_ApprovalDao().GetTotalApproval(mEmployee.employee.employee_id, 2);
            }
            else
            {
                Notification("False", "Approval request failure, error: ", MyConstants.NOTIFY_ERROR);
            }
            return RedirectToAction("Approval");
        }
        [HttpPost]
        public ActionResult Confirm()
        {
            return View();
        }
    }
}