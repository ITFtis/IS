using Dou.Controllers;
using Dou.Misc.Attr;
using Dou.Models.DB;
using IS.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IS.Controllers.Base
{
    [MenuDef(Name = "訊息來源", MenuPath = "基本資料", Action = "Index",  Func = FuncEnum.ALL, AllowAnonymous = false)]
    //[AutoLogger(Content = AutoLoggerAttribute.LogContent.All)]
    public class InfoSourceController : AGenericModelController<InfoSource>
    {
        // GET: ConsultMethod
        public ActionResult Index()
        {
            return View();
        }
        protected override void UpdateDBObject(IModelEntity<InfoSource> dbEntity, IEnumerable<InfoSource> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            InfoSourceSelectItemsClassImp.Reset();
        }
        protected override void AddDBObject(IModelEntity<InfoSource> dbEntity, IEnumerable<InfoSource> objs)
        {
            base.AddDBObject(dbEntity, objs);
            InfoSourceSelectItemsClassImp.Reset();
        }
        protected override void DeleteDBObject(IModelEntity<InfoSource> dbEntity, IEnumerable<InfoSource> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            InfoSourceSelectItemsClassImp.Reset();
        }
        protected override IModelEntity<InfoSource> GetModelEntity()
        {
            return new ModelEntity<InfoSource>(Manager.RoleController._dbContext);
        }
    }
}