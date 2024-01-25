using Dou.Controllers;
using Dou.Models.DB;
using IS.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IS.Controllers.Base
{
    public class DepBaseController : AGenericModelController<DepBase>
    {
        // GET: ConsultMethod
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<DepBase> GetModelEntity()
        {
            return new ModelEntity<DepBase>(Manager.RoleController._dbContext);
        }
        internal IQueryable<DepBase> GetAllData()
        {
            return GetDataDBObject(GetModelEntity(), null) as IQueryable<DepBase>;
        }
        internal ModelEntity<DepBase> GetInstanceModelEntity()
        {
            return new ModelEntity<DepBase>(Manager.RoleController._dbContext);
        }
    }
}