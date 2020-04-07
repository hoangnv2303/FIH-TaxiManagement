using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor_Registration_Data.EF;

namespace Visitor_Registration_Data.Dao
{
    public class Taxi_UserDao
    {
        VisitorRegistration_Model db = null;
        public Taxi_UserDao()
        {
            db = new VisitorRegistration_Model();
        }

        public bool InsertOrUpdateUser(tbl_Taxi_User_Infor user)
        {
            try
            {
                var userCheck = db.tbl_Taxi_User_Infor.FirstOrDefault(x => x.Taxi_Request_Infor_Id == user.Taxi_Request_Infor_Id
                && x.EmployeeId == user.EmployeeId && x.Process == user.Process
                );
                if (userCheck == null)
                {
                    db.tbl_Taxi_User_Infor.Add(user);
                }
                else
                {
                    // Update item
                    if (user.Cost == null && user.CarNumber == null)
                    {
                        if (user.CardNumber != null && userCheck.CreateBy == null)
                        {
                            userCheck.CreateDate = user.CreateDate;
                            userCheck.CreateBy = user.CreateBy;
                        }
                    }
                    userCheck.CarNumber = user.CarNumber;
                    userCheck.SwipeTime = user.SwipeTime;
                    userCheck.CardNumber = user.CardNumber;
                    userCheck.Cost = user.Cost;
                    userCheck.RefNumber = user.RefNumber;
                    userCheck.UpdateDate = user.UpdateDate;
                    userCheck.UpdateBy = user.UpdateBy;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("InsertOrUpdateUser", ex.ToString());
                return false;
            }
        }

        public bool UpdateBaseInformation(tbl_Taxi_User_Infor visitor, string employeeIdOld, string dropOff2)
        {
            try
            {
                var visitorUpdate = db.tbl_Taxi_User_Infor.Where(x => x.Taxi_Request_Infor_Id == visitor.Taxi_Request_Infor_Id && x.EmployeeId == employeeIdOld);
                if (visitorUpdate == null)
                {
                    return false;
                }
                else
                {
                    foreach (var item in visitorUpdate)
                    {
                        item.Name = visitor.Name;
                        item.SLM_Name = visitor.SLM_Name;
                        item.EmployeeId = visitor.EmployeeId;
                        item.Remark = visitor.Remark;

                        // Check when dropOff2 != null
                        if (item.Process == 0 && (dropOff2 != "" && dropOff2 != null))
                        {
                            item.Process = 1;
                            // add new users
                            visitor.Process = 2;
                            db.tbl_Taxi_User_Infor.Add(visitor);
                        }
                    }
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("UpdateBaseInformation", ex.ToString());
                return false;
            }
        }

        public bool RemoveUserByRequestId(Guid id)
        {
            try
            {
                var visitor = db.tbl_Taxi_User_Infor.Where(x => x.Taxi_Request_Infor_Id == id);
                db.tbl_Taxi_User_Infor.RemoveRange(visitor);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("RemoveUserByRequestId", ex.ToString());
                return false;
            }
        }


        public bool RemoveUserByEmployeeId(Guid id, string employeeId)
        {
            try
            {
                var visitor = db.tbl_Taxi_User_Infor.Where(x => x.Taxi_Request_Infor_Id == id && x.EmployeeId == employeeId);
                db.tbl_Taxi_User_Infor.RemoveRange(visitor);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("RemoveVisitorByEmployeeId", ex.ToString());
                return false;
            }

        }
    }
}
