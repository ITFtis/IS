using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using IS.Models.Prj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IS.Controllers.Prj
{
    [MenuDef(Name = "諮詢查詢", MenuPath = "諮詢資料", Action = "Index", Index = 4, Func = FuncEnum.None, AllowAnonymous = false)]
    [AutoLogger(Content = AutoLoggerAttribute.LogContent.AssignContent, AssignContent = "編號:{No},狀態:{Status}]")]
    public class ConsultQueryController : ConsultBaseController
    {
        // GET: ConsultQuery
        public ActionResult Index()
        {
            return View();
        }
        public override DataManagerOptions GetDataManagerOptions()
        {
            var options= base.GetDataManagerOptions();
            options.viewable = true;
            return options;
        }
    }
}