using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor_Registration_Data.EF;
using Common;

namespace Visitor_Registration_Data.Dao
{
    public class VisitorInforDao
    {
        VisitorRegistration_Model db = null;
        public VisitorInforDao()
        {
            db = new VisitorRegistration_Model();
        }

        public bool UpdateBaseInformation(tbl_Visitor_Infor visitor, string nationalIdOld, string employee)
        {
            try
            {
                var visitorUpdate = db.tbl_Visitor_Infor.FirstOrDefault(x => x.Request_Infor_Id == visitor.Request_Infor_Id && x.NationalId == visitor.NationalId);
                if (visitorUpdate == null)
                {
                    var visitorOld = db.tbl_Visitor_Infor.FirstOrDefault(x => x.Request_Infor_Id == visitor.Request_Infor_Id && x.NationalId == nationalIdOld);
                    db.tbl_Visitor_Infor.Remove(visitorOld);
                    db.tbl_Visitor_Infor.Add(visitor);
                }
                else
                {
                    visitorUpdate.VisitorName = visitor.VisitorName;
                    visitorUpdate.CompanyName = visitor.CompanyName;
                    visitorUpdate.NationalId = visitor.NationalId;
                    visitorUpdate.Remark = visitor.Remark;
                    visitorUpdate.UpdateDate = DateTime.Now;
                    visitorUpdate.UpdateBy = employee;
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

        public bool InsertOrUpdateVisitorInfor(tbl_Visitor_Infor visitor)
        {
            try
            {
                var checkExists = db.tbl_Visitor_Infor.FirstOrDefault(x => x.Request_Infor_Id == visitor.Request_Infor_Id && x.NationalId == visitor.NationalId);
                if (checkExists == null)
                {
                    db.tbl_Visitor_Infor.Add(visitor);
                }
                else
                {
                    checkExists.Badge = visitor.Badge;
                    checkExists.Access = visitor.Access;
                    checkExists.Tem = visitor.Tem;
                    checkExists.MealCard = visitor.MealCard;
                    if (visitor.EntryTime != null) // update when exit
                    {
                        checkExists.EntryTime = visitor.EntryTime;
                    }
                    checkExists.ExitTime = visitor.ExitTime;
                    checkExists.Package = visitor.Package;
                    checkExists.EscorterId = visitor.EscorterId;
                    if (visitor.Remark != null && visitor.Remark != "")
                    {
                        checkExists.Remark = visitor.Remark;
                    }
                    checkExists.UpdateDate = visitor.UpdateDate;
                    checkExists.UpdateBy = visitor.UpdateBy;
                    checkExists.BadgeReturn = visitor.BadgeReturn;
                    checkExists.AccessReturn = visitor.AccessReturn;
                    checkExists.MealCardReturn = visitor.MealCardReturn;
                    // Update
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("InsertOrUpdateVisitorInfor", ex.ToString());
                return false;
            }
        }

        public bool RemoveVisitorByRequestId(Guid id)
        {
            try
            {
                var visitor = db.tbl_Visitor_Infor.Where(x => x.Request_Infor_Id == id);
                db.tbl_Visitor_Infor.RemoveRange(visitor);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("RemoveVisitorByRequestId", ex.ToString());
                return false;
            }
        }

        public bool RemoveVisitorByNationalId(Guid id, string nationalId)
        {
            try
            {
                var visitor = db.tbl_Visitor_Infor.FirstOrDefault(x => x.NationalId == nationalId && x.Request_Infor_Id == id);
                db.tbl_Visitor_Infor.Remove(visitor);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("RemoveVisitorByNationalId", ex.ToString());
                return false;
            }
        }
    }
}
