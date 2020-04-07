using EquipmentRegistation.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Visitor_Registration_Data.Dao;
using Visitor_Registration_Data.EF;
using VisitorRegistration.Common;

namespace Visitor_Registration.Controllers
{
    public class SettingController : BaseController
    {
        // GET: Setting
        [HttpGet]
        [HasCredential(RoleID = "DEPUTY")]
        public ActionResult Index(string Department)
        {
            List<tbl_User> listDeputy = null;
            var deputyList = "";
            // For admin config
            SelectList groupList = new SelectList((IEnumerable)GetDepartMent(), "Value", "Value");
            ViewBag.GroupList = groupList;

            if (Department == null || Department == "")
            {
                Department = mEmployee.employee.departmentName;
            }

            ViewBag.HeadName = new DepartmentDao().GetHeadIdByDepartment(Department);
            ViewBag.Department = Department;
            deputyList = new DepartmentDao().GetListDeputyByDepartment(Department);
            // add to user
            // add list deputy
            if (deputyList != null && deputyList != "")
            {
                var listUser = deputyList.Split(';').ToList();
                listDeputy = new UserDao().GetListUser(listUser);
            }
            ViewBag.ListDeputy = listDeputy;
            return View();
        }

        public JsonResult AddUser(string userId, string dep)
        {
            if (dep == null || dep == "")
            {
                dep = mEmployee.employee.departmentName;
            }
            var checkUser = adWebHelper.GetDetailUserInfo(mEmployee.employee.access_token, userId.Trim());
            if (checkUser != null)
            {
                // Insert user to database
                InsertOrUpdateUser(checkUser);
                GrantPermisstion(checkUser.ad_user_employeeID, 3);

                // Insert new user to deputy list
                var listDepartment = new DepartmentDao().GetListDeputyByDepartment(dep);
                if (listDepartment == null || listDepartment == "")
                {
                    listDepartment = userId;
                }
                else if (!listDepartment.Contains(userId))
                {
                    listDepartment = listDepartment + ";" + userId;
                }
                var updateResult = new DepartmentDao().UpdateDeputy(dep, listDepartment);
                if (updateResult)
                {
                    return Json(new { result = true });
                }
            }
            return Json(new { result = false });
        }
        public JsonResult Delete(string listDeputy, string id, string dep)
        {
            if (dep == null || dep == "")
            {
                dep = mEmployee.employee.departmentName;
            }

            var updateDeputy = new DepartmentDao().UpdateDeputy(dep, listDeputy);
            // check and delete permisstion
            var check = new DepartmentDao().CheckApprovalPermisstion(id);
            var result = true;
            if (!check) // if not exists in deputy list or head => delete permisstion approval
            {
                result = new UserDao().DeleteUserRole(id, 3);
            }
            if (updateDeputy && result)
                return Json(new { status = true });
            else
                return Json(new { status = false });
        }


        [HttpGet]
        [HasCredential(RoleID = "CHANGE_PERMISSTION")]
        public ActionResult ShowUser()
        {
            var selectList = new SelectList(
                                new List<SelectListItem>
                                {
                                    new SelectListItem {Text = "Admin", Value = "1"},
                                    new SelectListItem {Text = "Host", Value = "2"},
                                    new SelectListItem {Text = "Receptionist", Value = "7"},
                                    new SelectListItem {Text = "Security", Value = "5"},
                                }, "Value", "Text");
            
            ViewBag.GroupList = selectList;

            ViewBag.ListUser = new UserDao().GetListUserInfor();
            return View();
        }

        public JsonResult UpdatePermission(string employeeId, int role)
        {
            var updateResult = GrantPermisstion(employeeId, role);
            if (updateResult)
                return Json(new { status = true });
            else
                return Json(new { status = false });
        }

        public JsonResult DeletePermission(string employeeId, int role)
        {
            var deleteResult = new UserDao().DeleteUserRole(employeeId, role);
            if (deleteResult)
                return Json(new { status = true });
            else
                return Json(new { status = false });
        }

        [HttpGet]
        [HasCredential(RoleID = "CHANGE_PERMISSTION")]
        public ActionResult BlackList()
        {
            ViewBag.BlackList = new UserDao().GetBlackList();
            return View();
        }

        public JsonResult BlackListDelete(string nationalId)
        {
            // check and delete permisstion
            var resultDelete = new UserDao().DeleteBlackList(nationalId);
            if (resultDelete)
                return Json(new { status = true });
            else
                return Json(new { status = false });
        }

        public JsonResult BlackListInsertOrUpdate(string visitor, string company, string nationalId, string reason)
        {
            // check and delete permisstion
            tbl_BlackList newBlackList = new tbl_BlackList();
            newBlackList.VisitorName = visitor;
            newBlackList.CompanyName = company;
            newBlackList.NationalId = nationalId;
            newBlackList.Reason = reason;
            newBlackList.CreateBy = mEmployee.employee.employee_id;
            newBlackList.CreateDate = DateTime.Now;
            var resultUpdate = new UserDao().InsertOrUpdateBlackList(newBlackList);
            if (resultUpdate)
                return Json(new { status = true });
            else
                return Json(new { status = false });
        }

    }
}