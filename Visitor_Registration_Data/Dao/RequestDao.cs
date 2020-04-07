using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor_Registration_Data.EF;
using Visitor_Registration_Data.Model;
using Common;
namespace Visitor_Registration_Data.Dao
{
    public class RequestDao
    {
        VisitorRegistration_Model db = null;
        public RequestDao()
        {
            db = new VisitorRegistration_Model();
        }

        public bool UpdateRequestInfor(tbl_Request_Infor request)
        {
            try
            {
                var requestOld = db.tbl_Request_Infor.FirstOrDefault(x => x.Id == request.Id);
                if (requestOld != null)
                {
                    requestOld.IncommingDate = request.IncommingDate;
                    requestOld.OutgoingDate = request.OutgoingDate;
                    requestOld.PurposeVisit = request.PurposeVisit;
                    requestOld.Remark = request.Remark;
                    requestOld.Type = request.Type;
                    requestOld.CreatedDate = request.CreatedDate;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool InsertRequestInfor(tbl_Request_Infor request)
        {
            try
            {
                if (CheckExistsRequest(request))
                {
                    db.tbl_Request_Infor.Add(request);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteLogError.Write("InsertRequestInfor", ex.ToString());
                return false;
            }
        }

        public bool CheckExistsRequest(tbl_Request_Infor request)
        {
            try
            {
                var result = db.tbl_Request_Infor.FirstOrDefault(x => x.EmployeeId == request.EmployeeId
                && x.PurposeVisit.Trim().ToLower() == request.PurposeVisit.Trim().ToLower()
                && x.IncommingDate == request.IncommingDate && x.OutgoingDate == request.OutgoingDate);
                if (result == null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("CheckExistsRequest", ex.ToString());
                return false;
            }
        }

        public List<RequestInforModel> GetListRequests(int type, string employeeId)
        {
            try
            {
                var result = db.Database.SqlQuery<RequestInforModel>("p_showRequestByType @type, @employeeId",
                    new SqlParameter("@type", type),
                    new SqlParameter("@employeeId", employeeId)
                ).ToList();
                return result;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetListRequests", ex.ToString());
                return null;
            }

        }

        public List<RequestInforModel> GetListRequestById(Guid id, string nationalId)
        {
            try
            {
                var result = db.Database.SqlQuery<RequestInforModel>("p_showRequestById @id, @nationalId",
                    new SqlParameter("@id", id),
                    new SqlParameter("@nationalId", nationalId)
                ).ToList();
                return result;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetListRequestById", ex.ToString());
                return null;
            }
        }
        // Report
        public List<ReportModel> GetListReportDaily()
        {
            try
            {
                var output = db.Database.SqlQuery<ReportModel>("p_getReport").ToList();
                return output;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetListReportDaily", ex.ToString());
                return null;
            }

        }
        // Get list my request
        public List<MyRequestModel> GetListMyRequest(string employeeId)
        {
            try
            {
                var result = db.Database.SqlQuery<MyRequestModel>("p_getMyRequest @employeeId",
                      new SqlParameter("@employeeId", employeeId)
                  ).ToList();
                return result;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetListMyRequest", ex.ToString());
                return null;
            }

        }

        public bool SendMailRemove(Guid id, string listReceptions, string reason, string employee)
        {
            try
            {
                var result = db.Database.SqlQuery<string>("p_SendMailDelete @id, @listReceptions, @reason, @employeeName",
                    new SqlParameter("@id", id),
                    new SqlParameter("@listReceptions", listReceptions),
                    new SqlParameter("@reason", reason),
                    new SqlParameter("@employeeName", employee)).FirstOrDefault();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveRequest(Guid id)
        {
            try
            {
                var request = db.tbl_Request_Infor.Single(x => x.Id == id);
                db.tbl_Request_Infor.Remove(request);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("RemoveRequest", ex.ToString());
                return false;
            }
        }
    }
}
