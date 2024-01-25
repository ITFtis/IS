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
    [MenuDef(Name = "諮詢分類", MenuPath = "基本資料", Action = "Index", Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class ConsultTypeController : AGenericModelController<ConsultType>
    {
        // GET: ConsultMethod
        public ActionResult Index()
        {
            return View();
        }
        protected override void DeleteDBObject(IModelEntity<ConsultType> dbEntity, IEnumerable<ConsultType> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            ConsultTypeSelectItemsClassImp.Reset();
        }
        protected override void AddDBObject(IModelEntity<ConsultType> dbEntity, IEnumerable<ConsultType> objs)
        {
            base.AddDBObject(dbEntity, objs);
            ConsultTypeSelectItemsClassImp.Reset();
        }
        protected override void UpdateDBObject(IModelEntity<ConsultType> dbEntity, IEnumerable<ConsultType> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            ConsultTypeSelectItemsClassImp.Reset();
        }
        protected override IModelEntity<ConsultType> GetModelEntity()
        {
            return new ModelEntity<ConsultType>(Manager.RoleController._dbContext);
        }
    }
}