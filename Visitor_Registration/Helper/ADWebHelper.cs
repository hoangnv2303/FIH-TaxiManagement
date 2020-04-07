using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VisitorRegistration.Models;
using Visitor_Registration.Models;
using VisitorRegistration.Helper;

namespace EquipmentRegistation.Helper
{
    public class ADWebHelper
    {
        private string ADWeb_URI = @"http://idmgt.fushan.fihnbb.com";
        private string CLIENT_ID = @"vEwSNKfc6USHiiyLA3sR7DuKFFrUlbJVm";
        private string CLIENT_SECRET = @"F1uk98jcuxlUoQPAFsqfrJYF3AZwBm7a";
        private string CLIENT_REDIRECT_URL = @"http://localhost:50349/login/success";
        public static string URI_ADWEB_SEARCH = "/adweb/record/search/v1";

        public ADWebHelper()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ADWeb_URI"]))
                ADWeb_URI = ConfigurationManager.AppSettings["ADWeb_URI"];

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CLIENT_ID"]))
                CLIENT_ID = ConfigurationManager.AppSettings["CLIENT_ID"];

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CLIENT_SECRET"]))
                CLIENT_SECRET = ConfigurationManager.AppSettings["CLIENT_SECRET"];

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CLIENT_REDIRECT_URL"]))
                CLIENT_REDIRECT_URL = ConfigurationManager.AppSettings["CLIENT_REDIRECT_URL"];
        }

