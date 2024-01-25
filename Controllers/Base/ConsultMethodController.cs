using Dou.Controllers;
using Dou.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using IS.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IS.Controllers.Prj
{
    [MenuDef(Name = "諮詢方式", MenuPath = "基本資料", Action = "Index", Func = FuncEnum.ALL, AllowAnonymous = false)]
    public class ConsultMethodController :  AGenericModelController<ConsultMethod>
    {
        // GET: ConsultMethod
        public ActionResult Index()
        {
            return View();
        }
        //public override Task<ActionResult> Update(IEnumerable<ConsultMethod> objs, bool cache = true)
        //{
        //    var sss = HelperUtilities.GetKeyValues<ConsultMethod>(objs.First(), (GetModelEntity() as ModelEntity<ConsultMethod>)._context);
        //    return base.Update(objs, cache);
        //}
        protected override void AddDBObject(IModelEntity<ConsultMethod> dbEntity, IEnumerable<ConsultMethod> objs)
        {
            base.AddDBObject(dbEntity, objs);
            ConsultMethodSelectItemsClassImp.Reset();
        }
        protected override void UpdateDBObject(IModelEntity<ConsultMethod> dbEntity, IEnumerable<ConsultMethod> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            ConsultMethodSelectItemsClassImp.Reset();
        }
        protected override void DeleteDBObject(IModelEntity<ConsultMethod> dbEntity, IEnumerable<ConsultMethod> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            ConsultMethodSelectItemsClassImp.Reset();
        }
        protected override IModelEntity<ConsultMethod> GetModelEntity()
        {
            return new ModelEntity<ConsultMethod>(Manager.RoleController._dbContext);
        }
        public override Task<ActionResult> GetData(params KeyValueParams[] paras)
        {
            return base.GetData(paras);
        }
    }
}