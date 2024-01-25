using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(IS.Startup))]

namespace IS
{
    public class Startup 
    {
        static internal string ApplicationCookieAppName= null;
        public void Configuration(IAppBuilder app)
        {
            

            // 如需如何設定應用程式的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkID=316888
            var sd = System.Environment.MachineName;
            bool isDebug = true;// System.Environment.MachineName.StartsWith("090");
            //var sd = Flood.FloodCal.DeSerialize<MenuDefAttribute[]>(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Config"), "DouMenuExt.xml"));
            Dou.Context.Init(new Dou.DouConfig
            {
                //SystemManagerDBConnectionName = "DouModelContextExt",
                DefaultPassword = "3922",
                PasswordEncode = (p) =>
                {
                    //return (int.Parse(p) * 4 + 13579) + "";
                    return System.Web.Helpers.Crypto.HashPassword(p);
                },
                VerifyPassword = (ep, vp) =>
                {
                    //int pint = 0;
                    //bool tp = int.TryParse(vp, out pint);
                    //if(!tp)
                    //    return false;
                    //else
                    //{
                    //    return ep == (pint * 4 + 13579) + "";
                    //}
                    
                    return System.Web.Helpers.Crypto.VerifyHashedPassword(ep, vp);
                },
                //LoggerExpired=13,
                SessionTimeOut = 20,
                SqlDebugLog = isDebug,
                AllowAnonymous = false,
                LoginPage = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext).Action("Login", "User"),
                //LoginPage = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext).Action("DouLogin", "User"),
                LoggerListen = (log) =>
                {
                    if (log.WorkItem == Dou.Misc.DouErrorHandler.ERROR_HANDLE_WORK_ITEM)
                    {
                        Debug.WriteLine("DouErrorHandler發出的錯誤!!\n" + log.LogContent);
                        Logger.Log.For(null).Error("DouErrorHandler發出的錯誤!!\n" + log.LogContent);
                    }
                }
            });
            ApplicationCookieAppName = "ApplicationCookie" + Dou.Context.Config.AppName;
            //login Remember Me 用 
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = ApplicationCookieAppName,// "ApplicationCookie" + Dou.Context.Config.AppName,
                LoginPath = new PathString("/User/Login"),
                Provider = new CookieAuthenticationProvider(),
                ExpireTimeSpan = TimeSpan.FromMinutes(Dou.Context.Config.SessionTimeOut)
            });
        }
    }
}
