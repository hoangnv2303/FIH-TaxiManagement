using Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor_Registration_Data.EF;
using Visitor_Registration_Data.Model;

namespace Visitor_Registration_Data.Dao
{
    public class Taxi_RequestDao
    {
        VisitorRegistration_Model db = null;
        public Taxi_RequestDao()
        {
            db = new VisitorRegistration_Model();
        }

        public bool InsertOrUpdateRequest(tbl_Taxi_Request_Infor request)
        {
            try
            {
                var checkRequest = db.tbl_Taxi_Request_Infor.FirstOrDefault(x => x.Id == request.Id);
                if (checkRequest != null)
                {
                    checkRequest.ScheduleTime = request.ScheduleTime;
                    checkRequest.Purpose = request.Purpose;
                    checkRequest.Pickup = request.Pickup;
                    checkRequest.DropOff1 = request.DropOff1;
                    checkRequest.DropOff2 = request.DropOff2;
                    checkRequest.Remark = request.Remark;
                }
                else
                {
                    db.tbl_Taxi_Request_Infor.Add(request);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("CreateRequest", ex.ToString());
                return false;
            }
        }


        public bool RemoveRequest(Guid id)
        {
            try
            {
                var request = db.tbl_Taxi_Request_Infor.Single(x => x.Id == id);
                db.tbl_Taxi_Request_Infor.Remove(request);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("RemoveRequest", ex.ToString());
                return false;
            }
        }


        // Get list my request
        public List<MyRequestModel> GetListMyRequest(string employeeId)
        {
            try
            {
                var result = db.Database.SqlQuery<MyRequestModel>("Taxi_p_getMyRequest @employeeId",
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

        // Get list request by id
        public List<RequestInforModel> GetListRequestById(Guid id, string employeeId)
        {
            try
            {
                var result = db.Database.SqlQuery<RequestInforModel>("Taxi_p_showRequestById @id, @employeeId",
                    new SqlParameter("@id", id),
                    new SqlParameter("@employeeId", employeeId)
                ).ToList();
                return result;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetListRequestById", ex.ToString());
                return null;
            }
        }
        public bool SendMailRemove(Guid id, string listReceptions, string reason, string employee)
        {
            try
            {
                var result = db.Database.SqlQuery<string>("Taxi_p_SendMailDelete @id, @listReceptions, @reason, @employeeName",
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

        public List<RequestInforModel> GetListRequests(int type, string employeeId)
        {
            try
            {
                var result = db.Database.SqlQuery<RequestInforModel>("Taxi_p_showRequestByType @type, @employeeId",
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
        public List<ReportModel> GetListReportDaily()
        {
            try
            {
                var output = db.Database.SqlQuery<ReportModel>("Taxi_p_getReport").ToList();
                return output;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetListReportDaily", ex.ToString());
                return null;
            }
        }

    }
}