        public string GetAccessToken(string _code)
        {
            try
            {
                var res = (ADWeb_URI + "/adweb/oauth2/access_token/v1")
                    .PostUrlEncodedAsync(new
                    {
                        client_id = CLIENT_ID,
                        redirect_uri = CLIENT_REDIRECT_URL,
                        client_secret = CLIENT_SECRET,
                        code = _code,
                        grant_type = "authorization_code"
                    }).ReceiveString().Result;
                dynamic obj = JsonConvert.DeserializeObject<object>(res);
                if (!string.IsNullOrEmpty(obj.access_token.ToString()))
                    return obj.access_token.ToString();
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public UserModel getUserInfo(string access_token)
        {
            try
            {
                var res = (ADWeb_URI + "/adweb/people/me/v1")
                    .WithOAuthBearerToken(access_token)
                    .GetStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<UserModel>(res);
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public void SendMail(string access_token)
        //{
        //    try
        //    {
        //        var res = (ADWeb_URI + "/adweb/email/create/v1")
        //           .WithOAuthBearerToken(access_token)
        //           .PostUrlEncodedAsync(new
        //           {
        //               mail_subject = "Send Mail",
        //               mail_body = @"Dear Mr./Ms.<br><br>
        //                    You have one approval request level 2 in Taxi Management System.< br >< br >
        //                    Please check it out here: http://visitor-registration-dev.fushan.fihnbb.com/approval <br><br>
        //                    Regards,< br > Taxi Management System<br>< br >",
        //               recipients = "hoangnv@fih-foxconn.com"
        //           }).Result;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public EmployeeModel GetDetailUserInfo(string access_token, string ad)
        {
            try
            {
                var res = (ADWeb_URI + URI_ADWEB_SEARCH)
                   .WithOAuthBearerToken(access_token)
                   .SetQueryParams(new
                   {
                       model = "hr.employee",
                       fields = "[\"id\", \"name\",\"ad_user_employeeID\", \"ad_user_displayName\", \"work_email\", \"job_title\", \"ad_user_sAMAccountName\", \"parent_id\", \"department_id\"]",
                       search_datas = "[('ad_user_employeeID', '=', '" + ad + "')]"
                       //search_datas = "[('ad_user_sAMAccountName', '=', '" + ad + "')]"
                   })
                   .GetStringAsync().Result;

                var data = JsonConvert.DeserializeObject<List<CommonModel>>(res);
                if (data.Count == 2 && data[1].data != null)
                {
                    dynamic _data = data[1].data[0][0];
                    if (_data.job_title == "Head of Factory")
                    {
                        var _obj = new EmployeeModel
                        {
                            id = _data.id,
                            name = _data.name,
                            ad_user_displayName = _data.ad_user_displayName,
                            ad_user_employeeID = _data.ad_user_employeeID,
                            ad_user_sAMAccountName = _data.ad_user_sAMAccountName,
                            job_title = _data.job_title,
                            work_email = _data.work_email,
                            department_id = new List<object> { "0", "Fushan Factory" },
                            parent_id = new List<object> { _data.id, _data.ad_user_displayName },
                        };
                        return _obj;
                    }
                    var temp = JsonConvert.DeserializeObject<EmployeeModel>(data[1].data[0][0].ToString());
                    return temp;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public EmployeeModel GetDetailUserInfoByID(string access_token, int id)
        {
            try
            {
                var res = (ADWeb_URI + URI_ADWEB_SEARCH)
                   .WithOAuthBearerToken(access_token)
                   .SetQueryParams(new
                   {
                       model = "hr.employee",
                       fields = "[\"id\", \"name\",\"ad_user_employeeID\", \"ad_user_displayName\", \"work_email\", \"job_title\", \"ad_user_sAMAccountName\", \"department_id\"]",
                       search_datas = "[('id', '=', " + id + ")]"
                   })
                   .GetStringAsync().Result;

                var data = JsonConvert.DeserializeObject<List<CommonModel>>(res);
                if (data.Count == 2 && data[1].data != null)
                {
                    dynamic _data = data[1].data[0][0];
                    if (_data.job_title == "Head of Factory")
                    {
                        var _obj = new EmployeeModel
                        {
                            id = _data.id,
                            name = _data.name,
                            ad_user_displayName = _data.ad_user_displayName,
                            ad_user_employeeID = _data.ad_user_employeeID,
                            ad_user_sAMAccountName = _data.ad_user_sAMAccountName,
                            job_title = _data.job_title,
                            work_email = _data.work_email,
                            department_id = new List<object> { "0", "Fushan Factory" },
                            parent_id = new List<object> { _data.id, _data.ad_user_displayName },
                        };
                        return _obj;
                    }
                    var temp = JsonConvert.DeserializeObject<EmployeeModel>(data[1].data[0][0].ToString());
                    return temp;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public EmployeeModel GetHeadOfFunction(string access_token, string dept)
        {
            try
            {
                var res = (ADWeb_URI + URI_ADWEB_SEARCH)
                 .WithOAuthBearerToken(access_token)
                 .SetQueryParams(new
                 {
                     model = "hr.department",
                     fields = "[\"id\", \"ad_department_code\", \"manager_id\"]",
                     search_datas = "[('id', '!=', 21),( 'name', '=', '" + dept + "'),('manager_id', '!=', False)]"
                 })
                 .GetStringAsync().Result;

                var data = JsonConvert.DeserializeObject<List<CommonModel>>(res);
                if (data.Count == 2 && data[1].data != null)
                {
                    dynamic obj = data[1].data[0];
                    return GetDetailUserInfoByID(access_token, (int)obj[0].manager_id[0]);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DepartmentModel> GetDepartments(string access_token)
        {
            try
            {
                List<DepartmentModel> list = new List<DepartmentModel>();
                var res = (ADWeb_URI + MyConstants.URI_ADWEB_SEARCH)
                  .WithOAuthBearerToken(access_token)
                  .SetQueryParams(new
                  {
                      model = "hr.department",
                      fields = "[\"id\", \"name\"]",
                      search_datas = "[('id', '!=', " + 21 + "),(\"parent_id\", \"=\", False)]"
                  })
                  .GetStringAsync().Result;

                var data = JsonConvert.DeserializeObject<List<CommonModel>>(res);
                if (data.Count == 2 && data[1].data != null)
                {
                    foreach (var item in data[1].data)
                    {
                        dynamic temp = item[0];
                        list.Add(new DepartmentModel
                        {
                            id = Convert.ToInt32(temp.id.ToString()),
                            name = temp.name.ToString()
                        });
                    }
                    return list.OrderBy(x => x.name).ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}