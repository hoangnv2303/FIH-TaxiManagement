using Common;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Visitor_Registration.Models;
using Visitor_Registration_Data.Dao;
using Visitor_Registration_Data.EF;
using Visitor_Registration_Data.Model;
using VisitorRegistation.Common;
using VisitorRegistration.Common;
using VisitorRegistration.Helper;

namespace Visitor_Registration.Controllers
{
    public class RequestController : BaseController
    {
        // GET: Request
        [DenyHasCredential(RoleID = "NO_CREATOR")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CreateRequest()
        {
            // Create GUIID
            Guid idRequest = Guid.NewGuid();

            CreateRequestModel newRequest = new CreateRequestModel();
            newRequest.Id = idRequest;
            newRequest.EmployeeId = mEmployee.employee.employee_id;
            var employeeInfor = adWebHelper.GetDetailUserInfo(mEmployee.employee.access_token, mEmployee.employee.employee_id);
            newRequest.SLM = adWebHelper.GetDetailUserInfoByID(mEmployee.employee.access_token, Convert.ToInt32(employeeInfor.parent_id[0].ToString())).ad_user_displayName;
            newRequest.FullName = mEmployee.employee.display_name;
            // Approver Information
            List<tbl_User> listApproval = null;
            var listApprov = new DepartmentDao().GetApproverName(mEmployee.employee.departmentName);
            if (listApprov != null && listApprov != "")
            {
                var listUser = listApprov.Split(';').ToList();
                listApproval = new UserDao().GetListUser(listUser);
            }
            ViewBag.ApproverName = string.Join("; ", listApproval.Select(x => x.FullName.ToString()).ToArray());

            // Information the requester

            return View(newRequest);
        }

        [HttpPost]
        [DenyHasCredential(RoleID = "NO_CREATOR")]
        public ActionResult CreateRequest(CreateRequestModel request)
        {
            try
            {
                // Insert request infor
                tbl_Taxi_Request_Infor taxiInfor = new tbl_Taxi_Request_Infor();
                taxiInfor.Id = request.Id;
                taxiInfor.EmployeeId = request.EmployeeId;
                taxiInfor.Purpose = request.Purpose;
                taxiInfor.ScheduleTime = request.ScheduleTime;
                taxiInfor.Head = adWebHelper.GetHeadOfFunction(mEmployee.employee.access_token, mEmployee.employee.departmentName).ad_user_displayName;
                taxiInfor.Pickup = request.Pickup;
                taxiInfor.DropOff1 = request.DropOff1;
                taxiInfor.DropOff2 = request.DropOff2;
                taxiInfor.CreateDate = DateTime.Now;
                taxiInfor.Remark = request.Remark;

                var resultRequest = new Taxi_RequestDao().InsertOrUpdateRequest(taxiInfor);
                // Insert personal infor
                var checkInsertDetail = true;

                if (request.ListVisitorCSV != "" && request.ListVisitorCSV != null)
                {
                    System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\TempCSV.csv", request.ListVisitorCSV, System.Text.Encoding.UTF8);
                    List<CSVModel> values = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\TempCSV.csv")
                                               .Skip(1)
                                               .Select(v => FromCsv(v))
                                               .ToList();
                    foreach (var item in values)
                    {
                        if (checkInsertDetail == true)
                        {
                            tbl_Taxi_User_Infor user = new tbl_Taxi_User_Infor();
                            user.Name = item.VisitorName;
                            user.EmployeeId = item.NationalId;
                            user.SLM_Name = item.CompanyName;
                            user.Taxi_Request_Infor_Id = request.Id;
                            user.Remark = item.Remark;

                            user.Taxi_Request_Infor_Id = request.Id;
                            // Two record if register dropoff 2
                            if (request.DropOff2 != "" && request.DropOff2 != null)
                            {
                                user.Process = 1;
                                checkInsertDetail = new Taxi_UserDao().InsertOrUpdateUser(user);
                                user.Process = 2;
                            }
                            checkInsertDetail = new Taxi_UserDao().InsertOrUpdateUser(user);
                        }
                    }
                }
                else
                {
                    foreach (var item in request.Visitor)
                    {
                        if (checkInsertDetail == true)
                        {
                            item.Taxi_Request_Infor_Id = request.Id;
                            // Two record if register dropoff 2
                            if (request.DropOff2 != "" && request.DropOff2 != null)
                            {
                                item.Process = 1;
                                checkInsertDetail = new Taxi_UserDao().InsertOrUpdateUser(item);
                                item.Process = 2;
                            }
                            checkInsertDetail = new Taxi_UserDao().InsertOrUpdateUser(item);
                        }
                    }
                }

                // Insert approval infor
                tbl_Taxi_Approval_Infor approval = new tbl_Taxi_Approval_Infor();
                approval.Request_Id = (Guid)request.Id;
                approval.DepartmentName = mEmployee.employee.departmentName;
                approval.Process = 1;
                approval.State = 0;
                approval.UpdateDate = DateTime.Now;

                var resultApproval = new Taxi_ApprovalDao().InsertOrUpdateApproval(approval);
                if (resultRequest == true && resultApproval == true && checkInsertDetail == true)
                {
                    Notification("Success", "Create request successfully", MyConstants.NOTIFY_SUCCESS);
                    return RedirectToAction("Index", "Home", new { type = 0 });
                }
                else
                {
                    //Rollback - delete when failure
                    var approvalDeleteResult = new Taxi_ApprovalDao().RemoveApprovalByRequestId(request.Id);
                    var userDeleteResult = new Taxi_UserDao().RemoveUserByRequestId(request.Id);
                    var requestDeleteResult = new Taxi_RequestDao().RemoveRequest(request.Id);
                    WriteLogError.Write("Insert Request Failure: ", "Request: " + resultRequest + " Visitor: " + checkInsertDetail + " Approval: " + resultApproval);
                    WriteLogError.Write("Rollback Request Failure: ", "Request: " + requestDeleteResult + " Visitor: " + userDeleteResult + " Approval: " + approvalDeleteResult);
                    Notification("False", "Create request failure, Please recheck all information!", MyConstants.NOTIFY_ERROR);
                }
            }
            catch (Exception ex)
            {
                Notification("False", "Create request failure, error: " + ex.ToString(), MyConstants.NOTIFY_ERROR);
            }
            return RedirectToAction("CreateRequest", "Request");
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult RequestDetail(Guid id, string nationalId)
        {
            ViewBag.RequestInfor = new Taxi_RequestDao().GetListRequestById(id, nationalId);
            ViewBag.NationalId = nationalId;
            ViewBag.ApprovalInfor = new Taxi_ApprovalDao().GetApprovalById(id);
            ViewBag.ApprovalPermistion = new Taxi_ApprovalDao().CheckPermissionApproval(id, mEmployee.employee.employee_id);
            return View();
        }

        /// <summary>
        /// For update information of request (Reception)
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RequestDetail(List<tbl_Taxi_User_Infor> visitor)
        {
            // Update here
            try
            {
                foreach (var item in visitor)
                {
                    tbl_Taxi_User_Infor vis = new tbl_Taxi_User_Infor();
                    vis.Taxi_Request_Infor_Id = item.Taxi_Request_Infor_Id;
                    vis.Name = item.Name;
                    vis.EmployeeId = item.EmployeeId;
                    vis.Process = item.Process;
                    vis.Remark = item.Remark;
                    vis.SwipeTime = (item.SwipeTime.GetValueOrDefault().Hour == 0) ? null : item.SwipeTime;
                    vis.CardNumber = item.CardNumber;

                    if (item.CarNumber == null && item.Cost == null)
                    {
                        if (item.CardNumber != null && item.CreateBy == null)
                        {
                            vis.CreateDate = DateTime.Now;
                            vis.CreateBy = mEmployee.employee.employee_id;
                        }
                    }
                    else
                    {
                        vis.CarNumber = item.CarNumber;
                        vis.Cost = item.Cost;
                        vis.RefNumber = item.RefNumber;
                        vis.UpdateDate = DateTime.Now;
                        vis.UpdateBy = mEmployee.employee.employee_id;
                    }
                    var result = new Taxi_UserDao().InsertOrUpdateUser(vis);
                }
                Notification("Success", "Update visitor successfully", MyConstants.NOTIFY_SUCCESS);
            }
            catch (Exception ex)
            {
                Notification("False", "Update visitor failure, error: " + ex.ToString(), MyConstants.NOTIFY_ERROR);
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [DenyHasCredential(RoleID = "NO_CREATOR")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult MyRequest()
        {
            ViewBag.ListRequest = new Taxi_RequestDao().GetListMyRequest(mEmployee.employee.employee_id);
            if (((List<string>)(Session[CommonConstants.SESSION_CREDENTIALS])).Contains("DELETE_REQUEST"))
            {
                ViewBag.Delete = "Delete";
            }
            return View();
        }

        public JsonResult DeleteRequest(Guid id, string listReceptions, string reason)
        {
            if (listReceptions != null && listReceptions != "")
            {
                // send mail
                new Taxi_RequestDao().SendMailRemove(id, listReceptions, reason, mEmployee.employee.display_name);
            }
            // check and delete permisstion
            var approvalResult = new Taxi_ApprovalDao().RemoveApprovalByRequestId(id);
            var visitorResult = new Taxi_UserDao().RemoveUserByRequestId(id);
            var requestResult = new Taxi_RequestDao().RemoveRequest(id);
            if (approvalResult && visitorResult && requestResult) // if not exists in deputy list or head => delete permisstion approval
                return Json(new { status = true });
            else
                return Json(new { status = false });
        }

        public JsonResult DeleteListRequest(List<Guid> listRequest)
        {
            bool check = true;
            foreach (var item in listRequest)
            {
                // check and delete permisstion
                var approvalResult = new Taxi_ApprovalDao().RemoveApprovalByRequestId(item);
                var visitorResult = new Taxi_UserDao().RemoveUserByRequestId(item);
                var requestResult = new Taxi_RequestDao().RemoveRequest(item);
                if (!approvalResult || !visitorResult || !requestResult)
                {
                    check = false;
                }
            }
            if (check) // if not exists in deputy list or head => delete permisstion approval
                return Json(new { status = true });
            else
                return Json(new { status = false });
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult UpdateRequest(Guid id)
        {
            var updateRequest = new Taxi_RequestDao().GetListRequestById(id, "1").FindAll(x => x.Process != 2);
            return View(updateRequest);
        }
        [HttpPost]
        public ActionResult UpdateRequest(List<RequestInforModel> listRequests)
        {
            try
            {
                bool resultVisitor = true;
                // update request information
                tbl_Taxi_Request_Infor request = new tbl_Taxi_Request_Infor();
                request.Id = listRequests[0].Id;
                request.Purpose = listRequests[0].Purpose;
                request.ScheduleTime = listRequests[0].ScheduleTime;
                request.Pickup = listRequests[0].Pickup;
                request.DropOff1 = listRequests[0].DropOff1;
                request.DropOff2 = listRequests[0].DropOff2;
                request.Remark = listRequests[0].Remark;
                var resultRequest = new Taxi_RequestDao().InsertOrUpdateRequest(request);
                // update visitor information
                foreach (var item in listRequests)
                {
                    tbl_Taxi_User_Infor vis = new tbl_Taxi_User_Infor();
                    vis.Taxi_Request_Infor_Id = listRequests[0].Id;
                    vis.Name = item.Name;
                    vis.EmployeeId = item.User_EmployeeId;
                    vis.SLM_Name = item.SLM_Name;
                    vis.Remark = item.User_Remark;
                    vis.UpdateBy = mEmployee.employee.employee_id;
                    //if (request.DropOff2 != "" && request.DropOff2 != null)
                    //{

                    //}
                    resultVisitor = new Taxi_UserDao().UpdateBaseInformation(vis, item.EmployeeId_Old, request.DropOff2);
                }
                if (resultRequest && resultVisitor)
                {
                    Notification("Success", "Update visitor successfully", MyConstants.NOTIFY_SUCCESS);
                }
                else
                {
                    Notification("False", "Update visitor failure, Please recheck information!", MyConstants.NOTIFY_ERROR);
                }
            }
            catch (Exception ex)
            {
                Notification("False", "Update visitor failure, error: " + ex.ToString(), MyConstants.NOTIFY_ERROR);
            }
            return RedirectToAction("MyRequest", "Request");
        }

        public JsonResult GetListReception()
        {
            List<tbl_User> listApproval = null;
            var listApprov = new DepartmentDao().GetApproverName(mEmployee.employee.departmentName);
            if (listApprov != null && listApprov != "")
            {
                var listUser = listApprov.Split(';').ToList();
                listApproval = new UserDao().GetListUser(listUser);
            }
            var listEmail = string.Join("; ", listApproval.Select(x => x.Email.ToString()).ToArray());

            // check and delete permisstion
            if (listEmail != null && listEmail != "")
                return Json(new { result = listEmail });
            else
                return Json(new { result = false });
        }
    }
}



