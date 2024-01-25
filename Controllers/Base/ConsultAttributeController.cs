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
    [MenuDef(Name = "諮詢屬性", MenuPath = "基本資料", Action = "Index", Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class ConsultAttributeController : AGenericModelController<ConsultAttribute>
    {
        // GET: ConsultMethod
        public ActionResult Index()
        {
            return View();
        }
        protected override void AddDBObject(IModelEntity<ConsultAttribute> dbEntity, IEnumerable<ConsultAttribute> objs)
        {
            base.AddDBObject(dbEntity, objs);
            ConsultAttributeSelectItemsClassImp.Reset();
        }
        protected override void UpdateDBObject(IModelEntity<ConsultAttribute> dbEntity, IEnumerable<ConsultAttribute> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            ConsultAttributeSelectItemsClassImp.Reset();
        }
        protected override void DeleteDBObject(IModelEntity<ConsultAttribute> dbEntity, IEnumerable<ConsultAttribute> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            ConsultAttributeSelectItemsClassImp.Reset();
        }
        protected override IModelEntity<ConsultAttribute> GetModelEntity()
        {
            return new ModelEntity<ConsultAttribute>(Manager.RoleController._dbContext);
        }
    }
}