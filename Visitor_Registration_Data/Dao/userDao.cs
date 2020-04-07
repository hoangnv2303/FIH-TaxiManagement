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
    public class UserDao
    {
        VisitorRegistration_Model db = null;
        public UserDao()
        {
            db = new VisitorRegistration_Model();
        }

        //Insert or update
        public string InsertOrUpdateUser(tbl_User user)
        {
            try
            {
                var userFind = db.tbl_User.FirstOrDefault(x => x.EmployeeId == user.EmployeeId);
                if (userFind == null)
                {
                    db.tbl_User.Add(user);
                    db.SaveChanges();
                    return user.EmployeeId;
                }
                else
                {
                    userFind.EmployeeId = user.EmployeeId;
                    userFind.FushanAd = user.FushanAd;
                    userFind.FullName = user.FullName;
                    userFind.Email = user.Email;
                    userFind.JobTitle = user.JobTitle;
                    userFind.DeparmentName = user.DeparmentName;
                    db.SaveChanges();
                    return userFind.EmployeeId;
                }
            }
            catch (Exception ex)
            {
                WriteLogError.Write("InsertOrUpdateUser", ex.ToString());
                return "false";
            }

        }
        public bool InsertUserRole(tbl_User_Role userRole)
        {
            try
            {
                var checkRole = db.tbl_User_Role.FirstOrDefault(x => x.EmployeeId == userRole.EmployeeId && x.Role_Id == userRole.Role_Id);
                if (checkRole == null)
                {
                    db.tbl_User_Role.Add(userRole);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("InsertUserRole", ex.ToString());
                return false;
            }

        }

        public List<tbl_User> GetListUser(List<string> listUser)
        {
            try
            {
                return db.tbl_User.Where(x => listUser.Contains(x.EmployeeId)).ToList();
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetListUser", ex.ToString());
                return null;
            }
        }

        public bool DeleteUserRole(string userId, int role)
        {
            try
            {
                var result = db.tbl_User_Role.FirstOrDefault(x => x.Role_Id == role && x.EmployeeId == userId);
                db.tbl_User_Role.Remove(result);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("DeleteUserRole", ex.ToString());
                return false;
            }
        }
        public List<EmployeeModelS> GetListUserInfor()
        {
            try
            {
                var result = (from a in db.tbl_User
                              join b in db.tbl_User_Role on a.EmployeeId equals b.EmployeeId
                              join c in db.tbl_Role on b.Role_Id equals c.Id
                              where c.Name != "Approver"
                              select new EmployeeModelS
                              {
                                  EmployeeId = a.EmployeeId,
                                  FullName = a.FullName,
                                  Name = c.Name,
                              }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetListUserInfor", ex.ToString());
                return null;
            }
        }

        public List<string> GetListCredential(string userId)
        {
            try
            {
                var result = db.Database.SqlQuery<string>("p_getCredential @employeeId",
                  new SqlParameter("@employeeId", userId)
                  ).ToList();
                return result;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("DeleteUserRole", ex.ToString());
                return null;
            }
        }

        public List<tbl_BlackList> GetBlackList()
        {
            try
            {
                return db.tbl_BlackList.ToList();
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetBlackList", ex.ToString());
                return null;
            }
        }

        public bool DeleteBlackList(string nationalId)
        {
            try
            {
                var result = db.tbl_BlackList.FirstOrDefault(x => x.NationalId == nationalId);
                db.tbl_BlackList.Remove(result);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("DeleteBlackList", ex.ToString());
                return false;
            }
        }
        public bool InsertOrUpdateBlackList(tbl_BlackList blackList)
        {
            try
            {
                var result = db.tbl_BlackList.FirstOrDefault(x => x.NationalId == blackList.NationalId);
                if (result != null)
                {
                    result.VisitorName = blackList.VisitorName;
                    result.CompanyName = blackList.CompanyName;
                    result.Reason = blackList.Reason;
                    result.CreateBy = blackList.CreateBy;
                    result.CreateDate = blackList.CreateDate;
                }
                else
                {
                    db.tbl_BlackList.Add(blackList);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("InsertOrUpdateBlackList", ex.ToString());
                return false;
            }
        }

        public string GetUserByEmployeeId(string employeeId)
        {
            try
            {
                return db.tbl_User.SingleOrDefault(x => x.EmployeeId == employeeId.Trim()).EmployeeId;
            }
            catch (Exception ex)
            {
                WriteLogError.Write("GetUserByEmployeeId", ex.ToString());
                return null;
            }
        }
    }
}
