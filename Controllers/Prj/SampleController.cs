using Dou.Controllers;
using Dou.Misc.Attr;
using Dou.Models.DB;
using IS.Models;
using IS.Models.Prj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IS.Controllers.Prj
{
    //[MenuDef(Name = "資料範例1", MenuPath = "", Action = "Index", Index = 0, Func = FuncEnum.ALL, AllowAnonymous = false)]
    //[AutoLogger(Content = AutoLoggerAttribute.LogContent.KeyOnly)]
    public class SampleController : AGenericModelController<Sample>
    {
        // GET: Sample
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Sample> GetModelEntity()
        {
            return new ModelEntity<Sample>(new DouImpModelContext());
        }
    }
}