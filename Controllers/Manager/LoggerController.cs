using Dou.Controllers;
using Dou.Misc.Attr;
using Dou.Models.DB;
using IS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IS.Controllers.Manager
{
    [MenuDef(Name = "記錄查詢", MenuPath = "系統管理", Action = "Index", Index = 4, Func = FuncEnum.ALL, AllowAnonymous = false)]
    [AutoLogger(Content = AutoLoggerAttribute.LogContent.All )]
    public class LoggerController : LoggerBaseController<Dou.Models.Logger>
    {
        // GET: Logger
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Dou.Models.Logger> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<Dou.Models.Logger>(new DouImpModelContext());
        }
    }
}