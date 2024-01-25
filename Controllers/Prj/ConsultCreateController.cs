using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using IS.Controllers.Manager;
using IS.Models;
using IS.Models.Prj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IS.Controllers.Prj
{
    [MenuDef(Id = "ConsultCreateController", Name = "諮詢通報", MenuPath = "諮詢資料", Action = "Index", Index = 0, Func = FuncEnum.Add | FuncEnum.Update | FuncEnum.Delete, AllowAnonymous = false)]
    [AutoLogger(Content = AutoLoggerAttribute.LogContent.AssignContent, AssignContent = "編號:{No},狀態:{Status}]")]
    public class ConsultCreateController : ConsultBaseController
    {
        // GET: ConsultCreate
        public ActionResult Index()
        {
            return View();
        }
        protected override IQueryable<ConsultRecord> BeforeIQueryToPagedList(IQueryable<ConsultRecord> iquery, params KeyValueParams[] paras)
        {
            if (!IsHeightPermissionUser)
            {
                //僅能看本人新增資料
                var currentuserid = UserController.CurrentFtisEmployee.Mno;// Dou.Context.CurrentUser<User>().Id;
                iquery = iquery.Where(s => s.RecordEmpId == currentuserid);
            }
            return base.BeforeIQueryToPagedList(iquery,paras);
        }
        public override DataManagerOptions GetDataManagerOptions()
        {
            var options= base.GetDataManagerOptions();
            return options;
        }
    }
}