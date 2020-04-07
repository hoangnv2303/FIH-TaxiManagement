using EquipmentRegistation.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Visitor_Registration_Data.Dao;
using Visitor_Registration_Data.EF;
using Visitor_Registration_Data.Model;
using VisitorRegistration.Helper;
using VisitorRegistration.Models;
using VisitorRegistation.Common;
using Visitor_Registration.Models;

namespace Visitor_Registration.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected UserModel mEmployee = null;
        protected ADWebHelper adWebHelper = new ADWebHelper();
        protected static List<DepartmentModel> department;

        // Get path
        protected static string DepartmentLevel2 = ConfigurationManager.AppSettings["DepartmentLevel2"];
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (Request.Cookies["user_cookie"] != null)
                {
                    var temp = Request.Cookies["user_cookie"].Value;
                    mEmployee = JsonConvert.DeserializeObject<UserModel>(temp);
                }
            }
            catch
            {
                mEmployee = null;
            }
            if (mEmployee == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                return;
            }
            else
            {
                if (adWebHelper.getUserInfo(mEmployee.employee.access_token) != null)
                {
                    // Set values from user here
                    ViewBag.User = mEmployee.employee.display_name;
                    var employeeInfor = adWebHelper.GetDetailUserInfo(mEmployee.employee.access_token, mEmployee.employee.employee_id);
                    //var sml = adWebHelper.GetDetailUserInfoByID(mEmployee.employee.access_token, Convert.ToInt32(employeeInfor.parent_id[0].ToString())).ad_user_displayName;
                    //var headInfor = adWebHelper.GetHeadOfFunction(mEmployee.employee.access_token, employeeInfor.department).ad_user_displayName;

                    // send mail nao
                    //adWebHelper.SendMail(mEmployee.employee.access_token);

                    // get department
                    department = adWebHelper.GetDepartments(mEmployee.employee.access_token);


                    if (Session[CommonConstants.USER_SESSION] == null)
                    {
                        UpdateUserInformation(employeeInfor);
                    }
                    Session[CommonConstants.USER_SESSION] = employeeInfor;
                    Session[CommonConstants.SESSION_CREDENTIALS] = new UserDao().GetListCredential(employeeInfor.ad_user_employeeID);
                    TempData["ApprovalTotalL1"] = new Taxi_ApprovalDao().GetTotalApproval(mEmployee.employee.employee_id, 1);
                    TempData["ApprovalTotalL2"] = new Taxi_ApprovalDao().GetTotalApproval(mEmployee.employee.employee_id, 2);
                    TempData["Pesmission"] = new UserDao().GetListCredential(employeeInfor.ad_user_employeeID);

                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                    return;
                }
            }
        }

        protected void Notification(string title, string content, string type)
        {
            TempData["notify-title"] = title;
            TempData["notify-content"] = content;
            if (type.Equals(MyConstants.NOTIFY_SUCCESS))
                TempData["notify-type"] = "success";
            else if (type.Equals(MyConstants.NOTIFY_INFO))
                TempData["notify-type"] = "info";
            else if (type.Equals(MyConstants.NOTIFY_NOTICE))
                TempData["notify-type"] = "notice";
            else if (type.Equals(MyConstants.NOTIFY_ERROR))
                TempData["notify-type"] = "error";
        }
        public static Dictionary<int, string> GetDepartMent()
        {
            Dictionary<int, string> dict = department.ToDictionary(x => x.id, x => x.name);
            //Dictionary<int, string> dict = new Dictionary<int, string>();
            //dict.Add(1, "Administration");
            //dict.Add(2, "Assembly Operations");
            //dict.Add(3, "Asset Management & Service Support");
            //dict.Add(4, "Business Control");
            //dict.Add(5, "Engineering");
            //dict.Add(6, "Fulfillment & Inventory Control");
            //dict.Add(7, "General Affairs");
            //dict.Add(8, "Human Resources");
            //dict.Add(9, "Information Technology ");
            //dict.Add(10, "Operational Excellence");
            //dict.Add(11, "Planning and Execution");
            //dict.Add(12, "Quality");
            //dict.Add(13, "Real Estate & Facilities ");
            //dict.Add(14, "Trade Compliance & Shipping");
            //dict.Add(15, "Accounting");
            //dict.Add(16, "Security");
            //dict.Add(17, "Ensky");
            //dict.Add(18, "IDM1");
            //dict.Add(19, "Research & Development");
            //dict.Add(20, "General Affairs & Security");
            return dict;
        }
        #region Update user-permission information from AD to database
        public void UpdateUserInformation(EmployeeModel employeeInfor)
        {
            //Get employeeInfor
            //var employeeInfor = (EmployeeModel)Session[CommonConstants.USER_SESSION];
            var headInfor = adWebHelper.GetHeadOfFunction(mEmployee.employee.access_token, employeeInfor.department);

            // User: Insert or update permisstion
            var employeeId = InsertOrUpdateUser(employeeInfor);
            GrantPermisstion(employeeId, 2); // Default permisstion

            // Department: Insert or update with headId
            var headId = InsertOrUpdateUser(headInfor);
            GrantPermisstion(headId, 3); // Default permisstion
            tbl_Taxi_Department_Infor department = new tbl_Taxi_Department_Infor();
            department.DepartmentName = employeeInfor.department;
            department.Head_id = headId;
            department.UpdateDate = DateTime.Now;
            new DepartmentDao().InsertOrUpdateDepartment(department);
        }

        public string InsertOrUpdateUser(EmployeeModel employeeInfor)
        {
            tbl_User user = new tbl_User();
            user.EmployeeId = employeeInfor.ad_user_employeeID;
            user.FushanAd = employeeInfor.ad_user_sAMAccountName;
            user.FullName = employeeInfor.ad_user_displayName;
            user.Email = employeeInfor.work_email;
            user.JobTitle = employeeInfor.job_title;
            user.DeparmentName = employeeInfor.department;
            var userResult = new UserDao().InsertOrUpdateUser(user);
            return userResult;
        }
        public bool GrantPermisstion(string EmployeeId, int roleId)
        {
            tbl_User_Role userRole = new tbl_User_Role();
            userRole.EmployeeId = EmployeeId;
            userRole.Role_Id = roleId;
            userRole.CreateDate = DateTime.Now;
            var result = new UserDao().InsertUserRole(userRole);
            return result;
        }
        #endregion

        public static CSVModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            CSVModel csv = new CSVModel();
            csv.VisitorName = values[0];
            csv.NationalId = values[1];
            csv.CompanyName = values[2];
            csv.Remark = values[3];
            return csv;
        }
    }
}