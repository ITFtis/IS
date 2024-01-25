using Dou;
using Dou.Controllers;
using Dou.Help;
using Dou.Misc.Attr;
using Dou.Models.DB;
using IS.Models;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace IS.Controllers.Manager
{
    [MenuDef(Name = "使用者管理", MenuPath = "系統管理", Action = "Index", Index = 1, Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class UserController : UserBaseControll<User, Role>
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        bool IsSsoAuth = true;
        //const string SsoServer = "https://pj.ftis.org.tw/Sample/Sso/";

        //舊SSO
        //const string SsoServer = "https://pj4.ftis.org.tw/SsoTest/";
        //string SsoLogin = SsoServer;//"http://localhost:49286/"; //AD
        //string SsoLogoff = SsoServer + "Auth/Logoff"; //AD
        //string SsoGetUser = SsoServer + "Auth/UserInfo";//AD


        //Azure AD
        const string SsoServer = "https://pj4.ftis.org.tw/Auth/";
        string SsoLogin = SsoServer + "Account/SignIn"; //AD
        string SsoLogoff = SsoServer + "Account/SignOut";//AD
        string SsoGetUser = SsoServer + "Account/UserInfo";//AD



        /// <summary>
        /// login remember me
        /// </summary>
        /// <param name="user"></param>
        /// <param name="returnUrl"></param>
        /// <param name="redirectLogin"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(User user, string returnUrl, bool redirectLogin = false, bool sso = true)
        {
            if (user.Id == null && User.Identity.IsAuthenticated)//記憶20分鐘自動登入
            {
                user.Id = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(f => f.Type == "Id")?.Value;
                if (user.Id != null)
                {
                    user = GetModelEntity().Find(user.Id) ?? user;
                    redirectLogin = false;
                }
            }
            //SSO
            if (IsSsoAuth && sso)
            {
                //取sso token
                var _ssotoken = HttpContext.Request.QueryString["ssotoken"];
                //有token(以驗證)
                if (_ssotoken != null)
                {
                    //取驗證使用者資料
                    var ssou = GetUserInfoSSO(_ssotoken);
                    if ((bool)ssou.Success)
                    {
                        dynamic ssouser = ssou.User;
                        string ssouid = ssouser.Mno.Value+"";//員編
                        string ssouname = ssouser.Name.Value + "";//姓名
                       // string ssoupw = ssouser.Password.Value + "";
                        string ssouemail = ssouser.EMail.Value + ""; //EMail
                        string ssoudc = ssouser.DCode_.Value + "";//部門代碼
                       // string ssoudn = (ssouser.Department as JObject)["DName"].Value<string>();//部門名稱
                       // string pw = (int.Parse(ssoupw) - 13579) / 4 + "";
                       string pw ="";
                        User u = FindUser(ssouid);//已驗證，故直接取系統使用者
                        redirectLogin = false;
                        if (u != null)
                        {
                            user = u;
                            //更新本身系統user
                            if (ssoudc != u.Dep || ssouemail != u.EMail || !Dou.Context.Config.VerifyPassword(u.Password, pw))
                            {
                                u.Dep = ssoudc;
                                u.EMail = ssouemail;
                                u.Password = Dou.Context.Config.PasswordEncode(pw);
                                this.UpdateDBObject(GetModelEntity(), new User[] { u });
                            }
                        }
                        else//系統尚無此使用者
                        {
                            
                            user = new User() { Id = ssouid, Name = ssouname, Password = Dou.Context.Config.PasswordEncode(pw), EMail = ssouemail, Dep = ssoudc , Enabled = false};
                            this.AddDBObject(GetModelEntity(), new User[] { user });
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = ssou.Desc;
                    }
                }
                else
                {
                    //導向sso驗證
                    return new RedirectResult(SsoLogin + "?redirectLogin=true&returnUrl=" + HttpUtility.UrlEncode(HttpContext.Request.Url + ""));
                }
            }

            ActionResult v = base.DouLogin(user, returnUrl, redirectLogin);

            if (ViewBag.ErrorMessage == null && user.Id != null && !User.Identity.IsAuthenticated)//login remember me
            {
                var identity = new ClaimsIdentity(new[] {new Claim("Id", user.Id)
                                    }, IS.Startup.ApplicationCookieAppName);

                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false,//rememberMe,
                    AllowRefresh = true
                }, identity);
            }
            if (IsSsoAuth && ViewBag.ErrorMessage != null)
            {
                ViewBag.LoginUrl = Dou.Context.Config.LoginPage;
                ViewBag.LogoffUrl = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext).Action("DouLogoff", "User");
                return PartialView("DouLoginError", user);//, new { u=user, msg= ViewBag.ErrorMessage });
            }
            else
            {
                if(v is RedirectResult || v is RedirectToRouteResult)
                {
                    DouUnobtrusiveSession.Session.Add("CurrentFtisUser", FtisHelper.DB.Hepler.GetEmployeeIncludeDepartment(user.Id));
                }
                return v is RedirectResult || v is RedirectToRouteResult ? v : PartialView(user);
            }
        }
       
        internal static FtisHelper.DB.Model.Employee CurrentFtisEmployee
        {
            get { return DouUnobtrusiveSession.Session["CurrentFtisUser"] as FtisHelper.DB.Model.Employee; }
        }

        dynamic GetUserInfoSSO(string token)
        {

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(SsoGetUser+"?token=" + token);
            request.Method = System.Net.WebRequestMethods.Http.Get;

            try
            {
                using (var response = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        return Newtonsoft.Json.Linq.JObject.Parse(reader.ReadToEnd()); ;
                    }
                }
            }
            catch (Exception ex)
            {
                dynamic obj = new System.Dynamic.ExpandoObject();
                obj.Success = false;
                obj.Desc = ex.Message;
                return obj;
            }
        }

        protected override Dou.Models.DB.IModelEntity<User> GetModelEntity()
        {
            return new ModelEntity<User>(RoleController._dbContext); //與RoleController._dbContext共用這樣cache的RoleUsers才會一致
        }
        public override ActionResult DouLogoff()
        {
            DouUnobtrusiveSession.Session.Remove("CurrentFtisUser");
            HttpContext.GetOwinContext().Authentication.SignOut(IS.Startup.ApplicationCookieAppName); //清除login remember me
            var sd  =User.Identity.IsAuthenticated;
            if (IsSsoAuth)
            {
                base.DouLogoff();
                var returnurl = Context.Config.LoginPage;
                Logger.Log.For(this).Info("returnurl:" + returnurl);
                if (!returnurl.ToLower().StartsWith("http"))
                {
                    var logoffUrl =  new UrlHelper(HttpContext.Request.RequestContext).Action("DouLogoff", "User");
                    returnurl = HttpContext.Request.Url.AbsoluteUri.Replace(logoffUrl, returnurl);
                }
                Logger.Log.For(this).Info("returnur2:" + returnurl);
                return Redirect(SsoLogoff+"?returnUrl=" + HttpUtility.UrlEncode( returnurl));
            }else
                return base.DouLogoff();
        }
    }
}