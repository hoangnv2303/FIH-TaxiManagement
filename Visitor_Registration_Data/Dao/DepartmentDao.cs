using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor_Registration_Data.EF;
using Common;

namespace Visitor_Registration_Data.Dao
{
    public class DepartmentDao
    {
        VisitorRegistration_Model db = null;
        public DepartmentDao()
        {
            db = new VisitorRegistration_Model();
        }

        public bool InsertOrUpdateDepartment(tbl_Taxi_Department_Infor department)
        {
            try
            {
                var departmentFind = db.tbl_Taxi_Department_Infor.FirstOrDefault(x => x.DepartmentName == department.DepartmentName);
                if (departmentFind == null)
                {
                    db.tbl_Taxi_Department_Infor.Add(department);
                }
                else
                {
                    departmentFind.Head_id = department.Head_id;
                    departmentFind.UpdateDate = department.UpdateDate;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetListDeputyByHeadId(string headId)
        {
            try
            {
                return db.tbl_Taxi_Department_Infor.FirstOrDefault(x => x.Head_id == headId).Deputy_id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetListDeputyByDepartment(string departmentName)
        {
            try
            {
                return db.tbl_Taxi_Department_Infor.FirstOrDefault(x => x.DepartmentName == departmentName).Deputy_id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetHeadIdByDepartment(string departmentName)
        {
            try
            {
                var result = (from a in db.tbl_Taxi_Department_Infor
                              join b in db.tbl_User on a.Head_id equals b.EmployeeId
                              where a.DepartmentName == departmentName
                              select new { Name = b.FullName }).FirstOrDefault().Name;
                return result;
                    //db.tbl_Department_Infor.FirstOrDefault(x => x.DepartmentName == departmentName).Head_id;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public string GetApproverName(string departmentName)
        {
            try
            {
                var department = db.tbl_Taxi_Department_Infor.FirstOrDefault(x => x.DepartmentName == departmentName);
                if (department.Deputy_id == null || department.Deputy_id == "")
                {
                    return department.Head_id;
                }
                else
                {
                    return department.Deputy_id;
                }
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetApproverName", ex.ToString());
                return "false";
            }
        }
        public bool UpdateDeputy(string department, string listDeputy)
        {
            try
            {
                var checkDeputy = db.tbl_Taxi_Department_Infor.FirstOrDefault(x => x.DepartmentName == department);
                checkDeputy.Deputy_id = listDeputy;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("UpdateDeputy", ex.ToString());
                return false;
            }        
        }


        public bool CheckApprovalPermisstion(string employeeId)
        {
            try
            {
                var check = db.tbl_Taxi_Department_Infor.FirstOrDefault(x => x.Head_id == employeeId || x.Deputy_id.Contains(employeeId));
                if (check != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("CheckApprovalPermisstion", ex.ToString());
                return false;
            }
        }
    }
}
