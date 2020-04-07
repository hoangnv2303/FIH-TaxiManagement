using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Visitor_Registration_Data.Dao;
using Visitor_Registration_Data.EF;
using Visitor_Registration_Data.Model;

namespace Visitor_Registration.Controllers
{
    public class VisitorController : BaseController
    {
        // GET: Visitor
        public JsonResult AddNewVisitor(Guid requestId, string name, string id, string company, string remark)
        {
            var newVisitor = new tbl_Taxi_User_Infor();
            newVisitor.Taxi_Request_Infor_Id = requestId;
            newVisitor.Process = 0;
            newVisitor.Name = name;
            newVisitor.EmployeeId = id;
            newVisitor.SLM_Name = company;
            newVisitor.Remark = remark;
            newVisitor.UpdateBy = mEmployee.employee.employee_id;
            newVisitor.UpdateDate = DateTime.Now;

            var insertResult = new Taxi_UserDao().InsertOrUpdateUser(newVisitor);
            if (insertResult)
                return Json(new { result = true });
            else
                return Json(new { result = false });
        }

        public JsonResult RemoveVisitor(Guid id, string nationalId)
        {
            var removeResult = new Taxi_UserDao().RemoveUserByEmployeeId(id, nationalId);
            if (removeResult)
                return Json(new { result = true });
            else
                return Json(new { result = false });
        }


        public JsonResult CheckEscorter(string employeeId)
        {
            // check and delete permisstion
            var checkUser = adWebHelper.GetDetailUserInfo(mEmployee.employee.access_token, employeeId.Trim());
            if (checkUser != null)
            {
                // Insert user to database
                var resultInsert = InsertOrUpdateUser(checkUser);
                var resultPermisstion = GrantPermisstion(checkUser.ad_user_employeeID, 2);
                if (resultInsert != null && resultPermisstion)
                {
                    return Json(new { result = "true" });
                }
            }
            return Json(new { result = employeeId });
        }

        public JsonResult CheckNationalId(string nationalId)
        {
            // check and delete permisstion
            var blackList = new UserDao().GetBlackList();
            if (blackList.Select(x => x.NationalId).ToList().Contains(nationalId))
                return Json(new { status = nationalId });
            else
                return Json(new { status = "true" });
        }

        public JsonResult AddBlacklists(string list)
        {
            var checkInsertDetail = "";
            if (list != "" && list != null)
            {
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\TempCSV.csv", list, System.Text.Encoding.UTF8);
                List<CSVModel> values = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\TempCSV.csv")
                                           .Skip(1)
                                           .Select(v => FromCsv(v))
                                           .ToList();
                foreach (var item in values)
                {
                    tbl_BlackList visitor = new tbl_BlackList();
                    visitor.VisitorName = item.VisitorName;
                    visitor.NationalId = item.CompanyName;
                    visitor.CompanyName = item.NationalId;
                    visitor.Reason = item.Remark;
                    visitor.CreateBy = mEmployee.employee.employee_id;
                    visitor.CreateDate = DateTime.Now;
                    if (!new UserDao().InsertOrUpdateBlackList(visitor))
                    {
                        checkInsertDetail = checkInsertDetail + "Visitor: " + item.VisitorName + " with national Id: " + item.CompanyName + "; ";
                    } 
                }
            }
            if (checkInsertDetail == "")
                return Json(new { status = true });
            else
                return Json(new { status = checkInsertDetail });
        }

    }
}