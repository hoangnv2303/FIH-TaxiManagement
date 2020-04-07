using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor_Registration_Data.EF;
using Common;
using System.Data.SqlClient;
using Visitor_Registration_Data.Model;

namespace Visitor_Registration_Data.Dao
{
    public class ApprovalDao
    {
        VisitorRegistration_Model db = null;
        public ApprovalDao()
        {
            db = new VisitorRegistration_Model();
        }

        public List<RequestInforModel> GetListApproval(string employeeId, int process)
        {
            var result = db.Database.SqlQuery<RequestInforModel>("p_showApproval @employeeId, @process",
                    new SqlParameter("@employeeId", employeeId),
                    new SqlParameter("@process", process)
                ).ToList();
            return result;
        }

        public int GetTotalApproval(string emp, int process)
        {
            try
            {
                var result = db.Database.SqlQuery<int>("p_getTotalApproval @employeeId, @process",
                   new SqlParameter("@employeeId", emp),
                   new SqlParameter("@process", process)
                ).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetTotalApproval", ex.ToString());
                return -1;
            }
        }

        public bool InsertOrUpdateApproval(tbl_Taxi_Approval_Infor approval)
        {
            try
            {
                var approveCheck = db.tbl_Taxi_Approval_Infor.FirstOrDefault(x => x.Request_Id == approval.Request_Id && x.Process == approval.Process);
                if (approveCheck == null)
                {
                    db.tbl_Taxi_Approval_Infor.Add(approval);
                }
                else
                {
                    approveCheck = approval;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("InsertOrUpdateApproval", ex.ToString());
                return false;
            }
        }

        public List<tbl_Taxi_Approval_Infor> GetApprovalById(Guid id)
        {
            try
            {
                return db.tbl_Taxi_Approval_Infor.Where(x => x.Request_Id == id).ToList();
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetApprovalById", ex.ToString());
                return null;
            }
        }

        public bool CheckPermissionApproval(Guid id, string employeeId)
        {
            try
            {
                var result = (from a in db.tbl_Taxi_Approval_Infor
                              join b in db.tbl_Taxi_Department_Infor on a.DepartmentName equals b.DepartmentName
                              where a.Request_Id == id && (b.Head_id.Contains(employeeId) || b.Deputy_id.Contains(employeeId)) && a.State == 0
                              select new { Name = a.DepartmentName }).FirstOrDefault().Name;
                if (result != null || result != "")
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                // if not permisstion auto return false
                return false;
            }
        }

        public bool RemoveApprovalByRequestId(Guid id)
        {
            try
            {
                var approval = db.tbl_Taxi_Approval_Infor.Where(x => x.Request_Id == id);
                db.tbl_Taxi_Approval_Infor.RemoveRange(approval);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("RemoveApprovalByRequestId", ex.ToString());
                return false;
            }
        }

    }
}
